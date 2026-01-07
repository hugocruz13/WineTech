import { useState } from "react";
import { useParams } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import Loading from "../components/Loading";
import styles from "../styles/WineDetailPage.module.css";
import { useVinhoCliente } from "../hooks/useVinhoCliente";

const WineDetailPage = () => {
  const { id } = useParams();
  const [quantidade, setQuantidade] = useState(1);
  const { getAccessTokenSilently } = useAuth0();
  const { wine, loading, error, added, handleAddToCart } = useVinhoCliente(
    id,
    getAccessTokenSilently
  );

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
                onClick={() => handleAddToCart(wine, quantidade, styles)}
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
