import { useEffect, useState, useCallback } from "react";
import {
  getNotificacoes,
  marcarNotificacaoComoLida,
} from "../api/notificacoes.service";

export const useNotifications = (getAccessTokenSilently) => {
  const [notifications, setNotifications] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchNotificacoes = useCallback(async () => {
    try {
      const token = await getAccessTokenSilently();
      const res = await getNotificacoes(token);
      setNotifications(res.data || []);
    } catch (err) {
    } finally {
      setLoading(false);
    }
  }, [getAccessTokenSilently]);

  useEffect(() => {
    fetchNotificacoes();
  }, [fetchNotificacoes]);

  const markAsRead = async (id) => {
    try {
      const token = await getAccessTokenSilently();
      await marcarNotificacaoComoLida(id, token);
      setNotifications((prev) =>
        prev.map((n) => (n.id === id ? { ...n, lida: true } : n))
      );
      window.dispatchEvent(new CustomEvent("notification:read"));
    } catch (err) {
    }
  };

  return { notifications, setNotifications, loading, markAsRead };
};
