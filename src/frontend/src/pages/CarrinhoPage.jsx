import { useNavigate } from "react-router-dom";
import { Trash2, Minus, Plus, ArrowLeft, ArrowRight } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import Loading from "../components/Loading";
import Header from "../components/Header";
import styles from "../styles/CarrinhoPage.module.css";
import { useCarrinho } from "../hooks/useCarrinho";

const CarrinhoPage = () => {
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();
  const { items, loading, error, remover, adicionarQuantidade, removerQuantidade } =
    useCarrinho(getAccessTokenSilently);

  if (loading) return <Loading />;
  if (error) return <p>{error}</p>;

  const subtotal = items.reduce(
    (acc, item) => acc + item.preco * item.quantidade,
    0
  );

  return (
    <div className={styles.pageContainer}>
      <Header />

      <main className={styles.carrinhoPage}>
        <div className={styles.carrinhoLeft}>
          <div className={styles.tituloCarrinho}>
            <h1>O Teu Carrinho</h1>
            <span>{items.length} itens</span>
          </div>

          {items.map((item) => (
            <div className={styles.carrinhoItem} key={item.vinhosId}>
              <img src={item.imagemUrl} alt={item.nomeVinho} />

              <div className={styles.vinhoInfo}>
                <h3>{item.nomeVinho}</h3>
                <p>
                  {item.produtor} • {item.tipo} • {item.ano}
                </p>

                <div className={styles.quantidade}>
                  <button onClick={() => removerQuantidade(item.vinhosId)}>
                    <Minus size={16} />
                  </button>

                  <span>{item.quantidade}</span>

                  <button onClick={() => adicionarQuantidade(item.vinhosId)}>
                    <Plus size={16} />
                  </button>
                </div>
              </div>

              <div className={styles.vinhoPreco}>
                <span>€{item.preco.toFixed(2)} / un</span>
                <strong>€{(item.preco * item.quantidade).toFixed(2)}</strong>
              </div>

              <button
                className={styles.remover}
                onClick={() => remover(item.vinhosId)}
              >
                <Trash2 size={18} />
              </button>
            </div>
          ))}

          <button className={styles.continuar} onClick={() => navigate("/")}>
            <ArrowLeft size={16} />
            Continuar a comprar
          </button>
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
            onClick={() => {
              if (items.length > 0) {
                navigate("/finalizar");
              }
            }}
          >
            Finalizar Compra
            <ArrowRight size={18} />
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

export default CarrinhoPage;
