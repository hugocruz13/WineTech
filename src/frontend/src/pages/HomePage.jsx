import { useEffect, useState } from "react";
import Header from "../components/Header";
import WineCard from "../components/WineCard";
import Loading from "../components/Loading";
import styles from "../styles/HomePage.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const HomePage = () => {
  const [wines, setWines] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchStock = async () => {
      try {
        const response = await fetch(`${API_URL}/api/adega/stock`);

        if (!response.ok) {
          throw new Error("Erro ao carregar vinhos");
        }

        const result = await response.json();
        setWines(result.data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchStock();
  }, []);

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
