import { useState } from "react";
import { Bell } from "lucide-react";
import "../styles/Notifications.css";

const notificationsMock = [
  {
    id: 1,
    title: "Novo vinho disponível",
    description: "O produtor Quinta do Vale lançou um novo tinto.",
    time: "há 5 min",
    unread: true,
  },
  {
    id: 2,
    title: "Pedido enviado",
    description: "O seu pedido #1234 foi enviado.",
    time: "há 1 h",
    unread: false,
  },
];

const Notifications = () => {
  const [open, setOpen] = useState(false);

  return (
    <div className="notifications-wrapper">
      <button
        className="notifications-btn"
        onClick={() => setOpen(!open)}
        aria-label="Notificações"
      >
        <Bell size={22} />
        <span className="notification-dot"></span>
      </button>

      {open && (
        <div className="notifications-dropdown">
          <div className="notifications-header">Notificações</div>

          <ul className="notifications-list">
            {notificationsMock.map((n) => (
              <li
                key={n.id}
                className={`notification-item ${n.unread ? "unread" : ""}`}
              >
                <div className="notification-title">{n.title}</div>
                <div className="notification-desc">{n.description}</div>
                <div className="notification-time">{n.time}</div>
              </li>
            ))}
          </ul>

          <div className="notifications-footer">Ver todas</div>
        </div>
      )}
    </div>
  );
};

export default Notifications;
