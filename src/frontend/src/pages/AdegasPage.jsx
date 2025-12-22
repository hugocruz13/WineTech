import { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import AdegaCard from "../components/AdegaCard";
import VinhoCard from "../components/VinhoCard";
import Loading from "../components/Loading";
import NovaAdegaModal from "../components/NovaAdegaModal";
import styles from "../styles/AdegasPage.module.css";
import { Plus } from "lucide-react";

const API_URL = import.meta.env.VITE_API_URL;

const AdegasPage = () => {
  const { getAccessTokenSilently } = useAuth0();

  const [adegas, setAdegas] = useState([]);
  const [vinhos, setVinhos] = useState([]);

  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  const [showModal, setShowModal] = useState(false);
  const [setShowVinhoModal] = useState(false);

  const [token, setToken] = useState(null);
  const [activeTab, setActiveTab] = useState("adegas");

  const fetchAdegas = async (accessToken) => {
    const response = await fetch(`${API_URL}/api/adega`, {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    });

    if (!response.ok) {
      throw new Error("Erro ao carregar adegas");
    }

    const result = await response.json();
    setAdegas(result.data);
  };

  const fetchVinhos = async (accessToken) => {
    const response = await fetch(`${API_URL}/api/vinho`, {
      headers: {
        Authorization: `Bearer ${accessToken}`,
      },
    });

    if (!response.ok) {
      throw new Error("Erro ao carregar vinhos");
    }

    const result = await response.json();
    setVinhos(result.data);
  };

  const init = async () => {
    try {
      setLoading(true);
      const accessToken = await getAccessTokenSilently();
      setToken(accessToken);

      await Promise.all([fetchAdegas(accessToken), fetchVinhos(accessToken)]);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    init();
  }, []);

  if (loading) return <Loading />;
  if (error) return <p>{error}</p>;

  return (
    <>
      <Header />

      <div className={styles.page}>
        <div className={styles.container}>
          {/* Header */}
          <div className={styles.header}>
            <h1>Gestão de Catálogo</h1>
            <p>Gerencie as suas adegas e vinhos.</p>
          </div>

          {/* Tabs */}
          <div className={styles.tabs}>
            <button
              className={`${styles.tab} ${
                activeTab === "adegas" ? styles.active : ""
              }`}
              onClick={() => setActiveTab("adegas")}
            >
              Adegas
            </button>

            <button
              className={`${styles.tab} ${
                activeTab === "vinhos" ? styles.active : ""
              }`}
              onClick={() => setActiveTab("vinhos")}
            >
              Vinhos
            </button>
          </div>

          {/* Actions */}
          {activeTab === "adegas" && (
            <div className={styles.actions}>
              <button className={styles.new} onClick={() => setShowModal(true)}>
                <Plus size={16} />
                Nova Adega
              </button>
            </div>
          )}

          {activeTab === "vinhos" && (
            <div className={styles.actions}>
              <button
                className={styles.new}
                onClick={() => setShowVinhoModal(true)}
              >
                <Plus size={16} />
                Novo Vinho
              </button>
            </div>
          )}

          {/* Conteúdo */}
          {activeTab === "adegas" && (
            <div className={styles.grid}>
              {adegas.length === 0 ? (
                <p className={styles.empty}>Nenhuma adega registada!</p>
              ) : (
                adegas.map((adega) => (
                  <AdegaCard key={adega.id} adega={adega} />
                ))
              )}
            </div>
          )}

          {activeTab === "vinhos" && (
            <div className={styles.grid}>
              {vinhos.length === 0 ? (
                <p className={styles.empty}>Nenhum vinho registado!</p>
              ) : (
                vinhos.map((vinho) => (
                  <VinhoCard key={vinho.id} vinho={vinho} />
                ))
              )}
            </div>
          )}
        </div>
      </div>

      {showModal && (
        <NovaAdegaModal
          token={token}
          apiUrl={API_URL}
          onClose={() => setShowModal(false)}
          onSuccess={() => init()}
        />
      )}
    </>
  );
};

export default AdegasPage;
