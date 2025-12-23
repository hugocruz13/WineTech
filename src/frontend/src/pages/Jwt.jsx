import React, { useEffect, useState } from "react";
import { useAuth0 } from "@auth0/auth0-react";
import styles from "../styles/Jwt.module.css";
import Loading from "../components/Loading";

const JWT = () => {
  const { user, logout, getAccessTokenSilently, isLoading } = useAuth0();
  const [token, setToken] = useState("");

  useEffect(() => {
    const obterTokenERegistrar = async () => {
      try {
        const tokenRecebido = await getAccessTokenSilently();
        setToken(tokenRecebido);
        console.log("Token obtido:", tokenRecebido);
      } catch (error) {
        console.error("Erro ao obter token ou registar utilizador:", error);
      }
    };

    obterTokenERegistrar();
  }, [getAccessTokenSilently, user]);

  if (isLoading) {
    return <Loading />;
  }

  return (
    <div className={styles.pageContainer}>
      <div className={styles.pageContent}>
        <nav className={styles.navbar}>
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
              className={styles.logoutButton}
            >
              Sair
            </button>
          </div>
        </nav>

        <h2>Dashboard</h2>
        <p>Estás autenticado com sucesso!</p>

        <div className={styles.debugBox}>
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
            className={styles.textArea}
          />
        </div>
      </div>
    </div>
  );
};

export default JWT;
