import { useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { Pencil, Trash2 } from "lucide-react";

import Header from "../components/Header";
import Loading from "../components/Loading";
import ConfirmDialog from "../components/ConfirmDialog";

import styles from "../styles/VinhoDetalhe.module.css";
import { useVinhoOwner } from "../hooks/useVinhoOwner";

const VinhoDetalhe = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();
  const [editMode, setEditMode] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);

  const {
    vinho,
    form,
    imagePreview,
    imageFile,
    loading,

    handleChange,
    handleSave,
    handleDelete,
    handleImageSelect,
    handleUploadImage,
  } = useVinhoOwner(id, getAccessTokenSilently);

  const onDeleteConfirm = async () => {
    await handleDelete();
    navigate("/dashboard");
  };

  const onSave = async () => {
    await handleSave();
    setEditMode(false);
  };

  if (loading) return <Loading />;
  if (!vinho) return null;

  const imageSrc = imagePreview || vinho.imageUrl || vinho.img;

  return (
    <>
      <Header />

      <ConfirmDialog
        open={confirmOpen}
        title="Apagar vinho"
        message="Tens a certeza que queres apagar este vinho? Esta ação é irreversível."
        onCancel={() => setConfirmOpen(false)}
        onConfirm={onDeleteConfirm}
      />

      <div className={styles.page}>
        <button className={styles.back} onClick={() => navigate(-1)}>
          ← Voltar ao Catálogo
        </button>

        <div className={styles.card}>
          <aside className={styles.imageSection}>
            <img src={imageSrc} alt={vinho.nome} />

            <label className={styles.upload}>
              Alterar imagem
              <input type="file" accept="image/*" onChange={handleImageSelect} />
            </label>

            {imageFile && (
              <button className={styles.uploadBtn} onClick={handleUploadImage}>
                Guardar imagem
              </button>
            )}
          </aside>

          <section className={styles.content}>
            {!editMode ? (
              <>
                <h1>{vinho.nome}</h1>

                <div className={styles.meta}>
                  <span>{vinho.produtor}</span>
                  <span>{vinho.tipo}</span>
                  <span>{vinho.ano}</span>
                </div>

                <p className={styles.desc}>{vinho.descricao}</p>

                <div className={styles.footer}>
                  <span className={styles.price}>
                    € {vinho.preco.toFixed(2)}
                  </span>

                  <div className={styles.actions}>
                    <button
                      className={styles.edit}
                      onClick={() => setEditMode(true)}
                    >
                      <Pencil size={16} />
                      Editar
                    </button>

                    <button
                      className={styles.delete}
                      onClick={() => setConfirmOpen(true)}
                    >
                      <Trash2 size={16} />
                      Apagar Adega
                    </button>
                  </div>
                </div>
              </>
            ) : (
              <>
                <h2>Editar Detalhes</h2>

                <div className={styles.formGrid}>
                  <input name="nome" value={form.nome} onChange={handleChange} />
                  <input
                    name="produtor"
                    value={form.produtor}
                    onChange={handleChange}
                  />
                  <input name="tipo" value={form.tipo} onChange={handleChange} />
                  <input
                    name="ano"
                    type="number"
                    value={form.ano}
                    onChange={handleChange}
                  />
                </div>

                <textarea
                  name="descricao"
                  value={form.descricao || ""}
                  onChange={handleChange}
                  onInput={(e) => {
                    e.target.style.height = "auto";
                    e.target.style.height = `${e.target.scrollHeight}px`;
                  }}
                  placeholder="Descrição"
                  className={styles.textarea}
                />


                <input
                  name="preco"
                  type="number"
                  value={form.preco}
                  onChange={handleChange}
                />

                <div className={styles.actions}>
                  <button
                    className={styles.cancel}
                    onClick={() => setEditMode(false)}
                  >
                    Cancelar
                  </button>
                  <button className={styles.save} onClick={onSave}>
                    Guardar
                  </button>
                </div>
              </>
            )}
          </section>
        </div>
      </div>
    </>
  );
};

export default VinhoDetalhe;
