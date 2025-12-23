import React, { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import AlertaCard from "../components/AlertaCard";
import Loading from "../components/Loading";
import styles from "../styles/AlertasPage.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const AlertasPage = () => {
    const [alertas, setAlertas] = useState([]);
    const [loading, setLoading] = useState(true);

    const { getAccessTokenSilently } = useAuth0();

    useEffect(() => {
        const fetchAlertas = async () => {
            try {
                const token = await getAccessTokenSilently();

                const response = await fetch(`${API_URL}/api/Alertas/todos`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                const data = await response.json();

                if (data.success) {
                    setAlertas(data.data);
                }
            } catch (error) {
                console.error("Erro ao buscar alertas:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchAlertas();
    }, [getAccessTokenSilently]);

    const marcarComoResolvido = (id) => {
        setAlertas((prev) =>
            prev.map((alerta) =>
                alerta.id === id ? { ...alerta, resolvido: true } : alerta
            )
        );
    };

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
