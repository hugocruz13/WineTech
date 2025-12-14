using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICompraBLL
    {
        Task<bool> ProcessarCarrinho(string utilizadorId);
        Task<List<Compra>> ObterComprasPorUtilizador(string utilizadorId);
        Task<List<CompraDetalhe>> ObterCompraPorId(int compraId);
    }
}
