import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";
import Loading from "../components/Loading";
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

  const buildPayload = () => ({
    id: vinho.id,
    nome: form.nome || null,
    produtor: form.produtor || null,
    tipo: form.tipo || null,
    descricao: form.descricao || null,
    ano: form.ano ? Number(form.ano) : 0,
    preco: form.preco ? Number(form.preco) : 0,
  });

  const handleSave = async () => {
    const token = await getAccessTokenSilently();

    const res = await fetch(`${API_URL}/api/vinho/${vinho.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(buildPayload()),
    });

    const json = await res.json();
    setVinho(json.data);
    setForm(json.data);
    setEditMode(false);
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

    const res = await fetch(`${API_URL}/api/vinho/${vinho.id}/upload-image`, {
      method: "POST",
      headers: { Authorization: `Bearer ${token}` },
      body: formData,
    });

    const json = await res.json();

    setVinho((prev) => ({ ...prev, imageUrl: json.data }));
    setForm((prev) => ({ ...prev, imageUrl: json.data }));
    setImageFile(null);
    setImagePreview(null);
  };

  if (loading) return <Loading />;
  if (!vinho) return null;

  const imageSrc = imagePreview || vinho.imageUrl || vinho.img;

  return (
    <>
      <Header />

      <div className={styles.page}>
        <div className={styles.wrapper}>
          <button className={styles.back} onClick={() => navigate(-1)}>
            ← Voltar ao Catálogo
          </button>

          <div className={styles.container}>
            {/* IMAGEM */}
            <aside className={styles.left}>
              <div className={styles.imageBox}>
                <img src={imageSrc} alt={vinho.nome} />
              </div>

              <label className={styles.upload}>
                Alterar imagem
                <input
                  type="file"
                  accept="image/*"
                  onChange={handleImageSelect}
                />
              </label>

              {imageFile && (
                <button
                  className={styles.uploadBtn}
                  onClick={handleUploadImage}
                >
                  Guardar imagem
                </button>
              )}
            </aside>

            {/* CONTEÚDO */}
            <section className={styles.right}>
              {!editMode ? (
                <>
                  <h1>{vinho.nome}</h1>

                  <div className={styles.meta}>
                    <span>{vinho.produtor}</span>
                    <span>{vinho.tipo}</span>
                    <span>{vinho.ano}</span>
                  </div>

                  <p className={styles.descricao}>{vinho.descricao}</p>

                  <div className={styles.footer}>
                    <span className={styles.preco}>
                      € {vinho.preco.toFixed(2)}
                    </span>

                    <button
                      className={styles.edit}
                      onClick={() => setEditMode(true)}
                    >
                      Editar
                    </button>
                  </div>
                </>
              ) : (
                <>
                  <h2>Editar Detalhes</h2>

                  <div className={styles.formGrid}>
                    <input
                      name="nome"
                      value={form.nome || ""}
                      onChange={handleChange}
                      placeholder="Nome"
                    />
                    <input
                      name="produtor"
                      value={form.produtor || ""}
                      onChange={handleChange}
                      placeholder="Produtor"
                    />
                    <input
                      name="tipo"
                      value={form.tipo || ""}
                      onChange={handleChange}
                      placeholder="Tipo"
                    />
                    <input
                      name="ano"
                      type="number"
                      value={form.ano || ""}
                      onChange={handleChange}
                      placeholder="Ano"
                    />
                  </div>

                  <textarea
                    name="descricao"
                    value={form.descricao || ""}
                    onChange={handleChange}
                    placeholder="Descrição"
                  />

                  <input
                    name="preco"
                    type="number"
                    value={form.preco || ""}
                    onChange={handleChange}
                    placeholder="Preço"
                    className={styles.price}
                  />

                  <div className={styles.actions}>
                    <button
                      className={styles.cancel}
                      onClick={() => {
                        setForm(vinho);
                        setEditMode(false);
                      }}
                    >
                      Cancelar
                    </button>

                    <button className={styles.save} onClick={handleSave}>
                      Guardar Alterações
                    </button>
                  </div>
                </>
              )}
            </section>
          </div>
        </div>
      </div>
    </>
  );
};

export default VinhoDetalhe;
