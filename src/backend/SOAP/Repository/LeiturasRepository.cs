using SOAP.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace SOAP.Repository
{
    public class LeiturasRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public LeiturasRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }
        public Models.Leituras InserirLeitura(Models.Leituras leitura)
        {
            Models.Leituras leituraCriada = new Models.Leituras();

            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirLeitura", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SensorId", leitura.SensorId);
                cmd.Parameters.AddWithValue("@Valor", Convert.ToSingle(leitura.Valor));

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        leituraCriada.Id = Convert.ToInt32(reader["Id"]);
                        leituraCriada.SensorId = Convert.ToInt32(reader["SensorId"]);
                        leituraCriada.Valor = Convert.ToSingle(reader["Valor"]);
                        leituraCriada.DataHora = Convert.ToDateTime(reader["DataHora"]);
                    }
                }
            }
            return leituraCriada;
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

        public LeiturasStock ObterLeiturasStock(int stockId)
        {
            var stock = new LeiturasStock
            {
                Temperatura = new List<Temp>(),
                Humidade = new List<Hum>(),
                Luminosidade = new List<Lum>()
            };

            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ObterLeiturasPorStock", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StockId", stockId);

                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string tipo = reader["TipoSensor"].ToString();
                        DateTime dataHora = Convert.ToDateTime(reader["DataHora"]);
                        double valor = Convert.ToDouble(reader["Valor"]);

                        switch (tipo)
                        {
                            case "Temperatura":
                                stock.Temperatura.Add(new Temp
                                {
                                    temperatura = valor,
                                    dataHora = dataHora
                                });
                                break;

                            case "Humidade":
                                stock.Humidade.Add(new Hum
                                {
                                    humidade = valor,
                                    dataHora = dataHora
                                });
                                break;

                            case "Luminosidade":
                                stock.Luminosidade.Add(new Lum
                                {
                                    luminosidade = Convert.ToInt32(valor),
                                    dataHora = dataHora
                                });
                                break;
                        }
                    }
                }
            }

            return stock;
        }
    }
}