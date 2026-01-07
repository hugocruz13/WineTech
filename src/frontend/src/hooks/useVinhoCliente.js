import { useEffect, useState } from "react";
import { getVinho } from "../api/vinho.service";
import { addToCart } from "../api/cart.service";

export const useVinhoCliente = (id, getAccessTokenSilently) => {
  const [wine, setWine] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [added, setAdded] = useState(false);

  useEffect(() => {
    const fetchWine = async () => {
      try {
        const token = await getAccessTokenSilently();
        const response = await getVinho(id, token);
        setWine(response.data ?? response);
      } catch (err) {
        setError(err.message);
      } finally {
        setLoading(false);
      }
    };

    fetchWine();
  }, [id, getAccessTokenSilently]);

  const handleAddToCart = async (wineData, quantidade, styles) => {
    try {
      const token = await getAccessTokenSilently();
      await addToCart(wineData.id, quantidade, token);
      setAdded(true);

      const cartIcon = document.querySelector(".cart-icon");
      cartIcon?.classList.add(styles.cartPulse);

      setTimeout(() => {
        setAdded(false);
        cartIcon?.classList.remove(styles.cartPulse);
      }, 1200);
    } catch (err) {
      console.error(err);
    }
  };

  return { wine, loading, error, added, handleAddToCart };
};
