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
    }
}