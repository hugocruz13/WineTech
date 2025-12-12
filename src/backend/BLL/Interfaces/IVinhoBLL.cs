using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IVinhoBLL
    {
        Task<Vinho> InserirVinho(Vinho vinho);
        Task<List<Vinho>> TodosVinhos();
        Task<Vinho> VinhoById(int id);
        Task<Vinho> ModificarVinho(Vinho vinho);
        Task<bool> ApagarVinho(int id);

    }
}
