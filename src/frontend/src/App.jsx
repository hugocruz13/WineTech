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

import SignIn from "./pages/SignIn";
import Home from "./pages/Home";
import Dashboard from "./pages/Dashboard";
import RoleGuard from "./components/RoleGuard";
import Loading from "./components/Loading";
import WineDetail from "./pages/WineDetail";
import NotificationsPage from "./pages/NotificationsPage";
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
      <Route path="/" element={<Home />} />
      <Route path="/vinho/:id" element={<WineDetail />} />
      <Route
        path="/notificacoes"
        element={
          <NotificationsPage
            notifications={notifications}
            setNotifications={setNotifications}
          />
        }
      />
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
  );
}

function App() {
  const { isAuthenticated, isLoading } = useAuth0();

  if (isLoading) return <Loading />;
  if (!isAuthenticated) return <SignIn />;

  return (
    <Router>
      <ToastContainer
        position="top-right"
        autoClose={5000}
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
