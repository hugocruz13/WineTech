import { useState } from "react";
import { X } from "lucide-react";
import styles from "../styles/NovaAdegaModal.module.css";

const NovoVinhoModal = ({ onClose, onSuccess, token, apiUrl }) => {
  const [nome, setNome] = useState("");
  const [produtor, setProdutor] = useState("");
  const [ano, setAno] = useState("");
  const [tipo, setTipo] = useState("");
  const [descricao, setDescricao] = useState("");
  const [preco, setPreco] = useState("");
  const [imagem, setImagem] = useState(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const formData = new FormData();
    formData.append("Nome", nome);
    formData.append("Produtor", produtor);
    formData.append("Ano", Number(ano));
    formData.append("Tipo", tipo);
    formData.append("Descricao", descricao);
    formData.append("Preco", Number(preco));

    if (imagem) {
      formData.append("ImagemUrl", imagem);
    }

    try {
      setLoading(true);

      const response = await fetch(`${apiUrl}/api/vinho`, {
        method: "POST",
        headers: {
          Authorization: `Bearer ${token}`,
        },
        body: formData,
      });

      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText);
      }

      onSuccess();
      onClose();
    } catch (err) {
      console.error("Erro ao criar vinho:", err.message);
      alert("Erro ao criar vinho");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className={styles.overlay}>
      <div className={styles.modal}>
        <div className={styles.header}>
          <h2>Novo Vinho</h2>
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
            Produtor
            <input
              required
              value={produtor}
              onChange={(e) => setProdutor(e.target.value)}
            />
          </label>

          <label>
            Ano
            <input
              type="number"
              required
              value={ano}
              onChange={(e) => setAno(e.target.value)}
            />
          </label>

          <label>
            Tipo
            <input
              required
              value={tipo}
              onChange={(e) => setTipo(e.target.value)}
            />
          </label>

          <label>
            Descrição
            <textarea
              value={descricao}
              onChange={(e) => setDescricao(e.target.value)}
            />
          </label>

          <label>
            Preço
            <input
              type="number"
              step="0.01"
              required
              value={preco}
              onChange={(e) => setPreco(e.target.value)}
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
            {loading ? "A criar..." : "Criar Vinho"}
          </button>
        </form>
      </div>
    </div>
  );
};

export default NovoVinhoModal;
