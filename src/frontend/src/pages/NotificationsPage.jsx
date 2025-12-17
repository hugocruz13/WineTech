import { useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Bell } from "lucide-react";

import Header from "../components/Header";
import { notificationConfig } from "../utils/notificationConfig";
import "../styles/NotificationPage.css";

const API_URL = import.meta.env.VITE_API_URL;

const NotificationsPage = ({ notifications, setNotifications }) => {
  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const fetchNotificacoes = async () => {
      try {
        const token = await getAccessTokenSilently();

        const res = await fetch(`${API_URL}/api/utilizador/notificacoes`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        const json = await res.json();
        setNotifications(json.data || []);
      } catch (err) {
        console.error("Erro ao buscar notificações", err);
      }
    };

    if (notifications.length === 0) {
      fetchNotificacoes();
    }
  }, [getAccessTokenSilently, notifications.length, setNotifications]);

  const handleMarkAsRead = async (id) => {
    try {
      const token = await getAccessTokenSilently();

      await fetch(`${API_URL}/api/utilizador/notificacoes/${id}/lida`, {
        method: "PUT",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      setNotifications((prev) =>
        prev.map((n) => (n.id === id ? { ...n, lida: true } : n))
      );

      window.dispatchEvent(new CustomEvent("notification:read"));
    } catch (err) {
      console.error("Erro ao marcar notificação como lida", err);
    }
  };

  return (
    <>
      <Header />

      <main className="notifications-page">
        <header className="notifications-header">
          <div>
            <h1>Notificações</h1>
            <p>Gerencie suas atualizações de pedidos e ofertas.</p>
          </div>
        </header>

        {notifications.length === 0 && (
          <p className="empty">Sem notificações</p>
        )}

        <ul className="page-notifications-list">
          {notifications.map((n) => {
            const config = notificationConfig[n.tipo] || {};
            const Icon = config.icon || Bell;

            return (
              <li
                key={n.id}
                className={`page-item ${config.className} ${
                  !n.lida ? "unread" : ""
                }`}
              >
                <div className="icon">
                  <Icon size={18} />
                </div>

                <div className="content">
                  <div className="content-header">
                    <h3>{n.titulo}</h3>
                    <span className="time">
                      {new Date(n.createdAt).toLocaleString("pt-PT", {
                        dateStyle: "short",
                        timeStyle: "short",
                      })}
                    </span>
                  </div>

                  <p>{n.mensagem}</p>

                  {!n.lida && (
                    <button
                      className="mark-read"
                      onClick={() => handleMarkAsRead(n.id)}
                    >
                      Marcar como lida
                    </button>
                  )}
                </div>
              </li>
            );
          })}
        </ul>
      </main>
    </>
  );
};

export default NotificationsPage;
