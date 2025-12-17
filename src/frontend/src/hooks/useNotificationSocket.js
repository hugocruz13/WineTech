import { useEffect, useRef } from "react";
import * as signalR from "@microsoft/signalr";
import { useAuth0 } from "@auth0/auth0-react";

const API_URL = import.meta.env.VITE_API_URL;

export default function useNotificationSocket(onNotification) {
  const { getAccessTokenSilently } = useAuth0();
  const startedRef = useRef(false);

  useEffect(() => {
    if (startedRef.current) return;
    startedRef.current = true;

    let connection;

    const start = async () => {
      const token = await getAccessTokenSilently();

      connection = new signalR.HubConnectionBuilder()
        .withUrl(`${API_URL}/hubs/notifications`, {
          accessTokenFactory: () => token,
        })
        .withAutomaticReconnect()
        .build();

      connection.on("ReceiveNotification", onNotification);

      await connection.start();
      console.log("🔌 SignalR global ligado");
    };

    start();

    return () => {
      if (connection) {
        connection.stop();
        startedRef.current = false;
      }
    };
  }, [getAccessTokenSilently, onNotification]);
}
