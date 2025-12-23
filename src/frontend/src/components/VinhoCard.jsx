import { Pencil, Check, X, Trash2 } from "lucide-react";
import { useState } from "react";
import styles from "../styles/VinhoCardStock.module.css";
import ConfirmDialog from "./ConfirmDialog";

const VinhoCard = ({
  nome,
  imagemUrl,
  categoria,
  ano,
  quantidade,
  onSave,
  onDelete,
}) => {
  const [editing, setEditing] = useState(false);
  const [value, setValue] = useState(quantidade);
  const [confirmOpen, setConfirmOpen] = useState(false);

  const lowStock = quantidade <= 10;

  const save = () => {
    if (value < 0) return;
    onSave(value);
    setEditing(false);
  };

  return (
    <>
      <div className={styles.row}>
        {/* PRODUTO */}
        <div className={styles.product}>
          <img src={imagemUrl} alt={nome} />
          <strong>{nome}</strong>
        </div>

        <div className={styles.cell}>{categoria}</div>
        <div className={styles.cell}>{ano}</div>

        {/* STATUS */}
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
                if (e.key === "Escape") setEditing(false);
              }}
              className={styles.qtyInput}
              autoFocus
            />
          ) : (
            quantidade
          )}
        </div>

        {/* AÇÕES */}
        <div className={styles.cell}>
          {!editing && (
            <button
              className={`${styles.actionBtn} ${styles.danger}`}
              onClick={() => setConfirmOpen(true)}
              title="Apagar stock"
            >
              <Trash2 size={16} />
            </button>
          )}

          {editing ? (
            <>
              <button
                className={styles.actionBtn}
                onClick={save}
                title="Guardar"
              >
                <Check size={16} />
              </button>
              <button
                className={styles.actionBtn}
                onClick={() => setEditing(false)}
                title="Cancelar"
              >
                <X size={16} />
              </button>
            </>
          ) : (
            <button
              className={styles.actionBtn}
              onClick={() => setEditing(true)}
              title="Editar quantidade"
            >
              <Pencil size={16} />
            </button>
          )}
        </div>
      </div>

      {/* MODAL CONFIRMAÇÃO */}
      <ConfirmDialog
        open={confirmOpen}
        title="Apagar stock"
        message="Tem a certeza que quer apagar todo o stock deste vinho? Esta ação não pode ser desfeita."
        onCancel={() => setConfirmOpen(false)}
        onConfirm={() => {
          onDelete();
          setConfirmOpen(false);
        }}
      />
    </>
  );
};

export default VinhoCard;
