using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SOAP.Repository
{
    public class NotificacaoRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public NotificacaoRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Notificacao InserirNotificacao(Notificacao notificacao)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirNotificacao", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Titulo", notificacao.Titulo);
                cmd.Parameters.AddWithValue("@Mensagem", notificacao.Mensagem);
                cmd.Parameters.AddWithValue("@Tipo", notificacao.Tipo);
                cmd.Parameters.AddWithValue("@UtilizadoresId", notificacao.UtilizadorId);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        notificacao = new Notificacao
                        {
                            Id = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Mensagem = reader.GetString(2),
                            Tipo = (TipoNotificacao)Enum.Parse(typeof(TipoNotificacao), reader.GetString(3), true),
                            Lida = reader.GetBoolean(4),
                            CreatedAt = reader.GetDateTime(5),
                            UtilizadorId = reader.GetString(6)
                        };
                    }
                }
            }

            return notificacao;
        }


        public List<Notificacao> ObterNotificacoesPorUtilizador(string utilizadorId)
        {
            var notificacoes = new List<Notificacao>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("NotificacoesPorUtilizador", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UtilizadorId", utilizadorId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notificacoes.Add(new Notificacao
                        {
                            Id = reader.GetInt32(0),
                            Titulo = reader.GetString(1),
                            Mensagem = reader.GetString(2),
                            Tipo = (TipoNotificacao)Enum.Parse(typeof(TipoNotificacao), reader.GetString(3), true),
                            Lida = reader.GetBoolean(4),
                            CreatedAt = reader.GetDateTime(5),
                            UtilizadorId = reader.GetString(6)
                        });
                    }
                }
            }
            return notificacoes;
        }

        public bool MarcarComoLida(int notificacaoId)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("MarcarNotificacaoComoLida", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", notificacaoId);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}