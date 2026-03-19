import { useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import AdegaCard from "../components/AdegaCard";
import VinhoCard from "../components/VinhoCardOwner";
import Loading from "../components/Loading";
import NovaAdegaModal from "../components/NovaAdegaModal";
import NovoVinhoModal from "../components/NovoVinhoModal";
import styles from "../styles/AdegasPage.module.css";
import { Plus } from "lucide-react";
import { useCatalogo } from "../hooks/useCatalogo";

const AdegasPage = () => {
  const { getAccessTokenSilently } = useAuth0();
  const { adegas, vinhos, loading, error, token, init } = useCatalogo(getAccessTokenSilently);
  const [showModal, setShowModal] = useState(false);
  const [showVinhoModal, setShowVinhoModal] = useState(false);
  const [activeTab, setActiveTab] = useState("adegas");

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
              className={`${styles.tab} ${activeTab === "adegas" ? styles.active : ""
                }`}
              onClick={() => setActiveTab("adegas")}
            >
              Adegas
            </button>

            <button
              className={`${styles.tab} ${activeTab === "vinhos" ? styles.active : ""
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

      {/* Modal Adega */}
      {showModal && (
        <NovaAdegaModal
          token={token}
          onClose={() => setShowModal(false)}
          onSuccess={() => init()}
        />
      )}

      {/* Modal Vinho */}
      {showVinhoModal && (
        <NovoVinhoModal
          token={token}
          onClose={() => setShowVinhoModal(false)}
          onSuccess={() => init()}
        />
      )}
    </>
  );
};

export default AdegasPage;
