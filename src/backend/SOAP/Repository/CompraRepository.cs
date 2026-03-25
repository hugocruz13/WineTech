using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SOAP.Repository
{
    public class CompraRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public CompraRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public List<int> ObterStockIds(StockInput stockInput)
        {
            var stockIds = new List<int>();

            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("SelecionarStockPorVinho", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@VinhoId", stockInput.VinhoId);
                cmd.Parameters.AddWithValue("@Quantidade", stockInput.Quantidade);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        stockIds.Add(reader.GetInt32(0));
                    }
                }
            }

            return stockIds;
        }

        public Compra CriarCompra(Compra compra)
        {
            Compra novaCompra = null;
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("CriarCompra", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UtilizadorId", compra.UtilizadorId);
                cmd.Parameters.AddWithValue("@Estado", compra.Estado);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        novaCompra = new Compra
                        {
                            Id = reader.GetInt32(0),
                            DataCompra = reader.GetDateTime(1),
                            UtilizadorId = compra.UtilizadorId,
                            ValorTotal = compra.ValorTotal,
                            Estado = reader.GetString(4),
                            cartao = reader.IsDBNull(5) ? 0 : reader.GetInt32(5)
                        };
                    }
                }
            }
            return novaCompra;
        }

        public bool CriarLinhas(LinhaCompra linha)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("LinhaCompra", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StockId", SqlDbType.Int);
                cmd.Parameters.AddWithValue("@CompraId", linha.CompraId);

                conn.Open();
                int rowsAffected = 0;

                foreach (var stockId in linha.stockIds)
                {
                    cmd.Parameters["@StockId"].Value = stockId;
                    rowsAffected += cmd.ExecuteNonQuery();
                }

                return rowsAffected > 0;
            }
        }


        public bool Finalizar(LinhaCompra linha)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("FinalizarCompra", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@StockId", SqlDbType.Int);

                conn.Open();
                int rowsAffected = 0;

                foreach (var stockId in linha.stockIds)
                {
                    cmd.Parameters["@StockId"].Value = stockId;
                    rowsAffected += cmd.ExecuteNonQuery();
                }

                return rowsAffected > 0;
            }
        }

        public bool AtualizarValorTotal(Compra compra)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("AtualizarValorCompra", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompraId", compra.Id);
                cmd.Parameters.AddWithValue("@Valor", compra.ValorTotal);
                cmd.Parameters.AddWithValue("@Estado", compra.Estado);
                cmd.Parameters.AddWithValue("@Cartao", compra.cartao);
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public List<Compra> ObterComprasPorUtilizador(string utilizadorId)
        {
            var compras = new List<Compra>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ComprasPorUtilizador", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UtilizadorId", utilizadorId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        compras.Add(new Compra
                        {
                            Id = GetInt32(reader, "Id"),
                            DataCompra = GetDateTime(reader, "DataCompra"),
                            ValorTotal = GetDoubleFromDecimal(reader, "ValorTotal"),
                            Estado = GetString(reader, "Estado"),
                            cartao = GetInt32(reader, "Cartao"),
                            UtilizadorId = GetString(reader, "UtilizadoresId")
                        });
                    }
                }
            }
            return compras;
        }

        public List<CompraDetalhe> CompraDetalhes(int idCompra)
        {
            var detalhes = new List<CompraDetalhe>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("DetalhesCompra", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CompraId", idCompra);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        detalhes.Add(new CompraDetalhe
                        {
                            IdCompra = GetInt32(reader, 0),
                            DataCompra = GetDateTime(reader, 1),
                            ValorTotal = GetDoubleFromDecimal(reader, 2),
                            IdVinho = GetInt32(reader, 3),
                            Nome = GetString(reader, 4),
                            Produtor = GetString(reader, 5),
                            Ano = GetInt32(reader, 6),
                            Tipo = GetString(reader, 7),
                            Quantidade = GetInt32(reader, 8),
                            Preco = GetFloat(reader, 9),
                            ImgVinho = GetString(reader, 10),
                            NomeUtilizador = GetString(reader, 11),
                            EmailUtilizador = GetString(reader, 12),
                            ImagemUtilizador = GetString(reader, 13),
                            StockId = GetInt32(reader, 14),
                            Cartao = GetInt32(reader, 15),
                            idUtilizador = GetString(reader, 16)
                        });
                    }
                }
            }
            return detalhes;
        }

        private static int GetInt32(SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal);
        }

        private static int GetInt32(SqlDataReader reader, string columnName)
        {
            return GetInt32(reader, reader.GetOrdinal(columnName));
        }

        private static DateTime GetDateTime(SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? DateTime.MinValue : reader.GetDateTime(ordinal);
        }

        private static DateTime GetDateTime(SqlDataReader reader, string columnName)
        {
            return GetDateTime(reader, reader.GetOrdinal(columnName));
        }

        private static double GetDoubleFromDecimal(SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? 0 : Convert.ToDouble(reader.GetDecimal(ordinal));
        }

        private static double GetDoubleFromDecimal(SqlDataReader reader, string columnName)
        {
            return GetDoubleFromDecimal(reader, reader.GetOrdinal(columnName));
        }

        private static float GetFloat(SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? 0 : reader.GetFloat(ordinal);
        }

        private static string GetString(SqlDataReader reader, int ordinal)
        {
            return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
        }

        private static string GetString(SqlDataReader reader, string columnName)
        {
            return GetString(reader, reader.GetOrdinal(columnName));
        }

    }
}