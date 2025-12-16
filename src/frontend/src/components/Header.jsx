import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { Search, ShoppingCart, Wine, Warehouse, Bell } from "lucide-react";
import ProfileDropdown from "./ProfileDropdown";
import RoleVisibility from "./RoleVisibility";
import "../styles/Header.css";

const Header = () => {
  const [openMenu, setOpenMenu] = useState(null);
  const navigate = useNavigate();

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
        <button className="icon-btn">
          <Bell size={22} onClick={() => navigate("/notificacoes")} />
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
    </header>
  );
};

export default Header;
