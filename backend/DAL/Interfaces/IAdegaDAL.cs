using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface IAdegaDAL
    {
        Task<Adega> InserirAdega(Models.Adega adega);
        Task< List<Adega>> TodasAdegas();
        Task<Adega> AdegaById(int id);
        Task<Adega> ModificarAdega(Models.Adega adega);
        Task<bool> ApagarAdega(int id);
        Task<List<Models.StockResumo>> ObterResumoPorAdega(int id);
        Task<bool> AdicionarStock(Models.StockInput stock);
        Task<bool> AtualizarStock(Models.StockInput stock);
        Task<int> ObterCapacidadeAtual(int adegaId);
    }
}
