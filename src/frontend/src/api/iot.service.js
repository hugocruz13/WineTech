import { httpClient } from "./httpClient";

export const getLeiturasStock = (stockId, token) =>
  httpClient(`/api/Leituras/${stockId}/leituras/stock`, { token });

export const checkLeiturasDisponiveis = (stockId, token) =>
  httpClient(`/api/Leituras/${stockId}/leituras/stock/existe`, { token });

export const getLeiturasAdega = (adegaId, token) =>
  httpClient(`/api/leituras/${adegaId}/leituras/adega`, { token });
