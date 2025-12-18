import { useState, useEffect } from "react";
import { User, LogOut, ShoppingBag } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import "../styles/ProfileDropdown.css";

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
    <div className="profile-wrapper">
      <button
        className="user-avatar-btn"
        onClick={onToggle}
        aria-label="Menu de Perfil"
      >
        {apiUser?.imgUrl ? (
          <img src={apiUser.imgUrl} alt={apiUser.nome} className="avatar-img" />
        ) : (
          <User size={20} />
        )}
      </button>

      {open && (
        <div className="profile-dropdown">
          <div className="profile-header">
            <span className="user-name">{apiUser?.nome || "Minha Conta"}</span>
            <span className="user-email">{apiUser?.email}</span>
          </div>

          <ul className="profile-menu">
            <li>
              <button
                className="menu-item"
                onClick={() => {
                  navigate("/perfil");
                  onToggle();
                }}
              >
                <User size={16} className="menu-icon" />
                <span>Ver Perfil</span>
              </button>
            </li>

            <li>
              <button
                className="menu-item"
                onClick={() => {
                  navigate("/compras");
                  onToggle();
                }}
              >
                <ShoppingBag size={16} className="menu-icon" />
                <span>Minhas Compras</span>
              </button>
            </li>
          </ul>

          <div className="profile-footer">
            <button className="logout-btn" onClick={handleLogout}>
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
