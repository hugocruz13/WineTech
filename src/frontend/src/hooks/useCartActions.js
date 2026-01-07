import { addToCart } from "../api/cart.service";

export const useCartActions = (getAccessTokenSilently) => {
  const addItem = async (vinhosId, quantidade = 1) => {
    const token = await getAccessTokenSilently();
    await addToCart(vinhosId, quantidade, token);
  };

  return { addItem };
};
