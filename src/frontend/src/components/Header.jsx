import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { Search, ShoppingCart, Wine, Warehouse, Bell } from "lucide-react";

import ProfileDropdown from "./ProfileDropdown";
import RoleVisibility from "./RoleVisibility";
import "../styles/Header.css";

const API_URL = import.meta.env.VITE_API_URL;

const Header = () => {
  const [openMenu, setOpenMenu] = useState(null);
  const [unreadCount, setUnreadCount] = useState(0);
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();

  const testNotification = async () => {
    try {
      const token = await getAccessTokenSilently();

      const res = await fetch(`${API_URL}/api/utilizador/notificacoes/teste`, {
        method: "GET",
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      const text = await res.text();
      console.log(text);
    } catch (err) {
      console.error("Erro ao testar notificação", err);
    }
  };

  useEffect(() => {
    const fetchUnreadNotifications = async () => {
      try {
        const token = await getAccessTokenSilently();

        const res = await fetch(`${API_URL}/api/utilizador/notificacoes`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        const json = await res.json();

        const unread = (json.data || []).filter((n) => !n.lida).length;

        setUnreadCount(unread);
      } catch (err) {
        console.error("Erro ao buscar notificações", err);
      }
    };

    fetchUnreadNotifications();
  }, [getAccessTokenSilently]);

  return (
    <header className="header-container">
      <div className="logo-section" onClick={() => navigate("/")}>
        <div className="logo-icon">
          <Wine size={28} strokeWidth={2.5} />
        </div>
        <h1 className="logo-text">VinhaTech</h1>
      </div>

      <div className="search-section">
        <div className="search-bar">
          <Search className="search-icon" size={20} />
          <input
            type="text"
            placeholder="Procurar vinhos, produtores..."
            className="search-input"
          />
        </div>
      </div>

      <div className="actions-section">
        <RoleVisibility role="owner">
          <button className="icon-btn" aria-label="Adegas">
            <Warehouse size={22} />
          </button>
        </RoleVisibility>

        <button
          className="icon-btn notification-btn"
          aria-label="Notificações"
          onClick={() => navigate("/notificacoes")}
        >
          <Bell size={22} />
          {unreadCount > 0 && <span className="notification-badge" />}
        </button>

        <button className="icon-btn" aria-label="Carrinho">
          <ShoppingCart size={22} />
        </button>

        <ProfileDropdown
          open={openMenu === "profile"}
          onToggle={() =>
            setOpenMenu(openMenu === "profile" ? null : "profile")
          }
        />
      </div>
      <button onClick={testNotification}>Testar notificação</button>
    </header>
  );
};

export default Header;
