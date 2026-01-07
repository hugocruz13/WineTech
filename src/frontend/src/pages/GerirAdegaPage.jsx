import { useParams, useNavigate } from "react-router-dom";
import { useEffect, useState } from "react";
import { Droplets, Sun, Thermometer, Plus, Trash2 } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import * as signalR from "@microsoft/signalr";

import Header from "../components/Header";
import Loading from "../components/Loading";
import IotLineChart from "../components/IoT/IotLineChart";
import IotCard from "../components/IoT/IotCard";
import VinhoCard from "../components/VinhoCard";
import DispositivoCard from "../components/DispositivoCard";
import SelecionarVinhoModal from "../components/SelecionarVinhoModal";
import AdicionarSensorModal from "../components/AdicionarSensorModal";
import ConfirmDialog from "../components/ConfirmDialog";


import styles from "../styles/GerirAdegaPage.module.css";
import { useGerirAdega, TIPOS_VALIDOS } from "../hooks/useGerirAdega";
import { toggleDispositivo } from "../api/adega.service";

const API_URL = import.meta.env.VITE_API_URL;

const GerirAdegaPage = () => {
  const { id: adegaId } = useParams();
  const { getAccessTokenSilently } = useAuth0();
  const {
    dispositivos,
    setDispositivos,
    iot,
    setIot,
    stock,
    loading,
    addSensor,
    deleteAdega,
    updateStock,
    deleteStock,
    addToStock,
  } = useGerirAdega(adegaId, getAccessTokenSilently);
  const [showSelectVinho, setShowSelectVinho] = useState(false);
  const [showAddSensor, setShowAddSensor] = useState(false);

  const navigate = useNavigate();
  const [showDeleteAdega, setShowDeleteAdega] = useState(false);



  /* ===== REAL TIME (SIGNALR) ===== */
  useEffect(() => {
    let connection;

    const startRealtime = async () => {
      try {
        const token = await getAccessTokenSilently();

        connection = new signalR.HubConnectionBuilder()
          .withUrl(`${API_URL}/hubs/leituras`, {
            accessTokenFactory: () => token,
          })
          .withAutomaticReconnect()
          .build();

        connection.on("ReceiveLeitura", (leitura) => {
          if (Number(leitura.adegaId) !== Number(adegaId)) return;

          setIot((prev) => {
            const tipo = leitura.tipo.toLowerCase();
            if (!TIPOS_VALIDOS.includes(tipo)) return prev;

            const timestamp = new Date(leitura.dataHora).getTime();

            const novoPonto =
              tipo === "temperatura"
                ? { dataHora: timestamp, temperatura: leitura.valor }
                : tipo === "humidade"
                  ? { dataHora: timestamp, humidade: leitura.valor }
                  : { dataHora: timestamp, luminosidade: leitura.valor };

            return {
              ...prev,
              [tipo]: [...prev[tipo], novoPonto].slice(-50),
            };
          });
        });

        await connection.start();
        console.log(" SignalR conectado");
      } catch (err) {
        console.error("Erro SignalR:", err);
      }
    };

    startRealtime();

    return () => {
      if (connection) connection.stop();
    };
  }, [adegaId, getAccessTokenSilently, setIot]);

  const handleDeleteAdega = async () => {
    try {
      await deleteAdega();
      setShowDeleteAdega(false);
      navigate("/dashboard");
    } catch (err) {
      console.error("Erro ao apagar adega:", err);
    }
  };

  const getDispositivoConfig = (tipo) => {
    const t = tipo.toLowerCase();

    if (t === "temperatura") {
      return {
        icon: <Thermometer color="#2563eb" size={20} />,
        ativo: iot.temperatura.length > 0,
      };
    }

    if (t === "humidade") {
      return {
        icon: <Droplets color="#16a34a" size={20} />,
        ativo: iot.humidade.length > 0,
      };
    }

    if (t === "luminosidade") {
      return {
        icon: <Sun color="#f59e0b" size={20} />,
        ativo: iot.luminosidade.length > 0,
      };
    }

    return {
      icon: null,
      ativo: false,
    };
  };



  if (loading) return <Loading />;

  const hasIotData =
    iot.temperatura.length ||
    iot.humidade.length ||
    iot.luminosidade.length;

  return (
    <>
      <Header />

      <div className={styles.page}>
        <div className={styles.title}>
          <h2>Adega Dashboard</h2>

          <button
            className={styles.deletePill}
            onClick={() => setShowDeleteAdega(true)}
          >
            <Trash2 size={16} />
            Apagar Adega
          </button>
        </div>



        {hasIotData ? (
          <div className={styles.cards}>
            <IotCard
              title="Temperatura"
              icon={<Thermometer color="#2563eb" size={20} />}
            >
              <IotLineChart
                data={iot.temperatura}
                dataKey="temperatura"
                unit="°C"
                color="#2563eb"
              />
            </IotCard>

            <IotCard
              title="Humidade"
              icon={<Droplets color="#16a34a" size={20} />}
            >
              <IotLineChart
                data={iot.humidade}
                dataKey="humidade"
                unit="%"
                color="#16a34a"
              />
            </IotCard>

            <IotCard
              title="Luminosidade"
              icon={<Sun color="#f59e0b" size={20} />}
            >
              <IotLineChart
                data={iot.luminosidade}
                dataKey="luminosidade"
                unit="lx"
                color="#f59e0b"
              />
            </IotCard>
          </div>
        ) : (
          <div className={styles.noData}>
            Não existem dados IoT disponíveis para esta adega.
          </div>
        )}


        {/* ===== DISPOSITIVOS ===== */}
        <div className={styles.stock}>
          <div className={styles.stockHeader}>
            <h3>Controlo de Sensores</h3>

            {dispositivos.length < 3 && (
              <button
                className={styles.new}
                onClick={() => setShowAddSensor(true)}
              >
                <Plus size={16} />
                Add Sensor
              </button>
            )}

          </div>

          <div className={styles.dispositivosContainer}>
            {dispositivos.map((d) => {
              const { icon, ativo } = getDispositivoConfig(d.tipo);

              return (
                <DispositivoCard
                  key={d.id}
                  tipo={d.tipo}
                  estado={ativo}
                  icon={icon}
                  onToggle={async () => {
                    try {
                      const token = await getAccessTokenSilently();
                      await toggleDispositivo(d.id, token);

                      setDispositivos((prev) =>
                        prev.map((x) =>
                          x.id === d.id ? { ...x, estado: !x.estado } : x
                        )
                      );
                    } catch (err) {
                      console.error("Erro ao alternar dispositivo", err);
                    }
                  }}
                />
              );
            })}
          </div>
        </div>


        {/* ===== STOCK ===== */}
        <div className={styles.stock}>
          <div className={styles.stockHeader}>
            <h3>Stock de Vinhos</h3>
            <button
              className={styles.new}
              onClick={() => setShowSelectVinho(true)}
            >
              <Plus size={16} />
              Add Product
            </button>

          </div>

          <div className={styles.table}>
            <div className={styles.tableHeader}>
              <span>Produto</span>
              <span>Categoria</span>
              <span>Anos</span>
              <span>Estado</span>
              <span>Quantidade</span>
              <span>Ações</span>
            </div>

            {stock.map((v) => (
              <VinhoCard
                key={v.vinhoId}
                nome={v.nome}
                imagemUrl={v.imagemUrl}
                categoria={v.tipo}
                ano={v.ano || "—"}
                quantidade={v.quantidade}
                onDelete={() => deleteStock(v.vinhoId)}
                onSave={(novaQtd) => updateStock(v.vinhoId, novaQtd)}
              />
            ))}
          </div>
        </div>
      </div>

      {showSelectVinho && (
        <SelecionarVinhoModal
          adegaId={adegaId}
          onClose={() => setShowSelectVinho(false)}
          onSelect={async (vinho) => {
            await addToStock(vinho);
            setShowSelectVinho(false);
          }}
        />
      )}

      {showAddSensor && (
        <AdicionarSensorModal
          adegaId={adegaId}
          onClose={() => setShowAddSensor(false)}
          onAdd={addSensor}
        />
      )}

      <ConfirmDialog
        open={showDeleteAdega}
        title="Apagar Adega"
        message="Tem a certeza que deseja apagar esta adega? Esta ação é irreversível."
        onCancel={() => setShowDeleteAdega(false)}
        onConfirm={handleDeleteAdega}
      />

    </>
  );
};

export default GerirAdegaPage;
