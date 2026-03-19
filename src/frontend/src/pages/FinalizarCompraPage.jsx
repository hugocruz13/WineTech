import { useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import Loading from "../components/Loading";
import styles from "../styles/FinalizarCompraPage.module.css";
import { useCheckout } from "../hooks/useCheckout";

const FinalizarCompraPage = () => {
  const { getAccessTokenSilently } = useAuth0();
  const { items, loading, comprando, handleFinalizarCompra } =
    useCheckout(getAccessTokenSilently);
  const [cardNumber, setCardNumber] = useState("");
  const [mes, setMes] = useState("");
  const [ano, setAno] = useState("");
  const finalizarCompra = () =>
    handleFinalizarCompra({ cardNumber, mes, ano });

  if (loading) return <Loading />;

  const subtotal = items.reduce(
    (acc, item) => acc + item.preco * item.quantidade,
    0
  );

  return (
    <div className={styles.pageContainer}>
      <Header />

      <main className={styles.finalizarPage}>
        <div className={styles.left}>
          <h1>Pagamento</h1>

          <div className={styles.card}>
            <h2>Método de Pagamento</h2>

            <div className={styles.metodo}>Cartão de Crédito</div>

            <div className={styles.form}>
              <label>
                Nome no Cartão
                <input placeholder="Como aparece no cartão" />
              </label>

              <label>
                Número do Cartão
                <input
                  placeholder="0000 0000 0000 0000"
                  value={cardNumber}
                  onChange={(e) => setCardNumber(e.target.value)}
                />
              </label>

              <div className={styles.row}>
                <label>
                  Validade (MM)
                  <input
                    placeholder="MM"
                    value={mes}
                    onChange={(e) => setMes(e.target.value)}
                  />
                </label>

                <label>
                  Validade (AA)
                  <input
                    placeholder="AA"
                    value={ano}
                    onChange={(e) => setAno(e.target.value)}
                  />
                </label>
              </div>
            </div>
          </div>
        </div>

        <div className={styles.carrinhoRight}>
          <h2>Resumo do Pedido</h2>

          <div className={styles.linha}>
            <span>Subtotal</span>
            <span>€{subtotal.toFixed(2)}</span>
          </div>

          <div className={styles.linha}>
            <span>Impostos (IVA incluído)</span>
            <span>-</span>
          </div>

          <hr />

          <div className={styles.total}>
            <span>Total</span>
            <span>€{subtotal.toFixed(2)}</span>
          </div>

          <button
            className={styles.finalizar}
            onClick={finalizarCompra}
            disabled={comprando}
          >
            {comprando ? "Processando..." : "Comprar"}
          </button>

          <p className={styles.seguro}>PAGAMENTO 100% SEGURO</p>

          <div className={styles.pagamentos}>
            <span>VISA</span>
            <span>Master Card</span>
          </div>
        </div>
      </main>
    </div>
  );
};

export default FinalizarCompraPage;
