import { useEffect, useState } from "react";
import { getPerfil, atualizarPerfil } from "../api/perfil.service";

export const usePerfil = (getAccessTokenSilently, authUser) => {
  const [user, setUser] = useState(null);
  const [form, setForm] = useState({ nome: "", email: "" });
  const [editMode, setEditMode] = useState(false);
  const [imageFile, setImageFile] = useState(null);
  const [imagePreview, setImagePreview] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchPerfil = async () => {
      const token = await getAccessTokenSilently();
      const res = await getPerfil(token);
      setUser(res);
      setForm({ nome: res?.nome ?? "", email: res?.email ?? "" });
      setLoading(false);
    };

    fetchPerfil();
  }, [getAccessTokenSilently]);

  const handleChange = (e) => {
    setForm({ ...form, [e.target.name]: e.target.value });
  };

  const handleImageSelect = (e) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setImageFile(file);
    setImagePreview(URL.createObjectURL(file));
  };

  const handleSave = async () => {
    const token = await getAccessTokenSilently();
    const formData = new FormData();

    if (form.nome !== user?.nome) formData.append("Nome", form.nome);
    if (form.email !== user?.email) formData.append("Email", form.email);
    if (imageFile) formData.append("Imagem", imageFile);

    if ([...formData.entries()].length === 0) {
      setEditMode(false);
      return;
    }

    await atualizarPerfil(formData, token);
    window.location.reload();

    setUser((prev) => ({ ...prev, nome: form.nome, email: form.email }));
    setImageFile(null);
    setImagePreview(null);
    setEditMode(false);
  };

  const imageSrc =
    imagePreview ||
    user?.imgUrl ||
    authUser?.picture ||
    "/avatar-placeholder.png";

  return {
    user,
    form,
    editMode,
    imageFile,
    imagePreview,
    loading,
    imageSrc,
    setEditMode,
    setForm,
    setImageFile,
    setImagePreview,
    handleChange,
    handleImageSelect,
    handleSave,
  };
};
