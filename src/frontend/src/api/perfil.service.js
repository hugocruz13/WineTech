import { httpClient } from "./httpClient";

export const getPerfil = (token) =>
  httpClient("/api/Utilizador/perfil", { token });

export const atualizarPerfil = (formData, token) =>
  httpClient("/api/Utilizador/perfil", {
    method: "PUT",
    token,
    body: formData,
  });
