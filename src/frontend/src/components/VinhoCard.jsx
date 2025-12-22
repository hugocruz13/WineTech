import { Pencil } from "lucide-react";
import styles from "../styles/VinhoCardStock.module.css";

const VinhoCard = ({ nome, imagemUrl, categoria, ano, quantidade, onEdit }) => {
  const lowStock = quantidade <= 10;

  return (
    <div className={styles.row}>
      <div className={styles.product}>
        <img src={imagemUrl} alt={nome} />
        <strong>{nome}</strong>
      </div>

      <div className={styles.cell}>{categoria}</div>
      <div className={styles.cell}>{ano}</div>

      <div className={styles.cell}>
        <span
          className={`${styles.badge} ${lowStock ? styles.low : styles.ok}`}
        >
          {lowStock ? "Low Stock" : "In Stock"}
        </span>
      </div>

      <div className={styles.quantity}>{quantidade}</div>

      <div className={styles.cell}>
        <button className={styles.actionBtn} onClick={onEdit}>
          <Pencil size={16} />
        </button>
      </div>
    </div>
  );
};

export default VinhoCard;
