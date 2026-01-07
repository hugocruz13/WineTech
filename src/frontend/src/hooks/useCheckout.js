import { useEffect, useState } from "react";
import { getCarrinhoDetalhes } from "../api/cart.service";
import { finalizarCompra } from "../api/compra.service";

export const useCheckout = (getAccessTokenSilently) => {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [comprando, setComprando] = useState(false);

  useEffect(() => {
    const fetchCompra = async () => {
      try {
        const token = await getAccessTokenSilently();
        const result = await getCarrinhoDetalhes(token);
        setItems(result.data ?? result);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchCompra();
  }, [getAccessTokenSilently]);

  const handleFinalizarCompra = async ({ cardNumber, mes, ano }) => {
    try {
      setComprando(true);
      const token = await getAccessTokenSilently();
      await finalizarCompra(
        { cardNumber, mes: Number(mes), ano: Number(ano) },
        token
      );
    } catch (err) {
      console.error("Erro ao finalizar compra:", err);
    } finally {
      setComprando(false);
    }
  };

  return { items, loading, error, comprando, handleFinalizarCompra, setError };
};
