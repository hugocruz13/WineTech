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
        public bool EliminarItem(int itemId, int utilizadoresId)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("EliminarItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", itemId);
                cmd.Parameters.AddWithValue("@UtilizadoresId", utilizadoresId);

                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

    }
}