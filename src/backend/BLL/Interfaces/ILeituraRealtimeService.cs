using System.Threading.Tasks;
using Models;

namespace BLL.Interfaces
{
    public interface ILeituraRealtimeService
    {
        Task SendToUserAsync(string userId, Leituras leitura);
    }
}
