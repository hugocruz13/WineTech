import { Package, Calendar, ArrowRight } from "lucide-react";
import "../styles/OrderCard.css";

export default function OrderCard({ orderNumber, date, price, onDetails }) {
  return (
    <div className="order-card">
      <div className="order-left">
        <div className="order-icon">
          <Package size={18} />
        </div>

        <div className="order-info">
          <h3 className="order-title">{orderNumber}</h3>

          <div className="order-date">
            <Calendar size={14} />
            <span>{date}</span>
          </div>
        </div>
      </div>

      <div className="order-right">
        <div className="order-total">
          <span className="order-total-label">Total</span>
          <span className="order-price">{price}</span>
        </div>

        <button className="order-button" onClick={onDetails}>
          Ver detalhes
          <ArrowRight size={16} />
        </button>
      </div>
    </div>
  );
}
