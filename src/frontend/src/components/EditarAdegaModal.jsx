import { useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import styles from "../styles/EditarAdegaModal.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const EditarAdegaModal = ({ adega, onClose }) => {
  const { getAccessTokenSilently } = useAuth0();

  const [form, setForm] = useState({
    nome: adega.nome || "",
    localizacao: adega.localizacao || "",
    capacidade: adega.capacidade || "",
  });

  const [imageFile, setImageFile] = useState(null);
  const [uploading, setUploading] = useState(false);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleImageChange = (e) => {
    if (e.target.files && e.target.files[0]) {
      setImageFile(e.target.files[0]);
    }
  };

  /* =========================
     PAYLOAD ADEGA
  ========================= */
  const buildPayload = () => ({
    id: adega.id,
    nome: form.nome || null,
    localizacao: form.localizacao || null,
    capacidade: form.capacidade ? Number(form.capacidade) : 0,
  });

  /* =========================
     SAVE
  ========================= */
  const handleSave = async () => {
    try {
      const token = await getAccessTokenSilently();

      // 1️⃣ Atualizar dados da adega
      await fetch(`${API_URL}/api/adega/${adega.id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
          Authorization: `Bearer ${token}`,
        },
        body: JSON.stringify(buildPayload()),
      });

      // 2️⃣ Upload da imagem (se existir)
      if (imageFile) {
        setUploading(true);

        const formData = new FormData();
        formData.append("file", imageFile); // KEY correta

        await fetch(`${API_URL}/api/adega/${adega.id}/upload-image`, {
          method: "POST",
          headers: {
            Authorization: `Bearer ${token}`,
          },
          body: formData,
        });
      }

      onClose();
      window.location.reload();
    } catch (err) {
      console.error("Erro ao atualizar adega:", err);
      alert("Erro ao atualizar adega");
    } finally {
      setUploading(false);
    }
  };

  return (
    <div className={styles.overlay}>
      <div className={styles.modal}>
        <h2>Editar Adega</h2>

        <input
          name="nome"
          value={form.nome}
          onChange={handleChange}
          placeholder="Nome"
        />

        <input
          name="localizacao"
          value={form.localizacao}
          onChange={handleChange}
          placeholder="Localização"
        />

        <input
          name="capacidade"
          type="number"
          value={form.capacidade}
          onChange={handleChange}
          placeholder="Capacidade"
        />

        {/* Upload imagem bonito */}
        <div className={styles.upload}>
          <label className={styles.uploadLabel}>Imagem da Adega</label>

          <div className={styles.uploadBox}>
            <label htmlFor="upload-image" className={styles.uploadBtn}>
              Selecionar imagem
            </label>

            <span className={styles.fileName}>
              {imageFile ? imageFile.name : "Nenhuma imagem selecionada"}
            </span>
          </div>

          <input
            id="upload-image"
            type="file"
            accept="image/*"
            onChange={handleImageChange}
            className={styles.hiddenInput}
          />

          {/* Preview */}
          {imageFile && (
            <img
              src={URL.createObjectURL(imageFile)}
              alt="Preview"
              className={styles.preview}
            />
          )}
        </div>

        <div className={styles.actions}>
          <button onClick={onClose} className={styles.cancel}>
            Cancelar
          </button>

          <button
            onClick={handleSave}
            className={styles.save}
            disabled={uploading}
          >
            {uploading ? "A guardar..." : "Guardar"}
          </button>
        </div>
      </div>
    </div>
  );
};

export default EditarAdegaModal;
