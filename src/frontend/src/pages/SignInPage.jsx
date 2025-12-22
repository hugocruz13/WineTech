import { useAuth0 } from "@auth0/auth0-react";
import styles from "../styles/SignInPage.module.css";

const SignInPage = () => {
  const { loginWithRedirect } = useAuth0();

  return (
    <div className={styles.container}>
      <div className={styles.card}>
        <h1 className={styles.title}>Bem-vindo</h1>
        <p className={styles.subtitle}>Por favor, faz login para continuar.</p>
        <button onClick={() => loginWithRedirect()} className={styles.button}>
          Entrar com Auth0
        </button>
      </div>
    </div>
  );
};

export default SignInPage;
