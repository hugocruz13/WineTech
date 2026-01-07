import { useAuth0 } from "@auth0/auth0-react";
import styles from "../styles/SignInPage.module.css";
import Loading from "../components/Loading";

const SignInPage = () => {
  const { loginWithRedirect, isLoading } = useAuth0();

  if (isLoading) {
    return <Loading />;
  }

  return (
    <div className={styles.container}>
      <div className={styles.shapes} aria-hidden="true">
        <span className={styles.blobOne} />
        <span className={styles.blobTwo} />
      </div>

      <div className={styles.grid}>
        <section className={styles.hero}>
          <span className={styles.badge}>VinhaTech · Adegas inteligentes</span>
          <h1 className={styles.title}>Controla a tua adega com dados em tempo real.</h1>
          <p className={styles.subtitle}>
            Recebe alertas IoT, monitoriza stock e mantém os teus vinhos na temperatura ideal.
          </p>
          <div className={styles.pills}>
            <span>Alertas instantâneos</span>
            <span>Pagamentos seguros</span>
            <span>Gestão colaborativa</span>
          </div>
        </section>

        <section className={styles.card}>
          <div className={styles.logoMark}>VT</div>
          <div className={styles.cardHeader}>
            <h2>Entrar</h2>
            <p>Autentica-te para continuar.</p>
          </div>

          <button
            onClick={() => loginWithRedirect()}
            className={styles.button}
          >
            Entrar com Auth0
          </button>

          <p className={styles.secureNote}>Sessão segura com Auth0</p>
        </section>
      </div>
    </div>
  );
};

export default SignInPage;
