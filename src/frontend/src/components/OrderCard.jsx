import { Package, Calendar, ArrowRight } from "lucide-react";
import styles from "../styles/OrderCard.module.css";
import Loading from "../components/Loading";

export default function OrderCard({ orderNumber, date, price, onDetails, loading }) {
  if (loading) {
    return <Loading />;
  }

  return (
    <div className={styles.orderCard}>
      <div className={styles.orderLeft}>
        <div className={styles.orderIcon}>
          <Package size={18} />
        </div>

        <div className={styles.orderInfo}>
          <h3 className={styles.orderTitle}>{orderNumber}</h3>

          <div className={styles.orderDate}>
            <Calendar size={14} />
            <span>{date}</span>
          </div>
        </div>
      </div>

      <div className={styles.orderRight}>
        <div className={styles.orderTotal}>
          <span className={styles.orderTotalLabel}>Total</span>
          <span className={styles.orderPrice}>{price}</span>
        </div>

        <button className={styles.orderButton} onClick={onDetails}>
          Ver detalhes
          <ArrowRight size={16} />
        </button>
      </div>
    </div>
  );
}
