import { useEffect } from "react";
import { useAuth0 } from "@auth0/auth0-react";

const Admin = () => {
  const { getAccessTokenSilently, isAuthenticated } = useAuth0();

  useEffect(() => {
    if (!isAuthenticated) return;

    let socket;

    const connectWebSocket = async () => {
      try {
        // 🔐 obter JWT do Auth0
        const token = await getAccessTokenSilently();

        // 🔌 criar WebSocket (ajusta a porta se necessário)
        socket = new WebSocket(`wss://localhost:7254/ws?access_token=${token}`);

        socket.onopen = () => {
          console.log("✅ WebSocket conectado");
        };

        socket.onmessage = (event) => {
          const data = JSON.parse(event.data);
          console.log("🔔 Notificação recebida:", data);
        };

        socket.onclose = () => {
          console.log("❌ WebSocket fechado");
        };

        socket.onerror = (err) => {
          console.error("WebSocket erro:", err);
        };
      } catch (err) {
        console.error("Erro ao ligar WebSocket", err);
      }
    };

    connectWebSocket();

    // cleanup quando sair da página
    return () => {
      if (socket) {
        socket.close();
      }
    };
  }, [isAuthenticated, getAccessTokenSilently]);

  return <h1>Admin</h1>;
};

export default Admin;
