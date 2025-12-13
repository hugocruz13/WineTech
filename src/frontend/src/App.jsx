import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import "./App.css";
import RoleGuard from "./components/RoleGuard";
import SignIn from "./pages/SignIn";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import JWT from "./pages/Jwt";

function App() {
  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return (
      <div
        style={{
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          height: "100vh",
          flexDirection: "column",
        }}
      >
        <div className="spinner"></div>
        <p>Validando autenticação...</p>
      </div>
    );
  }

  if (!isAuthenticated) {
    return <SignIn />;
  }

  return (
    <Router>
      <Routes>
        {/* Owner/User */}
        <Route path="/" element={<Home />} />

        <Route path="/jwt" element={<JWT />} />

        {/* --- Rotas Protegidas Owner --- */}
        <Route
          path="/dashboard"
          element={
            <RoleGuard role="owner">
              <Dashboard />
            </RoleGuard>
          }
        />
      </Routes>
    </Router>
  );
}

export default App;
