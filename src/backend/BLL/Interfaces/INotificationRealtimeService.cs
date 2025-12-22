using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface INotificationRealtimeService
    {
        Task SendToUserAsync(string userId, Notificacao notificacao);
    }
}
