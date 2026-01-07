import { useAuth0 } from "@auth0/auth0-react";

import Header from "../components/Header";
import Loading from "../components/Loading";
import styles from "../styles/PerfilDetalhe.module.css";
import { usePerfil } from "../hooks/usePerfil";

const PerfilDetalhe = () => {
    const { getAccessTokenSilently, user: authUser } = useAuth0();

    const {
        user,
        form,
        editMode,
        loading,
        imageSrc,
        setEditMode,
        setForm,
        setImageFile,
        setImagePreview,
        handleChange,
        handleImageSelect,
        handleSave,
    } = usePerfil(getAccessTokenSilently, authUser);

    if (loading) return <Loading />;

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
