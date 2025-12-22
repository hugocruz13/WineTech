import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { MapPin, Box, Pencil } from "lucide-react";
import EditarAdegaModal from "./EditarAdegaModal";
import styles from "../styles/AdegaCard.module.css";

const AdegaCard = ({ adega }) => {
  const navigate = useNavigate();
  const [editOpen, setEditOpen] = useState(false);

  return (
    <>
      <div className={styles.card}>
        <div className={styles.top}>
          {adega.imageUrl ? (
            <img
              src={adega.imageUrl}
              alt={adega.nome}
              className={styles.image}
            />
          ) : (
            <div className={styles.placeholder} />
          )}
        </div>

        <div className={styles.body}>
          <h3 className={styles.title}>{adega.nome}</h3>

          <div className={styles.info}>
            <MapPin size={16} />
            <span>{adega.localizacao || "-"}</span>
          </div>

          <div className={styles.info}>
            <Box size={16} />
            <span>Capacidade: {adega.capacidade || "-"}</span>
          </div>

          <div className={styles.footer}>
            <button
              className={styles.btnSecondary}
              onClick={() => navigate(`/adegas/${adega.id}`)}
            >
              Gerir
            </button>

            <button
              className={styles.iconBtn}
              onClick={() => setEditOpen(true)}
            >
              <Pencil size={16} />
            </button>
          </div>
        </div>
      </div>

      {editOpen && (
        <EditarAdegaModal adega={adega} onClose={() => setEditOpen(false)} />
      )}
    </>
  );
};

export default AdegaCard;
