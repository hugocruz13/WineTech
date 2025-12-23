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

const API_URL = import.meta.env.VITE_API_URL;

/* ===== FORMATAR DADOS IOT (FETCH INICIAL) ===== */
const formatData = (apiData) => ({
  temperatura: (apiData?.temperatura || []).map((t) => ({
    dataHora: new Date(t.dataHora).getTime(),
    temperatura: Number(t.temperatura.toFixed(2)),
  })),
  humidade: (apiData?.humidade || []).map((h) => ({
    dataHora: new Date(h.dataHora).getTime(),
    humidade: Number(h.humidade.toFixed(2)),
  })),
  luminosidade: (apiData?.luminosidade || []).map((l) => ({
    dataHora: new Date(l.dataHora).getTime(),
    luminosidade: Number(l.luminosidade.toFixed(2)),
  })),
});

const TIPOS_VALIDOS = ["temperatura", "humidade", "luminosidade"];

const GerirAdegaPage = () => {
  const { id: adegaId } = useParams();
  const { getAccessTokenSilently } = useAuth0();
  const [dispositivos, setDispositivos] = useState([]);

  const [iot, setIot] = useState({
    temperatura: [],
    humidade: [],
    luminosidade: [],
  });

  const [showSelectVinho, setShowSelectVinho] = useState(false);
  const [showAddSensor, setShowAddSensor] = useState(false);

  const navigate = useNavigate();
  const [showDeleteAdega, setShowDeleteAdega] = useState(false);


  const [stock, setStock] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchStock = async () => {
    const token = await getAccessTokenSilently();

    const res = await fetch(
      `${API_URL}/api/adega/${adegaId}/stock`,
      {
        headers: { Authorization: `Bearer ${token}` },
      }
    );

    const json = await res.json();
    setStock(json.data || []);
  };

  const addSensor = async (sensor) => {
    try {
      const token = await getAccessTokenSilently();

      const res = await fetch(`${API_URL}/api/Sensores`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(sensor),
      });

      if (!res.ok) throw new Error("Erro ao criar sensor");

      const json = await res.json();

      setDispositivos((prev) => [...prev, json.data]);
    } catch (err) {
      console.error("Erro ao adicionar sensor:", err);
    }
  };

  const deleteAdega = async () => {
    try {
      const token = await getAccessTokenSilently();

      const res = await fetch(
        `${API_URL}/api/Adega/${adegaId}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (!res.ok) throw new Error("Erro ao apagar adega");

      // Fecha modal e redireciona
      setShowDeleteAdega(false);
      navigate("/dashboard");
    } catch (err) {
      console.error("Erro ao apagar adega:", err);
    }
  };


  /* ===== FETCH INICIAL ===== */
  useEffect(() => {
    const fetchAll = async () => {
      try {
        const token = await getAccessTokenSilently();

        const [iotRes, stockRes, dispositivosRes] = await Promise.all([
          fetch(`${API_URL}/api/leituras/${adegaId}/leituras/adega`, {
            headers: { Authorization: `Bearer ${token}` },
          }),
          fetch(`${API_URL}/api/adega/${adegaId}/stock`, {
            headers: { Authorization: `Bearer ${token}` },
          }),
          fetch(`${API_URL}/api/sensores/${adegaId}`, {
            headers: { Authorization: `Bearer ${token}` },
          }),
        ]);

        const iotJson = await iotRes.json();
        const stockJson = await stockRes.json();
        const dispositivosJson = await dispositivosRes.json();

        setIot(formatData(iotJson.data));
        setStock(stockJson.data);
        setDispositivos(dispositivosJson.data);
      } catch (err) {
        console.error("Erro fetch inicial:", err);
      } finally {
        setLoading(false);
      }
    };

    if (adegaId) fetchAll();
  }, [adegaId, getAccessTokenSilently]);

  /* ===== UPDATE STOCK ===== */
  const updateStock = async (vinhoId, novaQuantidade) => {
    try {
      const token = await getAccessTokenSilently();

      const res = await fetch(
        `${API_URL}/api/Adega/${adegaId}/stock/${vinhoId}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            vinhoId,
            adegaId: Number(adegaId),
            quantidade: novaQuantidade,
          }),
        }
      );

      if (!res.ok) throw new Error("Erro ao atualizar stock");

      setStock((prev) =>
        prev.map((v) =>
          v.vinhoId === vinhoId ? { ...v, quantidade: novaQuantidade } : v
        )
      );
    } catch (err) {
      console.error("Erro update stock:", err);
    }
  };

  /* ===== APAGAR STOCK ===== */
  const deleteStock = async (vinhoId) => {
    try {
      const token = await getAccessTokenSilently();

      const res = await fetch(
        `${API_URL}/stock/${vinhoId}`,
        {
          method: "DELETE",
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (!res.ok) throw new Error("Erro ao apagar stock");

      // Remove o vinho da lista no frontend
      setStock((prev) => prev.filter((v) => v.vinhoId !== vinhoId));
    } catch (err) {
      console.error("Erro ao apagar stock:", err);
    }
  };


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
          if (Number(leitura.adegaId) !== Number(adegaId)) {
            return;
          }

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
  }, [adegaId, getAccessTokenSilently]);

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

  const addToStock = async (vinho) => {
    try {
      const token = await getAccessTokenSilently();

      const res = await fetch(
        `${API_URL}/api/Adega/${adegaId}/stock`,
        {
          method: "POST",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            vinhoId: vinho.id,
            adegaId: Number(adegaId),
            quantidade: 1,
          }),
        }
      );

      if (!res.ok) throw new Error("Erro ao adicionar vinho");

      await fetchStock();
    } catch (err) {
      console.error("Erro ao adicionar ao stock:", err);
    }
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

                      await fetch(`${API_URL}/api/dispositivos/${d.id}/toggle`, {
                        method: "PUT",
                        headers: {
                          Authorization: `Bearer ${token}`,
                        },
                      });

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
        onConfirm={deleteAdega}
      />

    </>
  );
};

export default GerirAdegaPage;
