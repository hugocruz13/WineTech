using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SOAP.Repository
{
    public class AdegaRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AdegaRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public int InserirAdega(string localizacao)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirAdega", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Localizacao", localizacao);
                conn.Open();

                var res = cmd.ExecuteScalar();
                return (res != null && res != DBNull.Value) ? Convert.ToInt32(res) : 0;
            }
        }

        public List<Adega> TodasAdegas()
        {
            List<Adega> lista = new List<Adega>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("TodasAdegas", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Adega
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Localizacao = reader["Localizacao"]?.ToString()
                        });
                    }
                }
            }
            return lista;
        }

        public Adega AdegaById(int id)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("AdegaById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Adega
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Localizacao = reader["Localizacao"]?.ToString()
                        };
                    }
                }
            }
            return null;
        }

        public bool ModificarAdega(Adega adega)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ModificarAdega", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", adega.Id);
                cmd.Parameters.AddWithValue("@Localizacao", adega.Localizacao);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }

        public bool ApagarAdega(int id)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ApagarAdega", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}