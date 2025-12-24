using Models;
using System;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IUtilizadorBLL
    {
        Task<Utilizador> RegisterUserAsync(Utilizador user);
        Task<Utilizador> GetUserByIdAsync(string id);
        // Atualiza Nome, Email e ImgUrl; ignora IsAdmin
        Task<Utilizador> UpdateUserAsync(Utilizador utilizador);
    }
}
