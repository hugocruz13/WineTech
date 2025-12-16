using Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Interfaces
{
    public interface INotificacaoBLL
    {
        Task<Notificacao> InserirNotificacao(Notificacao notificacao);
        Task<List<Notificacao>> ObterNotificacoesPorUtilizador(string utilizadorId);
        Task<bool> MarcarNotificacaoComoLida(int idNotificacao);
    }
}
