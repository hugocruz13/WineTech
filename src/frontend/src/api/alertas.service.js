import { httpClient } from "./httpClient";

export const getAlertas = (token) =>
  httpClient("/api/Alertas/todos", { token });

export const resolverAlerta = (alertaId, token) =>
  httpClient(`/api/Alertas/${alertaId}/resolver`, { method: "PUT", token });
