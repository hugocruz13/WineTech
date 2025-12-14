import { useEffect, useRef } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import Header from "../components/Header";

const registerUserInDb = async (user, token) => {
  await fetch("https://localhost:7148/api/utilizador", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      Authorization: `Bearer ${token}`,
    },
  });
};

const Home = () => {
  const { user, getAccessTokenSilently } = useAuth0();
  const hasRegisteredUser = useRef(false);
  useEffect(() => {
    const obterTokenERegistrar = async () => {
      if (!user || hasRegisteredUser.current) return;
      hasRegisteredUser.current = true;
      try {
        const tokenRecebido = await getAccessTokenSilently();
        await registerUserInDb(user, tokenRecebido);
      } catch (error) {
        console.error("Erro ao obter token ou registar utilizador:", error);
      }
    };
    obterTokenERegistrar();
  }, [getAccessTokenSilently, user]);

  return (
    <>
      <Header></Header>
    </>
  );
};

export default Home;
