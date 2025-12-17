import { useAuth0 } from "@auth0/auth0-react";
import "../styles/SignInPage.css";

const SignInPage = () => {
  const { loginWithRedirect } = useAuth0();

  return (
    <div className="container">
      <div className="card">
        <h1>Bem-vindo</h1>
        <p>Por favor, faz login para continuar.</p>
        <button onClick={() => loginWithRedirect()} className="button">
          Entrar com Auth0
        </button>
      </div>
    </div>
  );
};

export default SignInPage;
