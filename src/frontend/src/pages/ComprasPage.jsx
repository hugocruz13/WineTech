import { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import styles from "../styles/ComprasPage.module.css";

import Header from "../components/Header";
import OrderCard from "../components/OrderCard";
import Loading from "../components/Loading";

const API_URL = import.meta.env.VITE_API_URL;

export default function ComprasPage() {
  const { getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();

  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCompras = async () => {
      try {
        const token = await getAccessTokenSilently();

        const res = await fetch(`${API_URL}/api/compra`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (!res.ok) {
          throw new Error("Erro ao carregar encomendas");
        }

        const json = await res.json();

        if (json.success) {
          setOrders(json.data || []);
        } else {
          throw new Error("Resposta inválida da API");
        }
      } catch (err) {
        console.error("Erro ao buscar compras", err);
        setError("Não foi possível carregar as encomendas.");
      } finally {
        setLoading(false);
      }
    };

    fetchCompras();
  }, [getAccessTokenSilently]);

  return (
    <>
      <Header />

      <div className={styles.ordersContainer}>
        <div className={styles.ordersHeader}>
          <h2 className={styles.ordersTitle}>As Minhas Encomendas</h2>
          <p className={styles.ordersSubtitle}>
            Consulte o histórico das suas compras e reveja os detalhes de cada
            encomenda.
          </p>
        </div>

        {loading && <Loading />}
        {error && <p className={styles.error}>{error}</p>}

        {!loading && !error && orders.length === 0 && (
          <p>Não existem encomendas.</p>
        )}

        <div className={styles.ordersList}>
          {!loading &&
            !error &&
            orders.map((order) => (
              <OrderCard
                key={order.idCompra}
                orderNumber={`Encomenda #${order.idCompra}`}
                date={new Date(order.dataCompra).toLocaleDateString("pt-PT")}
                price={`€${order.valorTotal.toFixed(2)}`}
                onDetails={() => navigate(`/compra/${order.idCompra}`)}
              />
            ))}
        </div>
      </div>
    </>
  );
}
