import { httpClient } from "./httpClient";

export const getCarrinhoDetalhes = (token) =>
  httpClient("/api/carrinho/detalhes", { token });

export const addToCart = (vinhosId, quantidade, token) =>
  httpClient("/api/carrinho", {
    method: "POST",
    token,
    body: JSON.stringify({ vinhosId, quantidade }),
  });

export const removerDoCarrinho = (vinhoId, token) =>
  httpClient(`/api/carrinho?vinhoId=${vinhoId}`, {
    method: "DELETE",
    token,
  });

export const incrementarQuantidade = (vinhosId, quantidade, token) =>
  httpClient("/api/carrinho/add", {
    method: "PUT",
    token,
    body: JSON.stringify({ vinhosId, quantidade }),
  });

export const decrementarQuantidade = (vinhosId, quantidade, token) =>
  httpClient("/api/carrinho/dec", {
    method: "PUT",
    token,
    body: JSON.stringify({ vinhosId, quantidade }),
  });
