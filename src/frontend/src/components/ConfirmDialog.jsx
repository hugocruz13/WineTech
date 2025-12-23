import styles from "../styles/ConfirmDialog.module.css";

const ConfirmDialog = ({
    open,
    title = "Confirmar ação",
    message,
    onConfirm,
    onCancel,
}) => {
    if (!open) return null;

    return (
        <div className={styles.overlay}>
            <div className={styles.modal}>
                <h3>{title}</h3>
                <p>{message}</p>

                <div className={styles.actions}>
                    <button className={styles.cancel} onClick={onCancel}>
                        Cancelar
                    </button>
                    <button className={styles.confirm} onClick={onConfirm}>
                        Apagar
                    </button>
                </div>
            </div>
        </div>
    );
};

export default ConfirmDialog;
