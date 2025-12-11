import React, { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import "../styles/Home.css";

const Home = () => {
  const { user, logout, getAccessTokenSilently } = useAuth0();
  const [token, setToken] = useState("");

  useEffect(() => {
    const obterToken = async () => {
      try {
        const tokenRecebido = await getAccessTokenSilently();
        setToken(tokenRecebido);
        console.log("Token obtido:", tokenRecebido);
      } catch (error) {
        console.error("Erro ao obter token:", error);
        setToken("Erro: Não foi possível obter o token. Verifica a consola.");
      }
    };

    obterToken();
  }, [getAccessTokenSilently]);

  return (
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
  );
};

export default Home;
