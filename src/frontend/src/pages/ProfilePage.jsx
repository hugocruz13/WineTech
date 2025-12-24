import { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";

import Header from "../components/Header";
import Loading from "../components/Loading";
import styles from "../styles/PerfilDetalhe.module.css";

const API_URL = import.meta.env.VITE_API_URL;

const PerfilDetalhe = () => {
    const { getAccessTokenSilently, user: authUser } = useAuth0();

    const [user, setUser] = useState(null);
    const [form, setForm] = useState({ nome: "", email: "" });
    const [editMode, setEditMode] = useState(false);
    const [imageFile, setImageFile] = useState(null);
    const [imagePreview, setImagePreview] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const fetchPerfil = async () => {
            const token = await getAccessTokenSilently();
            const res = await fetch(`${API_URL}/api/Utilizador/perfil`, {
                headers: { Authorization: `Bearer ${token}` },
            });

            const json = await res.json();

            setUser(json);
            setForm({
                nome: json?.nome ?? "",
                email: json?.email ?? "",
            });

            setLoading(false);
        };

        fetchPerfil();
    }, []);

    const handleChange = (e) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleImageSelect = (e) => {
        const file = e.target.files[0];
        if (!file) return;

        setImageFile(file);
        setImagePreview(URL.createObjectURL(file));
    };

    const handleSave = async () => {
        const token = await getAccessTokenSilently();
        const formData = new FormData();

        if (form.nome !== user.nome) formData.append("Nome", form.nome);
        if (form.email !== user.email) formData.append("Email", form.email);
        if (imageFile) formData.append("Imagem", imageFile);

        if ([...formData.entries()].length === 0) {
            setEditMode(false);
            return;
        }

        await fetch(`${API_URL}/api/Utilizador/perfil`, {
            method: "PUT",
            headers: { Authorization: `Bearer ${token}` },
            body: formData,
        });

        window.location.reload();

        setUser((prev) => ({
            ...prev,
            nome: form.nome,
            email: form.email,
        }));

        setImageFile(null);
        setImagePreview(null);
        setEditMode(false);
    };

    if (loading) return <Loading />;

    const imageSrc =
        imagePreview ||
        user?.imgUrl ||
        authUser?.picture ||
        "/avatar-placeholder.png";

    return (
        <>
            <Header />

            <div className={styles.wrapper}>
                <div className={styles.card}>
                    {/* AVATAR */}
                    <div className={styles.avatarWrapper}>
                        <img src={imageSrc} alt="Avatar" />

                        {editMode && (
                            <label className={styles.uploadBtn}>
                                Carregar foto
                                <input
                                    type="file"
                                    accept="image/*"
                                    onChange={handleImageSelect}
                                />
                            </label>
                        )}

                        <h2>{user?.nome}</h2>
                        <span className={styles.userId}>ID: {user?.id}</span>
                    </div>

                    {/* CONTENT */}
                    <div className={styles.form}>
                        {!editMode ? (
                            <>
                                <div className={styles.field}>
                                    <span>ID de Utilizador</span>
                                    <strong>{user?.id}</strong>
                                </div>

                                <div className={styles.field}>
                                    <span>Nome Completo</span>
                                    <strong>{user?.nome || "—"}</strong>
                                </div>

                                <div className={styles.field}>
                                    <span>Email</span>
                                    <strong>{user?.email || "—"}</strong>
                                </div>

                                <button
                                    className={styles.edit}
                                    onClick={() => setEditMode(true)}
                                >
                                    Editar Perfil
                                </button>
                            </>
                        ) : (
                            <>
                                <label>Nome Completo</label>
                                <input
                                    name="nome"
                                    value={form.nome}
                                    onChange={handleChange}
                                />

                                <label>Email</label>
                                <input
                                    name="email"
                                    value={form.email}
                                    onChange={handleChange}
                                />

                                <div className={styles.actions}>
                                    <button
                                        className={styles.cancel}
                                        onClick={() => {
                                            setForm({
                                                nome: user.nome,
                                                email: user.email,
                                            });
                                            setImageFile(null);
                                            setImagePreview(null);
                                            setEditMode(false);
                                        }}
                                    >
                                        Cancelar
                                    </button>

                                    <button className={styles.save} onClick={handleSave}>
                                        Salvar Alterações
                                    </button>
                                </div>
                            </>
                        )}
                    </div>
                </div>
            </div>
        </>
    );
};

export default PerfilDetalhe;
