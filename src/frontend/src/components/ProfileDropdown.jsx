import { useAuth0 } from "@auth0/auth0-react";
import { User, LogOut, ShoppingBag, AlertTriangle } from "lucide-react";
import { useNavigate } from "react-router-dom";
import styles from "../styles/ProfileDropdown.module.css";
import RoleVisibility from "./RoleVisibility";
import { usePerfilInfo } from "../hooks/usePerfilInfo";

const ProfileDropdown = ({ open, onToggle }) => {
  const { logout, getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();
  const apiUser = usePerfilInfo(getAccessTokenSilently);

  const handleLogout = () => {
    logout({ logoutParams: { returnTo: window.location.origin } });
  };

  return (
    <div className={styles.profileWrapper}>
      <button
        className={styles.userAvatarBtn}
        onClick={onToggle}
        aria-label="Menu de Perfil"
      >
        {apiUser?.imgUrl ? (
          <img
            src={apiUser.imgUrl}
            alt={apiUser.nome}
            className={styles.avatarImg}
          />
        ) : (
          <User size={20} />
        )}
      </button>

      {open && (
        <div className={styles.profileDropdown}>
          <div className={styles.profileHeader}>
            <span className={styles.userName}>
              {apiUser?.nome || "Minha Conta"}
            </span>
            <span className={styles.userEmail}>{apiUser?.email}</span>
          </div>

          <ul className={styles.profileMenu}>
            <li>
              <button
                className={styles.menuItem}
                onClick={() => {
                  navigate("/perfil");
                  onToggle();
                }}
              >
                <User size={16} />
                <span>Ver Perfil</span>
              </button>
            </li>

            <li>
              <button
                className={styles.menuItem}
                onClick={() => {
                  navigate("/compras");
                  onToggle();
                }}
              >
                <ShoppingBag size={16} />
                <span>Minhas Compras</span>
              </button>
            </li>

            <li>
              <RoleVisibility role="owner">
                <button
                  className={styles.menuItem}
                  onClick={() => navigate('/alertas')}
                >
                  <AlertTriangle size={16} />
                  <span>Alertas</span>
                </button>
              </RoleVisibility>
            </li>
          </ul>

          <div className={styles.profileFooter}>
            <button className={styles.logoutBtn} onClick={handleLogout}>
              <LogOut size={16} />
              <span>Sair da app</span>
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

export default ProfileDropdown;
