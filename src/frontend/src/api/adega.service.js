import { httpClient } from "./httpClient";

export const getAdegas = (token) => httpClient("/api/adega", { token });

export const createAdega = (formData, token) =>
  httpClient("/api/adega", { method: "POST", token, body: formData });

export const updateAdega = (id, payload, token) =>
  httpClient(`/api/adega/${id}`, {
    method: "PUT",
    token,
    body: JSON.stringify(payload),
  });

export const uploadAdegaImagem = (id, formData, token) =>
  httpClient(`/api/adega/${id}/upload-image`, {
    method: "POST",
    token,
    body: formData,
  });

export const getAdegaStock = (adegaId, token) =>
  httpClient(`/api/Adega/${adegaId}/stock`, { token });

export const getStockDisponivel = (token) =>
  httpClient("/api/adega/stock", { token });

export const getLeiturasAdega = (adegaId, token) =>
  httpClient(`/api/leituras/${adegaId}/leituras/adega`, { token });

export const addStock = (adegaId, vinhoId, quantidade, token) =>
  httpClient(`/api/Adega/${adegaId}/stock`, {
    method: "POST",
    token,
    body: JSON.stringify({ vinhoId, adegaId: Number(adegaId), quantidade }),
  });

export const updateStockQuantidade = (adegaId, vinhoId, quantidade, token) =>
  httpClient(`/api/Adega/${adegaId}/stock/${vinhoId}`, {
    method: "PUT",
    token,
    body: JSON.stringify({ vinhoId, adegaId: Number(adegaId), quantidade }),
  });

export const deleteStockItem = (vinhoId, token) =>
  httpClient(`/stock/${vinhoId}`, { method: "DELETE", token });

export const deleteAdegaById = (adegaId, token) =>
  httpClient(`/api/Adega/${adegaId}`, { method: "DELETE", token });

export const getSensores = (adegaId, token) =>
  httpClient(`/api/sensores/${adegaId}`, { token });

export const createSensor = (sensor, token) =>
  httpClient(`/api/Sensores`, {
    method: "POST",
    token,
    body: JSON.stringify(sensor),
  });

export const toggleDispositivo = (dispositivoId, token) =>
  httpClient(`/api/dispositivos/${dispositivoId}/toggle`, {
    method: "PUT",
    token,
  });
