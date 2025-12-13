import React from "react";
import { Heart, Star, ShoppingCart } from "lucide-react";
import "../styles/WineCard.css";

const WineCard = ({ title, subtitle, price, rating, type, imageUrl }) => {
  return (
    <div className="card-container">
      <div className="image-wrapper">
        <button className="favorite-btn" aria-label="Adicionar aos favoritos">
          <Heart size={18} />
        </button>

        {imageUrl && <img src={imageUrl} alt={title} className="wine-image" />}
      </div>

      <div className="card-details">
        <div className="info-row">
          <span className="tag">{type}</span>
          <div className="rating">
            <Star size={14} fill="currentColor" />
            <span>{rating}</span>
          </div>
        </div>

        <h3 className="card-title">{title}</h3>
        <p className="card-subtitle">{subtitle}</p>

        <div className="card-footer">
          <span className="price">€{price}</span>

          <button className="cart-btn" aria-label="Adicionar ao carrinho">
            <ShoppingCart size={18} />
          </button>
        </div>
      </div>
    </div>
  );
};

export default WineCard;
