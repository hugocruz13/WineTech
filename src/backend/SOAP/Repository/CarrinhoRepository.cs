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

        public List<Carrinho> ObterCarrinho(int utilizadoresId)
        {
            List<Carrinho> lista = new List<Carrinho>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ObterCarrinho", conn))
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
                            Id = Convert.ToInt32(reader["Id"]),
                            VinhosId = Convert.ToInt32(reader["VinhosId"]),
                            UtilizadoresId = Convert.ToInt32(reader["UtilizadoresId"]),
                            Quantidade = Convert.ToInt32(reader["Quantidade"])
                        });
                    }
                }
            }
            return lista;
        }
        public List<Carrinho> InserirItem(int utilizadoresId, int vinhoId, int quantidade)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirItemCarrinho", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UtilizadoresId", utilizadoresId);
                cmd.Parameters.AddWithValue("@VinhosId", vinhoId);
                cmd.Parameters.AddWithValue("@Quantidade", quantidade);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return ObterCarrinho(utilizadoresId);
        }
        public List<Carrinho> AtualizarItem(int itemId, int utilizadoresId, int quantidade)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("AtualizarItem", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", itemId);
                cmd.Parameters.AddWithValue("@UtilizadoresId", utilizadoresId);
                cmd.Parameters.AddWithValue("@Quantidade", quantidade);

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return ObterCarrinho(utilizadoresId);
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