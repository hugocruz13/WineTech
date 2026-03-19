import { useAuth0 } from "@auth0/auth0-react";
import { Bell } from "lucide-react";

import Header from "../components/Header";
import Loading from "../components/Loading";
import { notificationConfig } from "../utils/notificationConfig";
import styles from "../styles/NotificationPage.module.css";
import { useNotifications } from "../hooks/useNotifications";

const NotificationsPage = () => {
  const { getAccessTokenSilently } = useAuth0();
  const { notifications, loading, markAsRead } =
    useNotifications(getAccessTokenSilently);

  if (loading) {
    return <Loading />;
  }

  return (
    <>
      <Header />

      <main className={styles.notificationsPage}>
        <header className={styles.notificationsHeader}>
          <div>
            <h1>Notificações</h1>
            <p>Gerencie suas atualizações de pedidos e ofertas.</p>
          </div>
        </header>

        {notifications.length === 0 && (
          <p className={styles.empty}>Sem notificações</p>
        )}

        <ul className={styles.pageNotificationsList}>
          {notifications.map((n) => {
            const config = notificationConfig[n.tipo] || {};
            const Icon = config.icon || Bell;

            return (
              <li
                key={n.id}
                className={[
                  styles.pageItem,
                  styles[config.className],
                  !n.lida && styles.unread,
                ]
                  .filter(Boolean)
                  .join(" ")}
              >
                <div className={styles.icon}>
                  <Icon size={18} />
                </div>

                <div className={styles.content}>
                  <div className={styles.contentHeader}>
                    <h3>{n.titulo}</h3>
                    <span className={styles.time}>
                      {new Date(n.createdAt).toLocaleString("pt-PT", {
                        dateStyle: "short",
                        timeStyle: "short",
                      })}
                    </span>
                  </div>

                  <p>{n.mensagem}</p>

                  {!n.lida && (
                    <button
                      className={styles.markRead}
                      onClick={() => markAsRead(n.id)}
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
