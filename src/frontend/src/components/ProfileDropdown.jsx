import { useState } from "react";
import { User, LogOut, ShoppingBag } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import "../styles/ProfileDropdown.css";

const ProfileDropdown = () => {
  const [open, setOpen] = useState(false);
  const { logout, user } = useAuth0();

  const handleLogout = () => {
    logout({ logoutParams: { returnTo: window.location.origin } });
  };

  return (
    <div className="profile-wrapper">
      <button
        className="user-avatar-btn"
        onClick={() => setOpen(!open)}
        aria-label="Menu de Perfil"
      >
        {user?.picture ? (
          <img src={user.picture} alt={user.name} className="avatar-img" />
        ) : (
          <User size={20} />
        )}
      </button>

      {open && (
        <div className="profile-dropdown">
          <div className="profile-header">
            <span className="user-name">{user?.name || "Minha Conta"}</span>
            <span className="user-email">{user?.email}</span>
          </div>

          <ul className="profile-menu">
            <li>
              <button className="menu-item">
                <User size={16} className="menu-icon" />
                <span>Ver Perfil</span>
              </button>
            </li>
            <li>
              <button className="menu-item">
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
