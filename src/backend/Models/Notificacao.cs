using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public enum TipoNotificacao
    {
        Informacao,
        Alerta,
        Erro,
        Sucesso
    }

    public class Notificacao
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public string Mensagem { get; set; }
        public TipoNotificacao Tipo { get; set; }
        public bool Lida { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UtilizadorId { get; set; }
    }
}
