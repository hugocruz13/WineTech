import { useEffect, useState } from "react";
import { getNotificacoes } from "../api/notificacoes.service";

export const useUnreadNotifications = (getAccessTokenSilently) => {
  const [unreadCount, setUnreadCount] = useState(0);

  useEffect(() => {
    const fetchUnread = async () => {
      try {
        const token = await getAccessTokenSilently();
        const res = await getNotificacoes(token);
        const unread = (res.data || []).filter((n) => !n.lida).length;
        setUnreadCount(unread);
      } catch (err) {
        console.error("Erro ao buscar notificações", err);
      }
    };

    fetchUnread();
  }, [getAccessTokenSilently]);

  useEffect(() => {
    const onNotification = () => setUnreadCount((c) => c + 1);
    window.addEventListener("notification:received", onNotification);
    return () =>
      window.removeEventListener("notification:received", onNotification);
  }, []);

  useEffect(() => {
    const onRead = () => setUnreadCount((c) => Math.max(0, c - 1));
    window.addEventListener("notification:read", onRead);
    return () => window.removeEventListener("notification:read", onRead);
  }, []);

  return unreadCount;
};
