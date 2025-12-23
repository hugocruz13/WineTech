using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace SOAP.Repository
{
    public class AlertasRepository
    {
        private readonly DbConnectionFactory _connectionFactory;

        public AlertasRepository(DbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public Models.Alertas InserirAlerta(Models.Alertas alerta)
        {
            Models.Alertas alertaCriado = new Models.Alertas();

            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("InserirAlerta", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@SensoresId", alerta.SensoresId);
                cmd.Parameters.AddWithValue("@TipoAlerta", alerta.TipoAlerta);
                cmd.Parameters.AddWithValue("@Mensagem", alerta.Mensagem);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        alertaCriado.Id = Convert.ToInt32(reader["Id"]);
                        alertaCriado.SensoresId = Convert.ToInt32(reader["SensoresId"]);
                        alertaCriado.TipoAlerta = reader["TipoAlerta"].ToString();
                        alertaCriado.Mensagem = reader["Mensagem"].ToString();
                        alertaCriado.DataHora = Convert.ToDateTime(reader["DataHora"]);
                        alertaCriado.Resolvido = Convert.ToBoolean(reader["Resolvido"]);
                    }
                }
            }
            return alertaCriado;
        }
        public List<Models.Alertas> ObterAlertasPorSensor(int sensorId)
        {
            List<Models.Alertas> lista = new List<Models.Alertas>();

            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ObterAlertasPorSensor", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SensoresId", sensorId);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Models.Alertas
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            SensoresId = Convert.ToInt32(reader["SensoresId"]),
                            TipoAlerta = reader["TipoAlerta"].ToString(),
                            Mensagem = reader["Mensagem"].ToString(),
                            DataHora = Convert.ToDateTime(reader["DataHora"]),
                            Resolvido = Convert.ToBoolean(reader["Resolvido"])
                        });
                    }
                }
            }
            return lista;
        }

        public List<SOAP.Models.AlertaComSensor> GetAllAlertas()
        {
            var lista = new List<SOAP.Models.AlertaComSensor>();

            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("GetAllAlertas", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new SOAP.Models.AlertaComSensor
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            TipoAlerta = reader["TipoAlerta"].ToString(),
                            Mensagem = reader["Mensagem"].ToString(),
                            DataHora = Convert.ToDateTime(reader["DataHora"]),
                            Resolvido = Convert.ToBoolean(reader["Resolvido"]),
                            SensoresId = Convert.ToInt32(reader["SensoresId"]),
                            IdentificadorHardware = reader["IdentificadorHardware"].ToString(),
                            TipoSensor = reader["TipoSensor"].ToString(),
                            AdegaId = Convert.ToInt32(reader["AdegaId"])
                        });
                    }
                }
            }

            return lista;
        }

        public bool ResolverAlerta(int alertaId)
        {
            using (var conn = _connectionFactory.GetConnection())
            using (var cmd = new SqlCommand("ResolverAlerta", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AlertaId", alertaId);

                conn.Open();
                var affected = cmd.ExecuteNonQuery();
                return affected > 0;
            }
        }

    }
}