import { Pencil, Check, X } from "lucide-react";
import { useState } from "react";
import styles from "../styles/VinhoCardStock.module.css";

const VinhoCard = ({ nome, imagemUrl, categoria, ano, quantidade, onSave }) => {
  const [editing, setEditing] = useState(false);
  const [value, setValue] = useState(quantidade);

  const lowStock = quantidade <= 10;

  const startEdit = () => {
    setValue(quantidade);
    setEditing(true);
  };

  const cancelEdit = () => {
    setValue(quantidade);
    setEditing(false);
  };

  const save = () => {
    if (value < 0) return;
    onSave(value);
    setEditing(false);
  };

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
          {lowStock ? "Stock Baixo" : "Em Stock"}
        </span>
      </div>

      {/* QUANTIDADE */}
      <div className={styles.quantity}>
        {editing ? (
          <input
            type="number"
            min="0"
            value={value}
            onChange={(e) => setValue(Number(e.target.value))}
            onKeyDown={(e) => {
              if (e.key === "Enter") save();
              if (e.key === "Escape") cancelEdit();
            }}
            autoFocus
            className={styles.qtyInput}
          />
        ) : (
          quantidade
        )}
      </div>

      {/* ACTIONS */}
      <div className={styles.cell}>
        {editing ? (
          <>
            <button className={styles.actionBtn} onClick={save} title="Guardar">
              <Check size={16} />
            </button>
            <button
              className={styles.actionBtn}
              onClick={cancelEdit}
              title="Cancelar"
            >
              <X size={16} />
            </button>
          </>
        ) : (
          <button
            className={styles.actionBtn}
            onClick={startEdit}
            title="Editar quantidade"
          >
            <Pencil size={16} />
          </button>
        )}
      </div>
    </div>
  );
};

export default VinhoCard;
