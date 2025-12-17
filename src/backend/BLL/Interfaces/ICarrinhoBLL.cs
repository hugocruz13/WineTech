using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface ICarrinhoBLL
    {
        Task<List<Models.Carrinho>> ObterCarrinhoPorUtilizador(string utilizadoresId);

        Task<List<Models.Carrinho>>InserirItem(Models.Carrinho itemCarrinho);

        Task<List<Models.Carrinho>> AtualizarItem(Models.Carrinho itemCarrinho);

        Task<bool>  EliminarItem(int vinhoId, string utilizadoresId);
        Task<List<CarrinhoDetalhe>> ObterDetalhesCarrinho(string utilizadoresId);
    }
}
