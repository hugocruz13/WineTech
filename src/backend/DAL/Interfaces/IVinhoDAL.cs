using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IVinhoDAL
    {
        Task<Vinho> InserirVinho(Models.Vinho vinho);
        Task<List<Vinho>> TodosVinhos();
        Task<Vinho> VinhoById(int id);
        Task<Vinho> ModificarVinho(Models.Vinho vinho);
        Task<bool> ApagarVinho(int id);
    }
}
