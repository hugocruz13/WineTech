using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUtilizadorDAL
    {
        Task<Models.Utilizador> AddUserAsync(Models.Utilizador user);
        Task<Models.Utilizador> GetUserByIdAsync(string id);
        Task<List<Models.Utilizador>> GetOwnersAsync();
        Task<Models.Utilizador> UpdateUserAsync(Models.Utilizador utilizador);
    }
}
