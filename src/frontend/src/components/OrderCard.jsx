import "../styles/OrderCard.css";

export default function OrderCard({ orderNumber, date, price, onDetails }) {
  return (
    <div className="order-card">
      <div className="order-left">
        <h3 className="order-title">Encomenda {orderNumber}</h3>

        <div className="order-date">
          <span className="calendar-icon"></span>
          <span>{date}</span>
        </div>
      </div>

      <div className="order-right">
        <span className="order-price">{price}</span>

        <button className="order-button" onClick={onDetails}>
          Ver detalhes
        </button>
      </div>
    </div>
  );
}
