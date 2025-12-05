using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SOAP.Repository
{
    public class AdegaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AdegaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<int> InserirAdegaAsync(string localizacao)
        {
            if (string.IsNullOrWhiteSpace(localizacao))
                throw new ArgumentException("A localização não pode ser nula ou vazia", nameof(localizacao));

            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirAdega", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Localizacao", localizacao);

                await conn.OpenAsync();
                return Convert.ToInt32(await cmd.ExecuteScalarAsync());
            }
        }

        public async Task<List<Adega>> TodasAdegasAsync()
        {
            var lista = new List<Adega>();

            using (var conn = _connectionFactory.GetConnection())
            {
                using (var cmd = new SqlCommand("TodasAdegas", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            lista.Add(new Adega
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Localizacao = reader["Localizacao"].ToString()
                            });
                        }
                    }
                }
            }

            return lista;
        }

        public async Task<Adega> AdegaByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero", nameof(id));

            using (var conn = _connectionFactory.GetConnection())
            {
                using (var cmd = new SqlCommand("AdegaById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    await conn.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Adega
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Localizacao = reader["Localizacao"].ToString()
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<bool> ModificarAdegaAsync(Adega adega)
        {
            if (adega == null)
                throw new ArgumentNullException(nameof(adega), "A adega não pode ser nula");

            using (var conn = _connectionFactory.GetConnection())
            {
                using (var cmd = new SqlCommand("ModificarAdega", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", adega.Id);
                    cmd.Parameters.AddWithValue("@Localizacao", adega.Localizacao);

                    await conn.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }

        public async Task<bool> ApagarAdegaAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("O ID deve ser maior que zero", nameof(id));

            using (var conn = _connectionFactory.GetConnection())
            {
                using (var cmd = new SqlCommand("ApagarAdega", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Id", id);

                    await conn.OpenAsync();
                    int rows = await cmd.ExecuteNonQueryAsync();
                    return rows > 0;
                }
            }
        }
    }
}
