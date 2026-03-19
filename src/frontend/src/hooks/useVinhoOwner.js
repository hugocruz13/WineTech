import { useEffect, useState } from "react";
import {
  getVinho,
  updateVinho,
  deleteVinho,
  uploadVinhoImagem,
} from "../api/vinho.service";

export const useVinhoOwner = (id, getAccessTokenSilently) => {
  const [vinho, setVinho] = useState(null);
  const [form, setForm] = useState({});
  const [imagePreview, setImagePreview] = useState(null);
  const [imageFile, setImageFile] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchVinho = async () => {
      const token = await getAccessTokenSilently();
      const res = await getVinho(id, token);
      setVinho(res.data);
      setForm(res.data);
      setLoading(false);
    };

    fetchVinho();
  }, [id, getAccessTokenSilently]);

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

    const res = await updateVinho(vinho.id, payload, token);
    setVinho(res.data);
    setForm(res.data);
  };

  const handleDelete = async () => {
    const token = await getAccessTokenSilently();
    await deleteVinho(vinho.id, token);
  };

  const handleImageSelect = (e) => {
    const file = e.target.files?.[0];
    if (!file) return;
    setImageFile(file);
    setImagePreview(URL.createObjectURL(file));
  };

  const handleUploadImage = async () => {
    if (!imageFile) return;
    const token = await getAccessTokenSilently();
    const formData = new FormData();
    formData.append("file", imageFile);

    const res = await uploadVinhoImagem(vinho.id, formData, token);
    setVinho((prev) => ({ ...prev, imageUrl: res.data }));
    setImageFile(null);
    setImagePreview(null);
  };

  return {
    vinho,
    form,
    imagePreview,
    imageFile,
    loading,
    setForm,
    setImageFile,
    setImagePreview,
    handleChange,
    handleSave,
    handleDelete,
    handleImageSelect,
    handleUploadImage,
  };
};
