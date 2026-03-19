import { httpClient } from "./httpClient";

export const getNotificacoes = (token) =>
  httpClient("/api/utilizador/notificacoes", { token });

export const marcarNotificacaoComoLida = (id, token) =>
  httpClient(`/api/utilizador/notificacoes/${id}/lida`, {
    method: "PUT",
    token,
  });
