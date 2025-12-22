import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { Search, ShoppingCart, Wine, Warehouse, Bell } from "lucide-react";

import ProfileDropdown from "./ProfileDropdown";
import RoleVisibility from "./RoleVisibility";
import styles from "../styles/Header.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const Header = () => {
  const [openMenu, setOpenMenu] = useState(null);
  const [unreadCount, setUnreadCount] = useState(0);
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const fetchUnread = async () => {
      try {
        const token = await getAccessTokenSilently();

        const res = await fetch(`${API_URL}/api/utilizador/notificacoes`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        const json = await res.json();
        const unread = (json.data || []).filter((n) => !n.lida).length;

        setUnreadCount(unread);
      } catch (err) {
        console.error("Erro ao buscar notificações", err);
      }
    };

    fetchUnread();
  }, [getAccessTokenSilently]);

  useEffect(() => {
    const onNotification = () => {
      setUnreadCount((c) => c + 1);
    };

    window.addEventListener("notification:received", onNotification);
    return () =>
      window.removeEventListener("notification:received", onNotification);
  }, []);

  useEffect(() => {
    const onRead = () => {
      setUnreadCount((c) => Math.max(0, c - 1));
    };

    window.addEventListener("notification:read", onRead);
    return () => window.removeEventListener("notification:read", onRead);
  }, []);

  return (
    <header className={styles.headerContainer}>
      <div className={styles.logoSection} onClick={() => navigate("/")}>
        <div className={styles.logoIcon}>
          <Wine size={28} strokeWidth={2.5} />
        </div>
        <h1 className={styles.logoText}>VinhaTech</h1>
      </div>

      <div className={styles.searchSection}>
        <div className={styles.searchBar}>
          <Search className={styles.searchIcon} size={20} />
          <input
            type="text"
            placeholder="Procurar vinhos, produtores..."
            className={styles.searchInput}
          />
        </div>
      </div>

      <div className={styles.actionsSection}>
        <RoleVisibility role="owner">
          <button
            className={styles.iconBtn}
            aria-label="Adegas"
            onClick={() => navigate("/dashboard")}
          >
            <Warehouse size={22} />
          </button>
        </RoleVisibility>

        <button
          className={`${styles.iconBtn} ${styles.notificationBtn}`}
          aria-label="Notificações"
          onClick={() => navigate("/notificacoes")}
        >
          <Bell size={22} />
          {unreadCount > 0 && <span className={styles.notificationBadge} />}
        </button>

        <button
          className={styles.iconBtn}
          aria-label="Carrinho"
          onClick={() => navigate("/carrinho")}
        >
          <ShoppingCart size={22} />
        </button>

        <ProfileDropdown
          open={openMenu === "profile"}
          onToggle={() =>
            setOpenMenu(openMenu === "profile" ? null : "profile")
          }
        />
      </div>
    </header>
  );
};

export default Header;
