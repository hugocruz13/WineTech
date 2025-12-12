using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.Remoting.Messaging;

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
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        novaCompra = new Compra
                        {
                            Id = reader.GetInt32(0),
                            UtilizadorId = compra.UtilizadorId,
                            ValorTotal = compra.ValorTotal,
                            DataCompra = reader.GetDateTime(1)
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
                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }
    }
}