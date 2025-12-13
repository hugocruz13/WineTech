import { Search, Bell, ShoppingCart, User, Wine } from "lucide-react";
import "../styles/Header.css";

const Header = () => {
  return (
    <header className="header-container">
      <div className="logo-section">
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
        <button className="icon-btn" aria-label="Notificações">
          <Bell size={22} />
          <span className="notification-badge"></span>
        </button>
        <button className="icon-btn" aria-label="Carrinho">
          <ShoppingCart size={22} />
        </button>
        <button className="user-avatar" aria-label="Perfil">
          <User size={20} />
        </button>
      </div>
    </header>
  );
};

export default Header;
