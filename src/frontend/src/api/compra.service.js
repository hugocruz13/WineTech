import { httpClient } from "./httpClient";

export const getCompras = (token) => httpClient("/api/compra", { token });

export const getCompraDetalhe = (id, token) =>
  httpClient(`/api/compra/${id}`, { token });

export const finalizarCompra = (payload, token) =>
  httpClient("/api/Compra", {
    method: "POST",
    token,
    body: JSON.stringify(payload),
  });
