import { ShoppingCart, Heart } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import "../styles/WineCard.css";

const API_URL = import.meta.env.VITE_API_URL;

const WineCard = ({ id, title, subtitle, price, type, imageUrl, year }) => {
  const navigate = useNavigate();

  const { getAccessTokenSilently } = useAuth0();

  const handleNavigate = () => {
    navigate(`/vinho/${id}`);
  };

  const handleAddToCart = async (e) => {
    e.stopPropagation();

    try {
      const token = await getAccessTokenSilently();

      const response = await fetch(`${API_URL}/api/carrinho`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          vinhosId: id,
          quantidade: 1,
        }),
      });

      if (!response.ok) {
        throw new Error("Erro ao adicionar ao carrinho");
      }

      console.log("Vinho adicionado ao carrinho");
    } catch (err) {
      console.error("Erro ao adicionar ao carrinho:", err);
    }
  };

  return (
    <article className="wine-card" onClick={handleNavigate}>
      <div className="wine-image-wrapper">
        {imageUrl && <img src={imageUrl} alt={title} className="wine-image" />}
      </div>

      <div className="wine-content">
        <div className="wine-meta">
          <span className="wine-tag">{type}</span>
          {year && <span className="wine-year">{year}</span>}
        </div>

        <h3 className="wine-title">{title}</h3>
        <p className="wine-producer">{subtitle}</p>

        <div className="wine-footer">
          <span className="wine-price">{price} €</span>

          <button className="add-cart-btn" onClick={handleAddToCart}>
            <ShoppingCart size={16} />
            <span>Adicionar</span>
          </button>
        </div>
      </div>
    </article>
  );
};

export default WineCard;
