using ContAsistencias.modelo;
using System.Data.SqlClient;

namespace ContAsistencias.data
{
    public class dhelperAsistencias
    {
        private readonly string _connectionString;

        public dhelperAsistencias(IConfiguration configuracion)
        {
            string conexionBase = configuracion.GetConnectionString("DefaultConnection") ?? "";
            _connectionString = conexionBase.Replace("|DataDirectory|", AppDomain.CurrentDomain.GetData("DataDirectory")?.ToString() ?? Directory.GetCurrentDirectory());
        }

        public async Task InsertarAsistencias(Asistencia asistencia)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO Asistencias (idUsuario, fecha, hora, tipo) VALUES (@idUsuario, @fecha, @hora, @tipo)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", asistencia.IdUsuario);
                    command.Parameters.AddWithValue("@fecha", asistencia.Fecha);
                    command.Parameters.AddWithValue("@hora", asistencia.Hora);
                    command.Parameters.AddWithValue("@tipo", asistencia.Tipo);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Asistencia>> ObtenerAsistenciasPorUsuario(int idUsuario)
        {
            List<Asistencia> asistencias = new List<Asistencia>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM Asistencias WHERE idUsuario = @idUsuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idUsuario", idUsuario);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Asistencia asistencia = new Asistencia
                            {
                                IdAsistencia = reader.GetInt32(reader.GetOrdinal("idAsistencia")),
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("idUsuario")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                Hora = reader.GetTimeSpan(reader.GetOrdinal("hora")),
                                Tipo = reader.GetString(reader.GetOrdinal("tipo"))
                            };
                            asistencias.Add(asistencia);
                        }
                    }
                }
            }
            return asistencias;
        }

        public async Task EliminarAsistencia(int idAsistencia)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "DELETE FROM Asistencias WHERE idAsistencia = @idAsistencia";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idAsistencia", idAsistencia);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task actualizarAsistencia(Asistencia asistencia)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE Asistencias SET fecha = @fecha, hora = @hora, tipo = @tipo WHERE idAsistencia = @idAsistencia";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idAsistencia", asistencia.IdAsistencia);
                    command.Parameters.AddWithValue("@fecha", asistencia.Fecha);
                    command.Parameters.AddWithValue("@hora", asistencia.Hora);
                    command.Parameters.AddWithValue("@tipo", asistencia.Tipo);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}