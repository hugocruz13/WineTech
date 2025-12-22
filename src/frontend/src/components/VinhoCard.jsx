import { useNavigate } from "react-router-dom";
import styles from "../styles/VinhoCard.module.css";

const VinhoCard = ({ vinho }) => {
  const navigate = useNavigate();

  return (
    <div
      className={styles.card}
      onClick={() => navigate(`/vinhos/${vinho.id}`)}
    >
      {/* imagem SEM FUNDO */}
      <div className={styles.imageWrapper}>
        {vinho.ano && <span className={styles.year}>{vinho.ano}</span>}

        <img src={vinho.img} alt={vinho.nome} className={styles.image} />
      </div>

      {/* info */}
      <div className={styles.body}>
        <h3 className={styles.title}>{vinho.nome}</h3>
        <p className={styles.produtor}>{vinho.produtor}</p>
        <span className={styles.tipo}>{vinho.tipo}</span>

        <div className={styles.price}>€ {vinho.preco.toFixed(2)}</div>
      </div>
    </div>
  );
};

export default VinhoCard;
