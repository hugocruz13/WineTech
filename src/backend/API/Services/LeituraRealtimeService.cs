using API.Hubs;
using BLL.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Models;

namespace API.Services
{
    public class LeituraRealtimeService : ILeituraRealtimeService
    {
        private readonly IHubContext<LeituraHub> _hub;

        public LeituraRealtimeService(IHubContext<LeituraHub> hub)
        {
            _hub = hub;
        }

        public async Task SendToUserAsync(string userId, Leituras leitura)
        {
            await _hub.Clients
                .Group(userId)
                .SendAsync("ReceiveLeitura", new
                {
                    sensorId = leitura.SensorId,
                    valor = leitura.Valor,
                    tipo = leitura.Tipo,
                    adegaId = leitura.AdegaId,
                    dataHora = leitura.DataHora
                });
        }
    }
}
