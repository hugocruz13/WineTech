import { useEffect, useState, useCallback } from "react";
import {
  getCarrinhoDetalhes,
  removerDoCarrinho,
  incrementarQuantidade,
  decrementarQuantidade,
} from "../api/cart.service";

export const useCarrinho = (getAccessTokenSilently) => {
  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const carregarCarrinho = useCallback(async () => {
    try {
      const token = await getAccessTokenSilently();
      const result = await getCarrinhoDetalhes(token);
      setItems(result.data ?? result);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, [getAccessTokenSilently]);

  useEffect(() => {
    carregarCarrinho();
  }, [carregarCarrinho]);

  const remover = async (vinhoId) => {
    try {
      const token = await getAccessTokenSilently();
      await removerDoCarrinho(vinhoId, token);
      setItems((prev) => prev.filter((item) => item.vinhosId !== vinhoId));
    } catch (err) {
      console.error("Erro ", err);
    }
  };

  const adicionarQuantidade = async (vinhosId) => {
    try {
      const token = await getAccessTokenSilently();
      await incrementarQuantidade(vinhosId, 1, token);
      setItems((prev) =>
        prev.map((item) =>
          item.vinhosId === vinhosId
            ? { ...item, quantidade: item.quantidade + 1 }
            : item
        )
      );
    } catch (err) {
      console.error("Erro ", err);
    }
  };

  const removerQuantidade = async (vinhosId) => {
    try {
      const token = await getAccessTokenSilently();
      await decrementarQuantidade(vinhosId, 1, token);
      setItems((prev) =>
        prev
          .map((item) =>
            item.vinhosId === vinhosId
              ? { ...item, quantidade: item.quantidade - 1 }
              : item
          )
          .filter((item) => item.quantidade > 0)
      );
    } catch (err) {
      console.error("Erro ", err);
    }
  };

  return {
    items,
    loading,
    error,
    remover,
    adicionarQuantidade,
    removerQuantidade,
  };
};
