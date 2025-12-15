import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";

import SignIn from "./pages/SignIn";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import RoleGuard from "./components/RoleGuard";
import Loading from "./components/Loading";
import WineDetail from "./pages/WineDetail";
import JWT from "./pages/Jwt";

function App() {
  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading) {
    return <Loading />;
  }

  if (!isAuthenticated) {
    return <SignIn />;
  }

  return (
    <Router>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/vinho/:id" element={<WineDetail />} />
        <Route
          path="/dashboard"
          element={
            <RoleGuard role="owner">
              <Dashboard />
            </RoleGuard>
          }
        />

        <Route path="/jwt" element={<JWT />} />
      </Routes>
    </Router>
  );
}

export default App;
