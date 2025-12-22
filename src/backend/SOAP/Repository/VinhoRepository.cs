using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SOAP.Repository
{
    public class VinhoRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public VinhoRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Vinho InserirVinho(Vinho vinho)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirVinho", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nome", vinho.Nome);
                cmd.Parameters.AddWithValue("@Produtor", vinho.Produtor);
                cmd.Parameters.AddWithValue("@Ano", vinho.Ano);
                cmd.Parameters.AddWithValue("@Tipo", vinho.Tipo);
                cmd.Parameters.AddWithValue("@Descricao", vinho.Descricao);
                cmd.Parameters.AddWithValue("@Preco", vinho.Preco);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Vinho
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Produtor = reader["Produtor"].ToString(),
                            Ano = Convert.ToInt32(reader["Ano"]),
                            Tipo = reader["Tipo"].ToString(),
                            Descricao = reader["Descricao"].ToString(),
                            ImagemUrl = reader["ImagemUrl"].ToString(),
                            Preco = (float)reader["Preco"]
                        };
                    }
                }
            }

            return null;
        }
        public Vinho ModificarVinho(Vinho vinho)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ModificarVinho", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", vinho.Id);
                cmd.Parameters.AddWithValue("@Nome", vinho.Nome);
                cmd.Parameters.AddWithValue("@Produtor", vinho.Produtor);
                cmd.Parameters.AddWithValue("@Ano", vinho.Ano == 0 ? DBNull.Value : (object)vinho.Ano);
                cmd.Parameters.AddWithValue("@Tipo", vinho.Tipo);
                cmd.Parameters.AddWithValue("@Descricao", vinho.Descricao);
                cmd.Parameters.AddWithValue("@ImagemUrl", vinho.ImagemUrl);
                cmd.Parameters.AddWithValue("@Preco", vinho.Preco == 0 ? DBNull.Value : (object)vinho.Preco);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Vinho
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Produtor = reader["Produtor"].ToString(),
                            Ano = Convert.ToInt32(reader["Ano"]),
                            Tipo = reader["Tipo"].ToString(),
                            Descricao = reader["Descricao"].ToString(),
                            ImagemUrl = reader["ImagemUrl"].ToString(),
                            Preco = (float)reader["Preco"]
                        };
                    }
                }
            }

            return null;
        }
        public Vinho VinhoById(int id)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("VinhoById", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Vinho
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Produtor = reader["Produtor"].ToString(),
                            Ano = Convert.ToInt32(reader["Ano"]),
                            Tipo = reader["Tipo"].ToString(),
                            Descricao = reader["Descricao"].ToString(),
                            ImagemUrl = reader["ImagemUrl"].ToString(),
                            Preco = (float)reader["Preco"]
                        };
                    }
                }
            }
            return null;
        }
        public List<Vinho> TodosVinhos()
        {
            List<Vinho> lista = new List<Vinho>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("TodosVinhos", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Vinho
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nome = reader["Nome"].ToString(),
                            Produtor = reader["Produtor"].ToString(),
                            Ano = Convert.ToInt32(reader["Ano"]),
                            Tipo = reader["Tipo"].ToString(),
                            Descricao = reader["Descricao"].ToString(),
                            ImagemUrl = reader["ImagemUrl"].ToString(),
                            Preco = (float)reader["Preco"]
                        });
                    }
                }
            }
            return lista;
        }
        public bool ApagarVinho(int id)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ApagarVinho", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                conn.Open();
                return cmd.ExecuteNonQuery() > 0;
            }
        }
    }
}