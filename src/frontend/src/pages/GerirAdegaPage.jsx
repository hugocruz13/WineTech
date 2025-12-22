import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Droplets, Sun, Thermometer, Plus } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import * as signalR from "@microsoft/signalr";

import Header from "../components/Header";
import Loading from "../components/Loading";
import IotLineChart from "../components/IoT/IotLineChart";
import IotCard from "../components/IoT/IotCard";
import VinhoCard from "../components/VinhoCard";

import styles from "../styles/GerirAdegaPage.module.css";

const API_URL = import.meta.env.VITE_API_URL;

/* ===== MAPA SENSOR → TIPO ===== */
const SENSOR_MAP = {
  1: "temperatura",
  2: "humidade",
  3: "luminosidade",
};

/* ===== FORMATAR DADOS IOT ===== */
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

const GerirAdegaPage = () => {
  const { id: adegaId } = useParams();
  const { getAccessTokenSilently } = useAuth0();

  const [iot, setIot] = useState(null);
  const [stock, setStock] = useState([]);
  const [loading, setLoading] = useState(true);

  /* ===== FETCH INICIAL ===== */
  useEffect(() => {
    const fetchAll = async () => {
      try {
        const token = await getAccessTokenSilently();

        const [iotRes, stockRes] = await Promise.all([
          fetch(`${API_URL}/api/leituras/${adegaId}/leituras/adega`, {
            headers: { Authorization: `Bearer ${token}` },
          }),
          fetch(`${API_URL}/api/adega/${adegaId}/stock`, {
            headers: { Authorization: `Bearer ${token}` },
          }),
        ]);

        const iotJson = await iotRes.json();
        const stockJson = await stockRes.json();

        setIot(formatData(iotJson.data));
        setStock(stockJson.data);
      } catch (err) {
        console.error(err);
      } finally {
        setLoading(false);
      }
    };

    if (adegaId) fetchAll();
  }, [adegaId, getAccessTokenSilently]);

  /* ===== REAL TIME (HUB CERTO) ===== */
  useEffect(() => {
    if (!iot) return;

    let connection;

    const startRealtime = async () => {
      const token = await getAccessTokenSilently();

      connection = new signalR.HubConnectionBuilder()
        .withUrl(`${API_URL}/hubs/leituras`, {
          accessTokenFactory: () => token,
        })
        .withAutomaticReconnect()
        .build();

      connection.on("ReceiveLeitura", (leitura) => {
        console.log("Leitura recebida:", leitura);

        setIot((prev) => {
          if (!prev) return prev;

          const tipo = SENSOR_MAP[leitura.sensorId];
          if (!tipo) return prev;

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
      console.log("📡 SignalR (leituras) conectado");
    };

    startRealtime();

    return () => {
      if (connection) {
        connection.stop();
        console.log(" SignalR (leituras) desligado");
      }
    };
  }, [iot, getAccessTokenSilently]);

  if (loading) return <Loading />;

  const hasIotData =
    iot &&
    iot.temperatura.length &&
    iot.humidade.length &&
    iot.luminosidade.length;

  return (
    <>
      <Header />

      <div className={styles.page}>
        <div className={styles.title}>
          <h2>Adega Dashboard</h2>
        </div>

        {hasIotData ? (
          <div className={styles.cards}>
            <IotCard title="Temperatura" icon={<Thermometer color="#2563eb" />}>
              <IotLineChart
                data={iot.temperatura}
                dataKey="temperatura"
                unit="°C"
                color="#2563eb"
              />
            </IotCard>

            <IotCard title="Humidade" icon={<Droplets color="#16a34a" />}>
              <IotLineChart
                data={iot.humidade}
                dataKey="humidade"
                unit="%"
                color="#16a34a"
              />
            </IotCard>

            <IotCard title="Luminosidade" icon={<Sun color="#f59e0b" />}>
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

        {/* STOCK */}
        <div className={styles.stock}>
          <div className={styles.stockHeader}>
            <h3>Stock de Vinhos</h3>
            <button className={styles.new}>
              <Plus size={16} />
              Add Product
            </button>
          </div>

          <div className={styles.table}>
            <div className={styles.tableHeader}>
              <span>Product</span>
              <span>Category</span>
              <span>Vintage</span>
              <span>Status</span>
              <span>Quantity</span>
              <span>Actions</span>
            </div>

            {stock.map((v) => (
              <VinhoCard
                key={v.vinhoId}
                nome={v.nome}
                imagemUrl={v.imagemUrl}
                categoria={v.tipo}
                ano={v.ano || "—"}
                quantidade={v.quantidade}
                onEdit={() => console.log("edit", v.vinhoId)}
              />
            ))}
          </div>
        </div>
      </div>
    </>
  );
};

export default GerirAdegaPage;
