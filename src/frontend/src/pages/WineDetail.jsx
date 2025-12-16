import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import Header from "../components/Header";
import Loading from "../components/Loading";
import "../styles/WineDetail.css";

const API_URL = import.meta.env.VITE_API_URL;

const WineDetail = () => {
  const { id } = useParams();
  const [wine, setWine] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchWine = async () => {
      try {
        const response = await fetch(`${API_URL}/vinho/${id}`);
        if (!response.ok) {
          throw new Error("Erro ao carregar vinho");
        }

        const result = await response.json();
        console.log(result.data);
        setWine(result.data ?? result);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchWine();
  }, [id]);

  if (loading) return <Loading />;
  if (error) return <p className="error">{error}</p>;
  if (!wine) return null;

  return (
    <>
      <Header />
      <main className="wine-detail-container">
        <div className="wine-detail-image">
          <img src={wine.img || "/placeholder-wine.png"} alt={wine.nome} />
        </div>

        <div className="wine-detail-info">
          <div className="wine-detail-tags">
            <span className="tag">{wine.tipo}</span>
            <span className="year">{wine.ano}</span>
          </div>

          <h1 className="wine-detail-title">{wine.nome}</h1>
          <p className="wine-detail-producer">{wine.produtor}</p>

          <div className="wine-detail-price">
            <span>{wine.preco.toFixed(2)} €</span>
            <small>Impostos incluídos</small>
          </div>

          <div className="wine-detail-actions">
            <button className="qty-btn">−</button>
            <span className="qty">1</span>
            <button className="qty-btn">+</button>

            <button className="add-cart-btn">Adicionar ao Carrinho</button>
          </div>

          <div className="wine-detail-description">
            <h3>Descrição</h3>
            <p>{wine.descricao}</p>
          </div>
        </div>
      </main>
    </>
  );
};

export default WineDetail;
