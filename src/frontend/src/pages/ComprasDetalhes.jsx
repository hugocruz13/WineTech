import Header from "../components/Header";
import Loading from "../components/Loading";
import { useEffect, useState } from "react";
import { CreditCard, Mail } from "lucide-react";
import { useParams } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import "../styles/CompraDetalhe.css";

const API_URL = import.meta.env.VITE_API_URL;

export default function CompraDetalhe() {
  const { id } = useParams();
  const { getAccessTokenSilently } = useAuth0();

  const [dados, setDados] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchCompra = async () => {
      try {
        const token = await getAccessTokenSilently();
        const res = await fetch(`${API_URL}/api/compra/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (!res.ok) throw new Error("Erro ao carregar compra");

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

  if (loading) return <Loading />;
  if (error) return <p className="error">{error}</p>;
  if (!dados.length) return null;

  const compra = dados[0];

  const subtotal = dados.reduce(
    (acc, item) => acc + item.preco * item.quantidade,
    0
  );

  return (
    <>
      <Header />
      <div className="page">
        <h1 className="titulo">Encomenda #{compra.idCompra}</h1>

        <div className="topo">
          <div className="card">
            <h3>DADOS DO CLIENTE</h3>

            <div className="cliente">
              <img
                src={compra.imagemUtilizador}
                alt="Avatar"
                className="avatar"
              />

              <div>
                <p className="nome">{compra.nomeUtilizador}</p>
                <p>
                  <Mail size={14} /> {compra.emailUtilizador}
                </p>
              </div>
            </div>
          </div>

          <div className="card">
            <h3>RESUMO FINANCEIRO</h3>

            <div className="resumo">
              <div>
                <p className="label">Método de Pagamento</p>
                <p className="pagamento">
                  <CreditCard size={14} /> Visa **** 4242
                </p>
              </div>

              <div className="total-box">
                <p className="label">Valor Total</p>
                <span>€ {subtotal.toFixed(2)}</span>
              </div>
            </div>
          </div>
        </div>

        <div className="card-itens">
          <h3>Itens do Pedido</h3>

          <table>
            <thead>
              <tr>
                <th>Produto</th>
                <th>Preço Unit.</th>
                <th>IoT</th>
              </tr>
            </thead>

            <tbody>
              {dados.map((item) => (
                <tr key={item.stockId}>
                  <td className="produto">
                    <img src={item.imgVinho} alt={item.nome} />
                    <div>
                      <strong>{item.nome}</strong>
                      <span>
                        {item.produtor} • {item.tipo} • {item.ano}
                      </span>
                    </div>
                  </td>

                  <td>€ {item.preco.toFixed(2)}</td>

                  <td>
                    <button
                      className="iot-btn"
                      title="Consultar temperatura da garrafa"
                    >
                      Ver IoT
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>

          <div className="totais">
            <div>
              <span>Subtotal</span>
              <span>€ {subtotal.toFixed(2)}</span>
            </div>
            <div>
              <span>Portes de Envio</span>
              <span className="gratis">Grátis</span>
            </div>
            <div className="total">
              <span>Total</span>
              <span>€ {subtotal.toFixed(2)}</span>
            </div>
          </div>
        </div>
      </div>
    </>
  );
}
