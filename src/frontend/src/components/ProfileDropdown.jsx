import { useState, useEffect } from "react";
import { User, LogOut, ShoppingBag } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import styles from "../styles/ProfileDropdown.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const ProfileDropdown = ({ open, onToggle }) => {
  const [apiUser, setApiUser] = useState(null);

  const { logout, getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();

  const handleLogout = () => {
    logout({ logoutParams: { returnTo: window.location.origin } });
  };

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const token = await getAccessTokenSilently();

        const res = await fetch(`${API_URL}/api/utilizador/perfil`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (!res.ok) {
          throw new Error("Erro ao carregar perfil");
        }

        const data = await res.json();
        setApiUser(data);
      } catch (err) {
        console.error("Erro ao buscar user da API", err);
      }
    };

    fetchUser();
  }, [getAccessTokenSilently]);

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
