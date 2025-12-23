import Header from "../components/Header";
import Loading from "../components/Loading";
import { useNavigate, useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import { CreditCard, Mail } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import styles from "../styles/CompraDetalhe.module.css";

const API_URL = import.meta.env.VITE_API_URL;

export default function CompraDetalhe() {
  const { id } = useParams();
  const { getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();

  const [dados, setDados] = useState([]);
  const [iotDisponivel, setIotDisponivel] = useState({});
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCompra = async () => {
      try {
        const token = await getAccessTokenSilently();

        const res = await fetch(`${API_URL}/api/compra/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (!res.ok) throw new Error();

        const json = await res.json();
        if (json.success) setDados(json.data || []);
      } catch {
        setError("Não foi possível carregar a encomenda.");
      } finally {
        setLoading(false);
      }
    };

    fetchCompra();
  }, [id, getAccessTokenSilently]);

  useEffect(() => {
    if (!dados.length) return;

    const verificarIot = async (stockId) => {
      const token = await getAccessTokenSilently();

      const res = await fetch(
        `${API_URL}/api/Leituras/${stockId}/leituras/stock/existe`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );

      if (!res.ok) return false;

      const json = await res.json();
      return json.success && json.data === true;
    };

    const carregarIot = async () => {
      const resultado = {};

      await Promise.all(
        dados.map(async (item) => {
          resultado[item.stockId] = await verificarIot(item.stockId);
        })
      );

      setIotDisponivel(resultado);
    };

    carregarIot();
  }, [dados, getAccessTokenSilently]);

  if (loading) return <Loading />;
  if (error) return <p className={styles.error}>{error}</p>;
  if (!dados.length) return null;

  const compra = dados[0];

  const subtotal = dados.reduce(
    (acc, item) => acc + item.preco * item.quantidade,
    0
  );

  const mostrarColunaIot = Object.values(iotDisponivel).some(Boolean);

  return (
    <>
      <Header />

      <div className={styles.page}>
        <h1 className={styles.titulo}>Encomenda #{compra.idCompra}</h1>

        {/* ---------------- TOPO ---------------- */}
        <div className={styles.topo}>
          <div className={styles.card}>
            <h3>DADOS DO CLIENTE</h3>

            <div className={styles.cliente}>
              <img
                src={compra.imagemUtilizador}
                alt="Avatar"
                className={styles.avatar}
              />

              <div>
                <p className={styles.nome}>{compra.nomeUtilizador}</p>
                <p>
                  <Mail size={14} /> {compra.emailUtilizador}
                </p>
              </div>
            </div>
          </div>

          <div className={styles.card}>
            <h3>RESUMO FINANCEIRO</h3>

            <div className={styles.resumo}>
              <div>
                <p className={styles.label}>Método de Pagamento</p>
                <p className={styles.pagamento}>
                  <CreditCard size={14} /> Visa ****{compra.cartao}
                </p>
              </div>

              <div className={styles.totalBox}>
                <p className={styles.label}>Valor Total</p>
                <span>€ {subtotal.toFixed(2)}</span>
              </div>
            </div>
          </div>
        </div>

        {/* ---------------- ITENS ---------------- */}
        <div className={styles.cardItens}>
          <h3>Itens do Pedido</h3>

          <table className={styles.table}>
            <thead className={styles.thead}>
              <tr>
                <th>Produto</th>
                <th>Preço Unit.</th>
                {mostrarColunaIot && <th>IoT</th>}
              </tr>
            </thead>

            <tbody className={styles.tbody}>
              {dados.map((item) => (
                <tr key={item.stockId}>
                  <td className={styles.produto}>
                    <img src={item.imgVinho} alt={item.nome} />
                    <div>
                      <strong>{item.nome}</strong>
                      <span>
                        {item.produtor} • {item.tipo} • {item.ano}
                      </span>
                    </div>
                  </td>

                  <td>€ {item.preco.toFixed(2)}</td>

                  {mostrarColunaIot && (
                    <td>
                      {iotDisponivel[item.stockId] && (
                        <button
                          className={styles.iotBtn}
                          onClick={() => navigate(`/iot/${item.stockId}`)}
                        >
                          Ver IoT
                        </button>
                      )}
                    </td>
                  )}
                </tr>
              ))}
            </tbody>
          </table>

          <div className={styles.totais}>
            <div>
              <span>Subtotal</span>
              <span>€ {subtotal.toFixed(2)}</span>
            </div>
            <div>
              <span>Portes de Envio</span>
              <span className={styles.gratis}>Grátis</span>
            </div>
            <div className={styles.total}>
              <span>Total</span>
              <span>€ {subtotal.toFixed(2)}</span>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
