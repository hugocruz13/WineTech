using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SOAP.Repository
{
    public class LeiturasRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public LeiturasRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public List<Models.Leituras> InserirLeitura(Models.Leituras leitura)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirLeitura", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SensorId", leitura.SensorId);
                cmd.Parameters.AddWithValue("@Valor", Convert.ToSingle(leitura.Valor));

                conn.Open();
                cmd.ExecuteNonQuery();
            }

            return ObterLeiturasPorSensor(leitura.SensorId);
        }
        public List<Models.Leituras> ObterLeiturasPorSensor(int sensorId)
        {
            List<Models.Leituras> lista = new List<Models.Leituras>();

            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ObterLeiturasPorSensor", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SensorId", sensorId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Models.Leituras
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SensorId = Convert.ToInt32(reader["SensorId"]),
                            Valor = Convert.ToSingle(reader["Valor"]),
                            DataHora = Convert.ToDateTime(reader["DataHora"])
                        });
                    }
                }
            }
            return lista;
        }
    }
}