import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import Loading from "../components/Loading";
import styles from "../styles/WineDetailPage.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const WineDetailPage = () => {
  const { id } = useParams();
  const [wine, setWine] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [quantidade, setQuantidade] = useState(1);
  const [added, setAdded] = useState(false);

  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const fetchWine = async () => {
      try {
        const token = await getAccessTokenSilently();
        const response = await fetch(`${API_URL}/api/vinho/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (!response.ok) throw new Error("Erro ao carregar vinho");

        const result = await response.json();
        setWine(result.data ?? result);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchWine();
  }, [id, getAccessTokenSilently]);

  const handleAddToCart = async () => {
    try {
      const token = await getAccessTokenSilently();

      const response = await fetch(`${API_URL}/api/carrinho`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          vinhosId: wine.id,
          quantidade,
        }),
      });

      if (!response.ok) return;

      setAdded(true);

      const cartIcon = document.querySelector(".cart-icon");
      cartIcon?.classList.add(styles.cartPulse);

      setTimeout(() => {
        setAdded(false);
        cartIcon?.classList.remove(styles.cartPulse);
      }, 1200);
    } catch (err) {
      console.error(err);
    }
  };

  if (loading) return <Loading />;
  if (error) return <p className={styles.error}>{error}</p>;
  if (!wine) return null;

  return (
    <>
      <Header />

      <main className={styles.wdPage}>
        <section className={styles.wdGrid}>
          <div className={styles.wdImageCard}>
            <img src={wine.img || "/placeholder-wine.png"} alt={wine.nome} />
          </div>

          <div className={styles.wdInfo}>
            <div className={styles.wdMeta}>
              <span className={styles.wdType}>{wine.tipo}</span>
              <span className={styles.wdYear}>{wine.ano}</span>
            </div>

            <h1 className={styles.wdTitle}>{wine.nome}</h1>
            <p className={styles.wdProducer}>{wine.produtor}</p>

            <div className={styles.wdPrice}>
              {wine.preco.toFixed(2)} €<span>IMPOSTOS INCLUÍDOS</span>
            </div>

            <div className={styles.wdActions}>
              <div className={styles.wdQty}>
                <button
                  onClick={() => setQuantidade((q) => (q > 1 ? q - 1 : 1))}
                >
                  −
                </button>

                <span>{quantidade}</span>

                <button onClick={() => setQuantidade((q) => q + 1)}>+</button>
              </div>

              <button
                className={`${styles.wdAddCart} ${added ? styles.added : ""}`}
                onClick={handleAddToCart}
                disabled={added}
              >
                {added ? "Adicionado ✓" : "Adicionar ao Carrinho →"}
              </button>
            </div>

            <div className={styles.wdDescription}>
              <h3>Descrição</h3>
              <p>{wine.descricao}</p>
            </div>

            <div className={styles.wdExtra}>
              <div className={styles.wdExtraCard}>
                <span>TEOR ALCOÓLICO</span>
                <strong>14% Vol.</strong>
              </div>

              <div className={styles.wdExtraCard}>
                <span>REGIÃO</span>
                <strong>Douro</strong>
              </div>
            </div>
          </div>
        </section>
      </main>
    </>
  );
};

export default WineDetailPage;
