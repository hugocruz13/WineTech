import {
  BrowserRouter as Router,
  Routes,
  Route,
  useNavigate,
  useLocation,
} from "react-router-dom";
import { useAuth0 } from "@auth0/auth0-react";
import { ToastContainer } from "react-toastify";
import { useRef, useEffect, useState } from "react";
import "react-toastify/dist/ReactToastify.css";

import useNotificationSocket from "./hooks/useNotificationSocket";
import { showNotificationToast } from "./utils/notificationToast";

import SignInPage from "./pages/SignInPage";
import FinalizarCompraPage from "./pages/FinalizarCompraPage";
import HomePage from "./pages/HomePage";
import AdegaPage from "./pages/AdegasPage";
import ComprasDetalhes from "./pages/ComprasDetalhes";
import ComprasPage from "./pages/ComprasPage";
import RoleGuard from "./components/RoleGuard";
import Loading from "./components/Loading";
import WineDetailPage from "./pages/WineDetailPage";
import NotificationsPage from "./pages/NotificationsPage";
import IotClientePage from "./pages/IotClientePage";
import CarrinhoPage from "./pages/CarrinhoPage";
import JWT from "./pages/Jwt";

function AppContent() {
  const navigate = useNavigate();
  const location = useLocation();

  const pendingRef = useRef([]);
  const [tick, setTick] = useState(0);
  const [notifications, setNotifications] = useState([]);

  useNotificationSocket((notification) => {
    setNotifications((prev) => {
      const exists = prev.some((n) => n.id === notification.id);
      if (exists) return prev;
      return [notification, ...prev];
    });

    pendingRef.current.push(notification);
    setTick((t) => t + 1);

    window.dispatchEvent(
      new CustomEvent("notification:received", { detail: notification })
    );
  });

  useEffect(() => {
    if (pendingRef.current.length === 0) return;

    const notification = pendingRef.current.shift();

    if (location.pathname !== "/notificacoes") {
      showNotificationToast(notification, () => navigate("/notificacoes"));
    }
  }, [tick, location.pathname, navigate]);

  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/vinho/:id" element={<WineDetailPage />} />
      <Route
        path="/notificacoes"
        element={
          <NotificationsPage
            notifications={notifications}
            setNotifications={setNotifications}
          />
        }
      />
      <Route path="/finalizar" element={<FinalizarCompraPage />} />
      <Route path="/compras" element={<ComprasPage />} />
      <Route path="/compra/:id" element={<ComprasDetalhes />} />
      <Route path="/carrinho" element={<CarrinhoPage />} />
      <Route path="/iot/:stockId" element={<IotClientePage />} />
      {/* Apagar em produção */}
      <Route path="/jwt" element={<JWT />} />
      {/*Owner */}
      <Route
        path="/dashboard"
        element={
          <RoleGuard role="owner">
            <AdegaPage />
          </RoleGuard>
        }
      />
    </Routes>
  );
}

function App() {
  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading) return <Loading />;
  if (!isAuthenticated) return <SignInPage />;

  return (
    <Router>
      <ToastContainer
        position="top-right"
        autoClose={1500}
        closeOnClick
        pauseOnHover
        draggable
        style={{ marginTop: "80px" }}
      />

      <AppContent />
    </Router>
  );
}

export default App;
