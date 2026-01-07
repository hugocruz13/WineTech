import { useEffect, useState } from "react";
import { getVinhos } from "../api/vinho.service";
import { getAdegaStock } from "../api/adega.service";

export const useVinhosDisponiveis = (adegaId, getAccessTokenSilently) => {
  const [vinhos, setVinhos] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const token = await getAccessTokenSilently();
        const [vinhosRes, stockRes] = await Promise.all([
          getVinhos(token),
          getAdegaStock(adegaId, token),
        ]);

        const vinhosData = vinhosRes.data || vinhosRes;
        const stockJson = stockRes.data || stockRes;

        const stockIds = new Set((stockJson || []).map((s) => s.vinhoId));
        const vinhosDisponiveis = (vinhosData || []).filter(
          (vinho) => !stockIds.has(vinho.id)
        );

        setVinhos(vinhosDisponiveis);
      } catch (err) {
        console.error("Erro ao carregar vinhos:", err);
      } finally {
        setLoading(false);
      }
    };

    if (adegaId) fetchData();
  }, [adegaId, getAccessTokenSilently]);

  return { vinhos, loading };
};
