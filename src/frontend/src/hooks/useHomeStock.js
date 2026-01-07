import { useEffect, useState } from "react";
import { getStockDisponivel } from "../api/adega.service";

export const useHomeStock = (getAccessTokenSilently) => {
  const [wines, setWines] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);

  useEffect(() => {
    const fetchStock = async () => {
      try {
        const token = await getAccessTokenSilently();
        const result = await getStockDisponivel(token);
        setWines(result.data ?? result);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchStock();
  }, [getAccessTokenSilently]);

  return { wines, loading, error };
};
