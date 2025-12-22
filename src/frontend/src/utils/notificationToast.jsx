import { toast } from "react-toastify";

export function showNotificationToast(notification, onClick) {
  const content = (
    <div>
      <strong>{notification.titulo}</strong>
      <p style={{ margin: 0 }}>{notification.mensagem}</p>
      <small>Clique para ver</small>
    </div>
  );

  switch (notification.tipo) {
    case 0:
      toast.info(content, { onClick });
      break;
    case 1:
      toast.warning(content, { onClick });
      break;
    case 2:
      toast.error(content, { onClick });
      break;
    case 3:
      toast.success(content, { onClick });
      break;
    default:
      toast(content, { onClick });
  }
}
