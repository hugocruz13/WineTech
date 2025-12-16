using BLL.Interfaces;
using DAL.Interfaces;
using Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class NotificacaoBLL: INotificacaoBLL
    {
        private readonly INotificacaoDAL _notificacaoDAL;
       private readonly IUtilizadorDAL _utilizadorDAL;

        public NotificacaoBLL(INotificacaoDAL notificacaoDAL, IUtilizadorDAL utilizadorDAL)
        {
            _notificacaoDAL = notificacaoDAL;
            _utilizadorDAL = utilizadorDAL;
        }

        public async Task<Notificacao> InserirNotificacao(Notificacao notificacao)
        {
            if (notificacao == null)
            {
                throw new System.ArgumentNullException(nameof(notificacao));
            }

            if (string.IsNullOrWhiteSpace(notificacao.UtilizadorId))
            {
                throw new ArgumentException("O utilizador da notificação é obrigatório.");
            }

            if (string.IsNullOrWhiteSpace(notificacao.Mensagem))
            {
                throw new System.ArgumentException("A mensagem da notificação não pode ser vazia.");
            }

            if (await _utilizadorDAL.GetUserByIdAsync(notificacao.UtilizadorId) == null)
            {
                throw new System.ArgumentException("Utilizador inválido para a notificação.");
            }

            return await _notificacaoDAL.InserirNotificacao(notificacao);
        }

        public async Task<List<Notificacao>> ObterNotificacoesPorUtilizador(string utilizadorId)
        {
            if (string.IsNullOrWhiteSpace(utilizadorId))
            {
                throw new ArgumentException("O utilizador da notificação é obrigatório.");
            }

            if (await _utilizadorDAL.GetUserByIdAsync(utilizadorId) == null)
            {
                throw new System.ArgumentException("Utilizador inválido para a notificação.");
            }

            return await _notificacaoDAL.ObterNotificacoesPorUtilizador(utilizadorId);
        }

        public async Task<bool> MarcarNotificacaoComoLida(int idNotificacao)
        {
            return await _notificacaoDAL.MarcarNotificacaoComoLida(idNotificacao);
        }
    }
}
