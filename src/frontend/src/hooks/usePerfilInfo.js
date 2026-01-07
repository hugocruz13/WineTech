import { useEffect, useState } from "react";
import { getPerfil } from "../api/perfil.service";

export const usePerfilInfo = (getAccessTokenSilently) => {
  const [apiUser, setApiUser] = useState(null);

  useEffect(() => {
    const fetchUser = async () => {
      try {
        const token = await getAccessTokenSilently();
        const data = await getPerfil(token);
        setApiUser(data);
      } catch (err) {
        console.error("Erro ao buscar user da API", err);
      }
    };

    fetchUser();
  }, [getAccessTokenSilently]);

  return apiUser;
};
