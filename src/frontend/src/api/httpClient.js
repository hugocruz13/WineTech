const API_URL = import.meta.env.VITE_API_URL;

const buildHeaders = (token, customHeaders, body) => {
  const headers = { ...customHeaders };

  if (token) {
    headers.Authorization = `Bearer ${token}`;
  }

  const isFormData = body instanceof FormData;
  if (!isFormData && body !== undefined && !headers["Content-Type"]) {
    headers["Content-Type"] = "application/json";
  }

  return headers;
};

export const httpClient = async (path, options = {}) => {
  const { method = "GET", headers = {}, token, body } = options;
  const response = await fetch(`${API_URL}${path}`, {
    method,
    headers: buildHeaders(token, headers, body),
    body,
  });

  if (!response.ok) {
    const errorText = await response.text().catch(() => "");
    const error = new Error(errorText || "Erro na chamada à API");
    error.status = response.status;
    throw error;
  }

  const contentType = response.headers.get("content-type") || "";
  if (contentType.includes("application/json")) {
    return response.json();
  }

  return response.text();
};
