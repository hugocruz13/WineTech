import { useAuth0 } from "@auth0/auth0-react";
import { useNavigate } from "react-router-dom";
import styles from "../styles/ComprasPage.module.css";

import Header from "../components/Header";
import OrderCard from "../components/OrderCard";
import Loading from "../components/Loading";
import { useCompras } from "../hooks/useCompras";

export default function ComprasPage() {
  const { getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();
  const { orders, loading, error } = useCompras(getAccessTokenSilently);

  return (
    <>
      <Header />

      <div className={styles.ordersContainer}>
        <div className={styles.ordersHeader}>
          <h2 className={styles.ordersTitle}>As Minhas Compras</h2>
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
