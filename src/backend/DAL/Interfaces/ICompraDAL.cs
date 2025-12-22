using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface ICompraDAL
    {
        Task<Models.Compra> CriarCompra(Models.Compra compra);
        Task<List<int>> ObterStockIds(Models.StockInput stockInput);
        Task<bool> CriarLinha(Models.LinhaCompra linha);
        Task<bool> FinalizarCompra(Models.LinhaCompra linha);
        Task<bool> AtualizarValorTotal(Models.Compra compra);
        Task<List<Models.Compra>> ObterComprasUtilizador(string utilizadorId);
        Task<List<Models.CompraDetalhe>> ObterDetalhesCompra(int compraId);
    }
}
