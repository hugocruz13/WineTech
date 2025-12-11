using Models;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IUtilizadorDAL
    {
        Task<int> AddUserAsync(Utilizador user);
    }
}
