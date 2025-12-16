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

        //List<Carrinho> AtualizarItem(int itemId, int utilizadoresId, int quantidade);

        //bool ApagarItem(int itemId, int utilizadoresId);
    }
}
