import { useState } from "react";
import { X, Cpu, Thermometer, Droplets, Sun } from "lucide-react";
import styles from "../styles/AdicionarSensorModal.module.css";

const AdicionarSensorModal = ({ adegaId, onClose, onAdd }) => {
    const [origem, setOrigem] = useState("GERADO");
    const [tipo, setTipo] = useState("Temperatura");

    const handleSubmit = () => {
        const identificadorHardware =
            origem === "REAL" ? `REAL_ADEGA_${adegaId}` : "GERADO";

        onAdd({
            identificadorHardware,
            tipo,
            estado: true,
            adegaId: Number(adegaId),
        });

        onClose();
    };

    return (
        <div className={styles.overlay}>
            <div className={styles.modal}>
                <div className={styles.header}>
                    <h2>Adicionar Sensor</h2>
                    <button onClick={onClose}>
                        <X size={18} />
                    </button>
                </div>

                <div className={styles.form}>
                    {/* ORIGEM */}
                    <div className={styles.field}>
                        <label>Origem do Sensor</label>
                        <div className={styles.selectWrapper}>
                            <Cpu size={16} />
                            <select
                                value={origem}
                                onChange={(e) => setOrigem(e.target.value)}
                            >
                                <option value="REAL">Real (hardware físico)</option>
                                <option value="GERADO">Gerado (simulação)</option>

                            </select>
                        </div>
                    </div>

                    {/* TIPO */}
                    <div className={styles.field}>
                        <label>Tipo de Sensor</label>
                        <div className={styles.selectWrapper}>
                            {tipo === "Temperatura" && <Thermometer size={16} />}
                            {tipo === "Humidade" && <Droplets size={16} />}
                            {tipo === "Luminosidade" && <Sun size={16} />}

                            <select value={tipo} onChange={(e) => setTipo(e.target.value)}>
                                <option>Temperatura</option>
                                <option>Humidade</option>
                                <option>Luminosidade</option>
                            </select>
                        </div>
                    </div>

                    {/* ACTIONS */}
                    <div className={styles.actions}>
                        <button className={styles.cancel} onClick={onClose}>
                            Cancelar
                        </button>
                        <button className={styles.submit} onClick={handleSubmit}>
                            Adicionar
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default AdicionarSensorModal;
