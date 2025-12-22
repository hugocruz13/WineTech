import { useEffect, useState } from "react";
import Header from "../components/Header";
import WineCard from "../components/VinhoCardCliente";
import Loading from "../components/Loading";
import styles from "../styles/HomePage.module.css";
import { useAuth0 } from "@auth0/auth0-react";

const API_URL = import.meta.env.VITE_API_URL;

const HomePage = () => {
  const [wines, setWines] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const fetchStock = async () => {
      try {
        const token = await getAccessTokenSilently();

        const response = await fetch(`${API_URL}/api/adega/stock`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

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
  }, [getAccessTokenSilently]);

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
