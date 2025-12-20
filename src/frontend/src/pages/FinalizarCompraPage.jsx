import { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import Loading from "../components/Loading";
import styles from "../styles/FinalizarCompraPage.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const FinalizarCompraPage = () => {
  const { getAccessTokenSilently } = useAuth0();

  const [items, setItems] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [cardNumber, setCardNumber] = useState("");
  const [mes, setMes] = useState("");
  const [ano, setAno] = useState("");
  const [comprando, setComprando] = useState(false);

  useEffect(() => {
    const fetchCompra = async () => {
      try {
        const token = await getAccessTokenSilently();

        const response = await fetch(`${API_URL}/api/carrinho/detalhes`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (!response.ok) {
          throw new Error("Erro ao carregar a compra");
        }

        const result = await response.json();
        setItems(result.data);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchCompra();
  }, [getAccessTokenSilently]);

  const handleFinalizarCompra = async () => {
    try {
      setComprando(true);
      setError(null);

      const token = await getAccessTokenSilently();

      const response = await fetch(`${API_URL}/api/Compra`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify({
          cardNumber,
          mes: Number(mes),
          ano: Number(ano),
        }),
      });

      if (!response.ok) {
        throw new Error("Erro ao finalizar a compra");
      }

      await response.json();
    } catch (err) {
      setError(err.message);
    } finally {
      setComprando(false);
    }
  };

  if (loading) return <Loading />;
  if (error) return <p>{error}</p>;

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

          {error && <p className={styles.error}>{error}</p>}

          <button
            className={styles.finalizar}
            onClick={handleFinalizarCompra}
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
