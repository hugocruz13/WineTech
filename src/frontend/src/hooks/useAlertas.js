import { useEffect, useState } from "react";
import { getAlertas, resolverAlerta } from "../api/alertas.service";

export const useAlertas = (getAccessTokenSilently) => {
  const [alertas, setAlertas] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const load = async () => {
      try {
        const token = await getAccessTokenSilently();
        const data = await getAlertas(token);
        if (data?.success) {
          setAlertas(data.data);
        }
      } catch (error) {
      } finally {
        setLoading(false);
      }
    };

    load();
  }, [getAccessTokenSilently]);

  const marcarComoResolvido = async (id) => {
    try {
      const token = await getAccessTokenSilently();
      const result = await resolverAlerta(id, token);
      if (result?.success !== false) {
        setAlertas((prev) =>
          prev.map((alerta) =>
            alerta.id === id ? { ...alerta, resolvido: true } : alerta
          )
        );
      }
    } catch (error) {
    }
  };

  return { alertas, loading, marcarComoResolvido };
};
