import { useEffect, useState } from "react";
import { getLeiturasStock } from "../api/iot.service";

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

export const useIotLeituras = (stockId, getAccessTokenSilently) => {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchIot = async () => {
      try {
        const token = await getAccessTokenSilently();
        const res = await getLeiturasStock(stockId, token);
        setData(formatData(res.data));
      } catch {
        setError("Não foi possível carregar dados IoT");
      } finally {
        setLoading(false);
      }
    };

    if (stockId) fetchIot();
  }, [stockId, getAccessTokenSilently]);

  return { data, loading, error };
};
