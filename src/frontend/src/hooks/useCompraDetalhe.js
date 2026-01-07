import { useEffect, useState } from "react";
import { getCompraDetalhe } from "../api/compra.service";
import { checkLeiturasDisponiveis } from "../api/iot.service";

export const useCompraDetalhe = (id, getAccessTokenSilently) => {
  const [dados, setDados] = useState([]);
  const [iotDisponivel, setIotDisponivel] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCompra = async () => {
      try {
        const token = await getAccessTokenSilently();
        const res = await getCompraDetalhe(id, token);
        if (res?.success) setDados(res.data || []);
      } catch (err) {
        setError("Não foi possível carregar a encomenda.");
      } finally {
        setLoading(false);
      }
    };

    fetchCompra();
  }, [id, getAccessTokenSilently]);

  useEffect(() => {
    if (!dados.length) return;

    const carregarIot = async () => {
      const resultado = {};
      await Promise.all(
        dados.map(async (item) => {
          const token = await getAccessTokenSilently();
          try {
            const res = await checkLeiturasDisponiveis(item.stockId, token);
            resultado[item.stockId] = res?.success && res.data === true;
          } catch {
            resultado[item.stockId] = false;
          }
        })
      );

      setIotDisponivel(resultado);
    };

    carregarIot();
  }, [dados, getAccessTokenSilently]);

  return { dados, iotDisponivel, loading, error };
};
