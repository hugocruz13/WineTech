import React, { useState } from "react";
import {
    AlertTriangle,
    Thermometer,
    Calendar,
    CheckCircle,
} from "lucide-react";
import styles from "../styles/AlertaCard.module.css";
import Loading from "../components/Loading";

const AlertaCard = ({ alerta, onResolvido }) => {
    const [loading, setLoading] = useState(false);

    const resolverAlerta = async () => {
        try {
            setLoading(true);
            await onResolvido(alerta.id);
        } catch (error) {
            console.error(error);
        } finally {
            setLoading(false);
        }
    };

    const isResolvido = alerta.resolvido;

    return (
        <div
            className={`${styles.alertaCard} ${isResolvido ? styles.resolvido : styles.naoResolvido
                }`}
        >
            {/* HEADER */}
            <div className={styles.header}>
                <span
                    className={`${styles.badgeCritico} ${isResolvido ? styles.badgeConcluido : ""
                        }`}
                >
                    {isResolvido ? (
                        <CheckCircle size={14} />
                    ) : (
                        <AlertTriangle size={14} />
                    )}
                    {isResolvido ? "Concluído" : "Crítico"}
                </span>

                <span
                    className={`${styles.status} ${isResolvido ? styles.statusResolvido : styles.statusNaoResolvido
                        }`}
                >
                    <CheckCircle size={14} />
                    {isResolvido ? "Resolvido" : "Não Resolvido"}
                </span>
            </div>

            {/* TITULO */}
            <h2 className={styles.titulo}>{alerta.tipoAlerta}</h2>

            {/* MENSAGEM */}
            <p className={styles.mensagem}>{alerta.mensagem}</p>

            {/* FOOTER */}
            <div className={styles.footer}>
                <div className={styles.chips}>
                    <span className={styles.chip}>
                        <Calendar size={14} />
                        {new Date(alerta.dataHora).toLocaleString("pt-BR")}
                    </span>

                    <span className={styles.chip}>
                        <Thermometer size={14} />
                        {alerta.tipoSensor} ({alerta.identificadorHardware})
                    </span>
                </div>

                {!isResolvido && (
                    <button
                        className={styles.botaoResolver}
                        onClick={resolverAlerta}
                        disabled={loading}
                    >
                        {loading ? (
                            <Loading size={16} />
                        ) : (
                            <>
                                <CheckCircle size={16} />
                                Resolver
                            </>
                        )}
                    </button>
                )}
            </div>
        </div>
    );
};

export default AlertaCard;
