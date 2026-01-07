import { useEffect, useState } from "react";
import { getCompras } from "../api/compra.service";

export const useCompras = (getAccessTokenSilently) => {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCompras = async () => {
      try {
        const token = await getAccessTokenSilently();
        const res = await getCompras(token);
        if (res?.success) {
          setOrders(res.data || []);
        } else if (Array.isArray(res)) {
          setOrders(res);
        } else {
          throw new Error("Resposta inválida da API");
        }
      } catch (err) {
        console.error("Erro ao buscar compras", err);
        setError("Não foi possível carregar as encomendas.");
      } finally {
        setLoading(false);
      }
    };

    fetchCompras();
  }, [getAccessTokenSilently]);

  return { orders, loading, error };
};
