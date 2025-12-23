import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { Pencil, Trash2 } from "lucide-react";

import Header from "../components/Header";
import Loading from "../components/Loading";
import ConfirmDialog from "../components/ConfirmDialog";

import styles from "../styles/VinhoDetalhe.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const VinhoDetalhe = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const { getAccessTokenSilently } = useAuth0();

  const [vinho, setVinho] = useState(null);
  const [form, setForm] = useState({});
  const [imagePreview, setImagePreview] = useState(null);
  const [imageFile, setImageFile] = useState(null);
  const [loading, setLoading] = useState(true);
  const [editMode, setEditMode] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);

  useEffect(() => {
    const fetchVinho = async () => {
      const token = await getAccessTokenSilently();
      const res = await fetch(`${API_URL}/api/vinho/${id}`, {
        headers: { Authorization: `Bearer ${token}` },
      });

      const json = await res.json();
      setVinho(json.data);
      setForm(json.data);
      setLoading(false);
    };

    fetchVinho();
  }, [id]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm((prev) => ({ ...prev, [name]: value }));
  };

  const handleSave = async () => {
    const token = await getAccessTokenSilently();

    const payload = {
      id: vinho.id,
      nome: form.nome,
      produtor: form.produtor,
      tipo: form.tipo,
      descricao: form.descricao,
      ano: Number(form.ano),
      preco: Number(form.preco),
    };

    const res = await fetch(`${API_URL}/api/vinho/${vinho.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(payload),
    });

    const json = await res.json();
    setVinho(json.data);
    setForm(json.data);
    setEditMode(false);
  };

  const handleDelete = async () => {
    const token = await getAccessTokenSilently();

    await fetch(`${API_URL}/api/vinho/${vinho.id}`, {
      method: "DELETE",
      headers: { Authorization: `Bearer ${token}` },
    });

    navigate("/dashboard");
  };

  const handleImageSelect = (e) => {
    const file = e.target.files[0];
    if (!file) return;

    setImageFile(file);
    setImagePreview(URL.createObjectURL(file));
  };

  const handleUploadImage = async () => {
    if (!imageFile) return;

    const token = await getAccessTokenSilently();
    const formData = new FormData();
    formData.append("file", imageFile);

    const res = await fetch(
      `${API_URL}/api/vinho/${vinho.id}/upload-image`,
      {
        method: "POST",
        headers: { Authorization: `Bearer ${token}` },
        body: formData,
      }
    );

    const json = await res.json();

    setVinho((prev) => ({ ...prev, imageUrl: json.data }));
    setImageFile(null);
    setImagePreview(null);
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
        onConfirm={handleDelete}
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
                  <button className={styles.save} onClick={handleSave}>
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
