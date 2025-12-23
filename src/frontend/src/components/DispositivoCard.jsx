import styles from "../styles/DispositivoCard.module.css";
import Loading from "../components/Loading";

const DispositivoCard = ({
    tipo,
    estado,
    icon,
    onToggle,
    loading
}) => {
    if (loading) {
        return <Loading />;
    }

    return (
        <div className={styles.card}>
            {/* HEADER */}
            <div className={styles.header}>
                <div className={styles.icon}>
                    {icon}
                </div>

                <label className={styles.switch}>
                    <input
                        type="checkbox"
                        checked={estado}
                        onChange={onToggle}
                    />
                    <span className={styles.slider}></span>
                </label>
            </div>

            {/* CONTENT */}
            <div className={styles.content}>
                <h4>{tipo}</h4>
            </div>

            {/* FOOTER */}
            <div className={styles.footer}>
                <span className={styles.status}>
                    <span
                        className={`${styles.dot} ${estado ? styles.active : styles.inactive
                            }`}
                    />
                    {estado ? "Ativo" : "Inativo"}
                </span>
            </div>
        </div>
    );
};

export default DispositivoCard;
