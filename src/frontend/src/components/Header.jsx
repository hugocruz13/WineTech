import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { ShoppingCart, Wine, Warehouse, Bell } from "lucide-react";

import ProfileDropdown from "./ProfileDropdown";
import RoleVisibility from "./RoleVisibility";
import Loading from "../components/Loading";
import styles from "../styles/Header.module.css";
import { useUnreadNotifications } from "../hooks/useUnreadNotifications";

const Header = ({ loading }) => {
  const [openMenu, setOpenMenu] = useState(null);
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();
  const unreadCount = useUnreadNotifications(getAccessTokenSilently);

  if (loading) {
    return <Loading />;
  }

  return (
    <header className={styles.headerContainer}>
      <div className={styles.logoSection} onClick={() => navigate("/")}>
        <div className={styles.logoIcon}>
          <Wine size={28} strokeWidth={2.5} />
        </div>
        <h1 className={styles.logoText}>VinhaTech</h1>
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
