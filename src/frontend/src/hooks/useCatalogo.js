import { useEffect, useState, useCallback } from "react";
import { getAdegas } from "../api/adega.service";
import { getVinhos } from "../api/vinho.service";

export const useCatalogo = (getAccessTokenSilently) => {
  const [adegas, setAdegas] = useState([]);
  const [vinhos, setVinhos] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [token, setToken] = useState(null);

  const init = useCallback(async () => {
    try {
      setLoading(true);
      const accessToken = await getAccessTokenSilently();
      setToken(accessToken);

      const [adegasResp, vinhosResp] = await Promise.all([
        getAdegas(accessToken),
        getVinhos(accessToken),
      ]);

      setAdegas(adegasResp.data ?? adegasResp?.data ?? adegasResp);
      setVinhos(vinhosResp.data ?? vinhosResp?.data ?? vinhosResp);
    } catch (err) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  }, [getAccessTokenSilently]);

  useEffect(() => {
    init();
  }, [init]);

  return { adegas, vinhos, loading, error, token, init };
};
