import { ShoppingCart, Heart } from "lucide-react";
import { useNavigate } from "react-router-dom";
import "../styles/WineCard.css";

const WineCard = ({ id, title, subtitle, price, type, imageUrl, year }) => {
  const navigate = useNavigate();

  const handleNavigate = () => {
    navigate(`/vinho/${id}`);
  };

  const handleAddToCart = (e) => {
    e.stopPropagation();
    console.log("Adicionar vinho:", id);
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
