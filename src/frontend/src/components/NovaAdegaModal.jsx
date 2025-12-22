import { useState } from "react";
import { X } from "lucide-react";
import styles from "../styles/NovaAdegaModal.module.css";

const NovaAdegaModal = ({ onClose, onSuccess, token, apiUrl }) => {
  const [nome, setNome] = useState("");
  const [localizacao, setLocalizacao] = useState("");
  const [capacidade, setCapacidade] = useState("");
  const [imagem, setImagem] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("Nome", nome);
    formData.append("Localizacao", localizacao);
    formData.append("Capacidade", capacidade);
    if (imagem) formData.append("Imagem", imagem);

    try {
      setLoading(true);

      const response = await fetch(`${apiUrl}/api/adega`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: formData,
      });

      if (!response.ok) throw new Error("Erro ao criar adega");

      onSuccess();
      onClose();
    } catch (err) {
      alert(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.overlay}>
      <div className={styles.modal}>
        <div className={styles.header}>
          <h2>Nova Adega</h2>
          <button onClick={onClose}>
            <X size={18} />
          </button>
        </div>

        <form onSubmit={handleSubmit} className={styles.form}>
          <label>
            Nome
            <input
              required
              value={nome}
              onChange={(e) => setNome(e.target.value)}
            />
          </label>

          <label>
            Localização
            <input
              required
              value={localizacao}
              onChange={(e) => setLocalizacao(e.target.value)}
            />
          </label>

          <label>
            Capacidade
            <input
              type="number"
              required
              value={capacidade}
              onChange={(e) => setCapacidade(e.target.value)}
            />
          </label>

          <label>
            Imagem
            <input
              type="file"
              accept="image/*"
              onChange={(e) => setImagem(e.target.files[0])}
            />
          </label>

          <button type="submit" disabled={loading}>
            {loading ? "A criar..." : "Criar Adega"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default NovaAdegaModal;
