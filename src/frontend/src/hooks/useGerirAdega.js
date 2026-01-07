import { useEffect, useState, useCallback } from "react";
import {
  getLeiturasAdega,
  getAdegaStock,
  getSensores,
  addStock,
  updateStockQuantidade,
  deleteStockItem,
  createSensor,
  deleteAdegaById,
  toggleDispositivo,
} from "../api/adega.service";

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

export const TIPOS_VALIDOS = ["temperatura", "humidade", "luminosidade"];

export const useGerirAdega = (adegaId, getAccessTokenSilently) => {
  const [dispositivos, setDispositivos] = useState([]);
  const [iot, setIot] = useState({
    temperatura: [],
    humidade: [],
    luminosidade: [],
  });
  const [stock, setStock] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchStock = useCallback(async () => {
    const token = await getAccessTokenSilently();
    const res = await getAdegaStock(adegaId, token);
    setStock(res.data || res);
  }, [adegaId, getAccessTokenSilently]);

  const init = useCallback(async () => {
    if (!adegaId) return;
    try {
      const token = await getAccessTokenSilently();
      const [iotRes, stockRes, dispositivosRes] = await Promise.all([
        getLeiturasAdega(adegaId, token),
        getAdegaStock(adegaId, token),
        getSensores(adegaId, token),
      ]);

      setIot(formatData(iotRes.data));
      setStock(stockRes.data || stockRes);
      setDispositivos(dispositivosRes.data || dispositivosRes);
    } catch (err) {
      console.error("Erro fetch inicial:", err);
    } finally {
      setLoading(false);
    }
  }, [adegaId, getAccessTokenSilently]);

  useEffect(() => {
    init();
  }, [init]);

  const addSensor = async (sensor) => {
    try {
      const token = await getAccessTokenSilently();
      const res = await createSensor(sensor, token);
      setDispositivos((prev) => [...prev, res.data || res]);
    } catch (err) {
      console.error("Erro ao adicionar sensor:", err);
    }
  };

  const deleteAdega = async () => {
    const token = await getAccessTokenSilently();
    await deleteAdegaById(adegaId, token);
  };

  const updateStock = async (vinhoId, novaQuantidade) => {
    try {
      const token = await getAccessTokenSilently();
      await updateStockQuantidade(adegaId, vinhoId, novaQuantidade, token);
      setStock((prev) =>
        prev.map((v) =>
          v.vinhoId === vinhoId ? { ...v, quantidade: novaQuantidade } : v
        )
      );
    } catch (err) {
      console.error("Erro update stock:", err);
    }
  };

  const deleteStock = async (vinhoId) => {
    try {
      const token = await getAccessTokenSilently();
      await deleteStockItem(vinhoId, token);
      setStock((prev) => prev.filter((v) => v.vinhoId !== vinhoId));
    } catch (err) {
      console.error("Erro ao apagar stock:", err);
    }
  };

  const addToStock = async (vinho) => {
    try {
      const token = await getAccessTokenSilently();
      await addStock(adegaId, vinho.id, 1, token);
      await fetchStock();
    } catch (err) {
      console.error("Erro ao adicionar ao stock:", err);
    }
  };

  const toggleDevice = async (dispositivoId) => {
    const token = await getAccessTokenSilently();
    await toggleDispositivo(dispositivoId, token);
  };

  return {
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
    fetchStock,
    toggleDevice,
  };
};
