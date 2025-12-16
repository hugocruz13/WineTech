using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Interfaces
{
    public interface INotificacaoDAL
    {
        Task<Models.Notificacao> InserirNotificacao(Models.Notificacao notificacao);
        Task<List<Models.Notificacao>> ObterNotificacoesPorUtilizador(string utilizadorId);
        Task<bool> MarcarNotificacaoComoLida(int idNotificacao);

    }
}
