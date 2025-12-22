import { Info, Bell, AlertTriangle, XCircle, CheckCircle } from "lucide-react";

export const notificationConfig = {
  0: {
    label: "Informação",
    icon: Bell,
    className: "info",
  },
  1: {
    label: "Alerta",
    icon: Info,
    className: "alert",
  },
  2: {
    label: "Erro",
    icon: XCircle,
    className: "error",
  },
  3: {
    label: "Sucesso",
    icon: CheckCircle,
    className: "success",
  },
};
