import { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { Bell, Check, X } from "lucide-react";
import Header from "../components/Header";
import "../styles/NotificationPage.css";

const API_URL = import.meta.env.VITE_API_URL;

const NotificationsPage = () => {
  const [notifications, setNotifications] = useState([]);
  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const fetchNotificacoes = async () => {
      const token = await getAccessTokenSilently();

      const res = await fetch(`${API_URL}/utilizador/notificacoes`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      const json = await res.json();
      setNotifications(json.data || []);
    };

    fetchNotificacoes();
  }, [getAccessTokenSilently]);

  return (
    <>
      <Header />

      <main className="notifications-page">
        <header className="notifications-header">
          <div className="title">
            <Bell size={24} />
            <h1>Notificações</h1>
          </div>
        </header>

        {notifications.length === 0 && (
          <p className="empty">Sem notificações</p>
        )}

        <ul className="page-notifications-list">
          {notifications.map((n) => (
            <li key={n.id} className={`page-item ${!n.lida ? "unread" : ""}`}>
              <div className="icon">
                <Bell size={16} />
              </div>

              <div className="content">
                <p className="message">{n.mensagem}</p>

                {!n.lida && (
                  <div className="actions">
                    <button className="primary">
                      <Check size={14} />
                      Marcar como lida
                    </button>
                    <button className="ghost">
                      <X size={14} />
                      Dispensar
                    </button>
                  </div>
                )}
              </div>

              <span className="time">
                {new Date(n.createdAt).toLocaleString("pt-PT")}
              </span>
            </li>
          ))}
        </ul>
      </main>
    </>
  );
};

export default NotificationsPage;
