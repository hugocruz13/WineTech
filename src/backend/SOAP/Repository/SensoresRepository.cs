using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SOAP.Repository
{
    public class SensoresRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public SensoresRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public Models.Sensores InserirSensor(Models.Sensores sensor)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirSensor", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IdentificadorHardware", sensor.IdentificadorHardware);
                cmd.Parameters.AddWithValue("@Tipo", sensor.Tipo);
                cmd.Parameters.AddWithValue("@Estado", sensor.Estado);
                cmd.Parameters.AddWithValue("@AdegaId", sensor.AdegaId);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Models.Sensores
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            IdentificadorHardware = reader["IdentificadorHardware"].ToString(),
                            Tipo = reader["Tipo"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            AdegaId = Convert.ToInt32(reader["AdegaId"])
                        };
                    }
                }
            }

            return null;
        }


        public List<Models.Sensores> TodosSensores()
        {
            List<Models.Sensores> lista = new List<Models.Sensores>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ObterSensores", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Models.Sensores
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            IdentificadorHardware = reader["IdentificadorHardware"].ToString(),
                            Tipo = reader["Tipo"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            AdegaId = Convert.ToInt32(reader["AdegaId"])
                        });
                    }
                }
            }
            return lista;
        }
        public List<Models.Sensores> ObterSensoresPorAdega(int adegaId)
        {
            List<Models.Sensores> lista = new List<Models.Sensores>();
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ObterSensoresPorAdega", conn))
            {

                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AdegaId", adegaId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Models.Sensores
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            IdentificadorHardware = reader["IdentificadorHardware"].ToString(),
                            Tipo = reader["Tipo"].ToString(),
                            Estado = Convert.ToBoolean(reader["Estado"]),
                            AdegaId = Convert.ToInt32(reader["AdegaId"])
                        });

                    }
                }
            }
            return lista;
        }
    }
}