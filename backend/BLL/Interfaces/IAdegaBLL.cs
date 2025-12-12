using Models;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface IAdegaBLL
    {
        Task<Adega> InserirAdega(Adega adega);
        Task<List<Adega>> TodasAdegas();
        Task<Adega> AdegaById(int id);
        Task<Adega> ModificarAdega(Adega adega);
        Task<bool> ApagarAdega(int id);
        Task<List<StockResumo>> ObterResumoPorAdega(int adegaId);
        Task<bool> AdicionarStock(StockInput stock);
        Task<bool> AtualizarStock(StockInput stock);
        Task<int> ObterCapacidadeAtual(int adegaId);
        Task<List<StockResumo>> ObterResumoStockTotal();

    }
}
