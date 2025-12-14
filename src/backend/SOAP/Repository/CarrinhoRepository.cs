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

    }
}