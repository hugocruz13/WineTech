using API.Hubs;
using BLL.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace API.Services
{
    public class NotificationRealtimeService : INotificationRealtimeService
    {
        private readonly IHubContext<NotificationHub> _hub;

        public NotificationRealtimeService(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendToUserAsync(string userId, Notificacao notificacao)
        {
            await _hub.Clients
                .Group(userId)
                .SendAsync("ReceiveNotification", new
                {
                    id = notificacao.Id,
                    titulo = notificacao.Titulo,
                    mensagem = notificacao.Mensagem,
                    tipo = notificacao.Tipo,
                    createdAt = notificacao.CreatedAt,
                    lida = notificacao.Lida
                });
        }
    }
}
