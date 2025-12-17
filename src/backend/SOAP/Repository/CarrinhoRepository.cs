using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SOAP.Repository
{
    public class CarrinhoRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CarrinhoRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public List<Carrinho> ObterCarrinhoPorUtilizador(string utilizadoresId)
        {
            List<Carrinho> lista = new List<Carrinho>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ObterCarrinhoPorUtilizador", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UtilizadoresId", utilizadoresId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Carrinho
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            VinhosId = reader.GetInt32(reader.GetOrdinal("VinhosId")),
                            UtilizadoresId = reader["UtilizadoresId"].ToString(),
                            Quantidade = reader.GetInt32(reader.GetOrdinal("Quantidade"))
                        });

                    }
                }
            }
            return lista;
        }
        public List<Carrinho> InserirItem(Carrinho itemCarrinho)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirItemCarrinho", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UtilizadoresId", itemCarrinho.UtilizadoresId);
                cmd.Parameters.AddWithValue("@VinhosId", itemCarrinho.VinhosId);
                cmd.Parameters.AddWithValue("@Quantidade", itemCarrinho.Quantidade);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return ObterCarrinhoPorUtilizador(itemCarrinho.UtilizadoresId);
        }
        public List<Carrinho> AtualizarItem(Carrinho itemCarrinho)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("AtualizarItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@VinhosId", itemCarrinho.VinhosId);
                cmd.Parameters.AddWithValue("@UtilizadoresId", itemCarrinho.UtilizadoresId);
                cmd.Parameters.AddWithValue("@Quantidade", itemCarrinho.Quantidade);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return ObterCarrinhoPorUtilizador(itemCarrinho.UtilizadoresId);
        }
        public bool EliminarItem(int vinhoId, string utilizadoresId)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("EliminarItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@VinhosId", vinhoId);
                cmd.Parameters.AddWithValue("@UtilizadoresId", utilizadoresId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool EliminarCarrinho(string utilizadoresId)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("EliminarCarrinho", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UtilizadorId", utilizadoresId);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public List<CarrinhoDetalhe> ObterDetalhesCarrinho(string utilizadoresId)
        {
            List<CarrinhoDetalhe> lista = new List<CarrinhoDetalhe>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ObterDetalhesCarrinho", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UtilizadoresId", utilizadoresId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new CarrinhoDetalhe
                        {
                            VinhosId = reader.GetInt32(0),
                            NomeVinho = reader.GetString(1),
                            Produtor = reader.GetString(2),
                            Ano = reader.GetInt32(3),
                            Tipo = reader.GetString(4),
                            Descricao = reader.GetString(5),
                            ImagemUrl = reader.GetString(6),
                            Preco = Math.Round((double)reader.GetFloat(7), 2),
                            Quantidade = reader.GetInt32(8)
                        });

                    }
                }
            }
            return lista;
        }

    }
}