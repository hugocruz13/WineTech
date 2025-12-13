using Models;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUtilizadorDAL
    {
        Task<Models.Utilizador> AddUserAsync(Models.Utilizador user);
        Task<Models.Utilizador> GetUserByIdAsync(string id);
    }
}
