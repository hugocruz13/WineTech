import { httpClient } from "./httpClient";

export const getVinhos = (token) => httpClient("/api/vinho", { token });

export const getVinho = (id, token) =>
  httpClient(`/api/vinho/${id}`, { token });

export const createVinho = (formData, token) =>
  httpClient("/api/vinho", { method: "POST", token, body: formData });

export const updateVinho = (id, payload, token) =>
  httpClient(`/api/vinho/${id}`, {
    method: "PUT",
    token,
    body: JSON.stringify(payload),
  });

export const deleteVinho = (id, token) =>
  httpClient(`/api/vinho/${id}`, { method: "DELETE", token });

export const uploadVinhoImagem = (id, formData, token) =>
  httpClient(`/api/vinho/${id}/upload-image`, {
    method: "POST",
    token,
    body: formData,
  });
