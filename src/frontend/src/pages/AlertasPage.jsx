import React from "react";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import AlertaCard from "../components/AlertaCard";
import Loading from "../components/Loading";
import styles from "../styles/AlertasPage.module.css";
import { useAlertas } from "../hooks/useAlertas";

const AlertasPage = () => {
    const { getAccessTokenSilently } = useAuth0();
    const { alertas, loading, marcarComoResolvido } = useAlertas(getAccessTokenSilently);

    return (
        <div>
            <Header />

            <div className={styles.page}>
                {/* TOPO */}
                <div className={styles.pageHeader}>
                    <div>
                        <h1 className={styles.titulo}>Lista de Alertas</h1>
                        <p className={styles.subtitulo}>
                            Monitore a estabilidade e temperatura dos seus ativos.
                        </p>
                    </div>
                </div>

                {/* LISTA */}
                {loading ? (
                    <Loading />
                ) : (
                    <div className={styles.alertasList}>
                        {alertas.map((alerta) => (
                            <AlertaCard
                                key={alerta.id}
                                alerta={alerta}
                                onResolvido={marcarComoResolvido}
                            />
                        ))}
                    </div>
                )}
            </div>
        </div>
    );
};

export default AlertasPage;
