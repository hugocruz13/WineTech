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
        Task<List<Models.Carrinho>>InserirItem(Models.Carrinho itemCarrinho, string utilizadoresId);
        Task<List<Carrinho>> AumentarItemCarrinho(Models.Carrinho itemCarrinho, string userid);
        Task<List<Carrinho>> DiminuirItemCarrinho(Models.Carrinho itemCarrinho, string userid);
        Task<bool>  EliminarItem(int vinhoId, string utilizadoresId);
        Task<List<CarrinhoDetalhe>> ObterDetalhesCarrinho(string utilizadoresId);

    }
}
