import { ShoppingCart } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import styles from "../styles/WineCard.module.css";
import Loading from "../components/Loading";

const API_URL = import.meta.env.VITE_API_URL;

const WineCard = ({ id, title, subtitle, price, type, imageUrl, year, loading }) => {
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();

  if (loading) {
    return <Loading />;
  }

  const handleNavigate = () => {
    navigate(`/vinho/${id}`);
  };

  const handleAddToCart = async (e) => {
    e.stopPropagation();

    try {
      const token = await getAccessTokenSilently();

      const response = await fetch(`${API_URL}/api/carrinho`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          vinhosId: id,
          quantidade: 1,
        }),
      });

      if (!response.ok) {
        throw new Error("Erro ao adicionar ao carrinho");
      }
    } catch (err) {
      console.error("Erro ao adicionar ao carrinho:", err);
    }
  };

  return (
    <article className={styles.wineCard} onClick={handleNavigate}>
      <div className={styles.wineImageWrapper}>
        {imageUrl && (
          <img src={imageUrl} alt={title} className={styles.wineImage} />
        )}
      </div>

      <div className={styles.wineContent}>
        <div className={styles.wineMeta}>
          {type && (
            <span className={`${styles.wineTag} ${styles[type.toLowerCase()]}`}>
              {type}
            </span>
          )}

          {year && <span className={styles.wineYear}>{year}</span>}
        </div>

        <h3 className={styles.wineTitle}>{title}</h3>
        <p className={styles.wineProducer}>{subtitle}</p>

        <div className={styles.wineFooter}>
          <span className={styles.winePrice}>{price} €</span>

          <button className={styles.addCartBtn} onClick={handleAddToCart}>
            <ShoppingCart size={16} />
            <span>Adicionar</span>
          </button>
        </div>
      </div>
    </article>
  );
};

export default WineCard;
