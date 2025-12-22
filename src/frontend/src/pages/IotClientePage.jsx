import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { Droplets, Sun, Thermometer } from "lucide-react";
import styles from "../styles/IotClientePage.module.css";
import Loading from "../components/Loading";
import Header from "../components/Header";
import IotCard from "../components/IoT/IotCard";
import IotLineChart from "../components/IoT/IotLineChart";
import IotStats from "../components/IoT/IotStats";
import { useAuth0 } from "@auth0/auth0-react";

const API_URL = import.meta.env.VITE_API_URL;

const formatData = (apiData) => ({
  temperatura: (apiData.temperatura || []).map((t) => ({
    dataHora: t.dataHora,
    temperatura: Number(t.temperatura.toFixed(2)),
  })),

  humidade: (apiData.humidade || []).map((h) => ({
    dataHora: h.dataHora,
    humidade: Number(h.humidade.toFixed(2)),
  })),

  luminosidade: (apiData.luminosidade || []).map((l) => ({
    dataHora: l.dataHora,
    luminosidade: Number(l.luminosidade.toFixed(2)),
  })),
});

const IotClientePage = () => {
  const { stockId } = useParams();
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const fetchIot = async () => {
      try {
        const token = await getAccessTokenSilently();

        const res = await fetch(
          `${API_URL}/api/Leituras/${stockId}/leituras/stock`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (!res.ok) throw new Error();

        const json = await res.json();
        setData(formatData(json.data));
      } catch {
        setError("Não foi possível carregar dados IoT");
      } finally {
        setLoading(false);
      }
    };

    fetchIot();
  }, [stockId, getAccessTokenSilently]);

  if (loading) return <Loading />;
  if (error) return <p className={styles.error}>{error}</p>;
  if (!data) return null;

  return (
    <>
      <Header />
      <div className={styles.page}>
        {/* -------- TEMPERATURA -------- */}
        <div className={styles.bigCard}>
          <div className={styles.bigHeader}>
            <div className={styles.headerTitle}>
              <Thermometer size={18} color="#2563eb" />
              <h3>Temperatura ao longo do tempo</h3>
            </div>

            <IotStats data={data.temperatura} dataKey="temperatura" unit="°C" />
          </div>

          <IotLineChart
            data={data.temperatura}
            dataKey="temperatura"
            unit="°C"
            color="#2563eb"
          />
        </div>

        {/* -------- HUMIDADE + LUMINOSIDADE -------- */}
        <div className={styles.grid}>
          <IotCard
            title="Humidade ao longo do tempo"
            icon={<Droplets color="#16a34a" />}
          >
            <IotLineChart
              data={data.humidade}
              dataKey="humidade"
              unit="%"
              color="#16a34a"
            />
          </IotCard>

          <IotCard
            title="Luminosidade ao longo do tempo"
            icon={<Sun color="#f59e0b" />}
          >
            <IotLineChart
              data={data.luminosidade}
              dataKey="luminosidade"
              unit="lx"
              color="#f59e0b"
            />
          </IotCard>
        </div>
      </div>
    </>
  );
};

export default IotClientePage;
