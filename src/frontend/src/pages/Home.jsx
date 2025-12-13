import React, { useEffect, useState, useRef } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import "../styles/Home.css";
import Header from "../components/Header";
import WineCard from "../components/WineCard";

const registerUserInDb = async (user, token) => {
  try {
    const response = await fetch("https://localhost:7148/api/utilizador", {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    });

    if (!response.ok) {
      throw new Error("Erro ao registar o utilizador na DB");
    }

    const data = await response.json();
    console.log("User registado ou obtido:", data);
    return data;
  } catch (error) {
    console.error("Erro no registo do utilizador:", error);
  }
};

const Home = () => {
  const { user, logout, getAccessTokenSilently } = useAuth0();
  const [token, setToken] = useState("");
  const hasRegisteredUser = useRef(false);

  useEffect(() => {
    const obterTokenERegistrar = async () => {
      if (!user || hasRegisteredUser.current) return;

      hasRegisteredUser.current = true;

      try {
        const tokenRecebido = await getAccessTokenSilently();
        setToken(tokenRecebido);

        await registerUserInDb(user, tokenRecebido);
      } catch (error) {
        console.error("Erro ao obter token ou registar utilizador:", error);
      }
    };

    obterTokenERegistrar();
  }, [getAccessTokenSilently, user]);

  return (
    <>
      <div className="pageContainer">
        <div className="pageContent">
          <nav className="navbar">
            <span style={{ fontWeight: "bold", fontSize: "1.2rem" }}>
              Aplicação Segura
            </span>
            <div style={{ display: "flex", alignItems: "center", gap: "15px" }}>
              <span>
                Olá, <strong>{user?.name}</strong>
              </span>
              <button
                onClick={() =>
                  logout({ logoutParams: { returnTo: window.location.origin } })
                }
                className="logoutButton"
              >
                Sair
              </button>
            </div>
          </nav>

          <h2>Dashboard</h2>
          <p>Estás autenticado com sucesso!</p>

          <div className="debugBox">
            <h3>O teu Token de Acesso (JWT)</h3>
            <p>
              Copia o texto abaixo e cola em{" "}
              <a href="https://jwt.io" target="_blank" rel="noreferrer">
                jwt.io
              </a>{" "}
              para verificar o campo <code>aud</code> (audience).
            </p>

            <textarea
              readOnly
              value={token}
              onClick={(e) => e.target.select()}
              placeholder="A carregar token..."
              className="textArea"
            />
          </div>
        </div>
      </div>
    </>
  );
};

export default Home;
