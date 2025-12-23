import { useEffect, useState } from "react";
import { X } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import styles from "../styles/SelecionarVinhoModal.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const SelecionarVinhoModal = ({ adegaId, onClose, onSelect }) => {
    const { getAccessTokenSilently } = useAuth0();

    const [vinhos, setVinhos] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchData = async () => {
            try {
                const token = await getAccessTokenSilently();

                const [vinhosRes, stockRes] = await Promise.all([
                    fetch(`${API_URL}/api/vinho`, {
                        headers: { Authorization: `Bearer ${token}` },
                    }),
                    fetch(`${API_URL}/api/Adega/${adegaId}/stock`, {
                        headers: { Authorization: `Bearer ${token}` },
                    }),
                ]);

                const vinhosJson = await vinhosRes.json();
                const stockJson = await stockRes.json();

                const stockIds = new Set(
                    (stockJson.data || []).map((s) => s.vinhoId)
                );

                const vinhosDisponiveis = (vinhosJson.data || []).filter(
                    (vinho) => !stockIds.has(vinho.id)
                );

                setVinhos(vinhosDisponiveis);
            } catch (err) {
                console.error("Erro ao carregar vinhos:", err);
            } finally {
                setLoading(false);
            }
        };

        if (adegaId) fetchData();
    }, [adegaId, getAccessTokenSilently]);

    return (
        <div className={styles.overlay}>
            <div className={styles.modal}>
                <div className={styles.header}>
                    <h2>Adicionar Vinho à Adega</h2>
                    <button onClick={onClose}>
                        <X size={18} />
                    </button>
                </div>

                {loading ? (
                    <p>A carregar...</p>
                ) : vinhos.length === 0 ? (
                    <p>Todos os vinhos já estão nesta adega.</p>
                ) : (
                    <div className={styles.list}>
                        {vinhos.map((vinho) => (
                            <div key={vinho.id} className={styles.item}>
                                <div className={styles.imageWrapper}>
                                    <img
                                        src={vinho.img}
                                        alt={vinho.nome}
                                    />
                                </div>

                                <div className={styles.info}>
                                    <strong>{vinho.nome}</strong>
                                    <span>
                                        {vinho.tipo} • {vinho.ano || "—"}
                                    </span>
                                </div>

                                <button
                                    onClick={() => {
                                        onSelect(vinho);
                                        onClose();
                                    }}
                                >
                                    Selecionar
                                </button>
                            </div>
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default SelecionarVinhoModal;
