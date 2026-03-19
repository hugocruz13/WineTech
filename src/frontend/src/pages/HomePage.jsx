import Header from "../components/Header";
import WineCard from "../components/VinhoCardCliente";
import Loading from "../components/Loading";
import styles from "../styles/HomePage.module.css";
import { useAuth0 } from "@auth0/auth0-react";
import { useHomeStock } from "../hooks/useHomeStock";

const HomePage = () => {
  const { getAccessTokenSilently } = useAuth0();
  const { wines, loading, error } = useHomeStock(getAccessTokenSilently);

  return (
    <>
      <Header />

      <main className={styles.homeContainer}>
        {loading && <Loading />}
        {error && <p className={styles.error}>{error}</p>}

        <div className={styles.wineGrid}>
          {wines.length > 0
            ? wines.map((wine) => (
              <WineCard
                key={wine.vinhoId}
                id={wine.vinhoId}
                title={wine.nome}
                subtitle={wine.produtor}
                price={wine.preco.toFixed(2)}
                type={wine.tipo}
                year={wine.ano}
                imageUrl={wine.imagemUrl}
              />
            ))
            : !loading && <p>Nenhum vinho disponível</p>}
        </div>
      </main>
    </>
  );
};

export default HomePage;
