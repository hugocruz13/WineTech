import { X } from "lucide-react";
import { useAuth0 } from "@auth0/auth0-react";
import styles from "../styles/SelecionarVinhoModal.module.css";
import { useVinhosDisponiveis } from "../hooks/useVinhosDisponiveis";

const SelecionarVinhoModal = ({ adegaId, onClose, onSelect }) => {
    const { getAccessTokenSilently } = useAuth0();
    const { vinhos, loading } = useVinhosDisponiveis(
        adegaId,
        getAccessTokenSilently
    );

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
