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
                string query = "INSERT INTO asistencias (id_usuario, fecha_asistencia, hora_asistencia, tipo_asistencia) VALUES (@id_usuario, @fecha_asistencia, @hora_asistencia, @tipo_asistencia)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", asistencia.IdUsuario);
                    command.Parameters.AddWithValue("@fecha_asistencia", asistencia.Fecha);
                    command.Parameters.AddWithValue("@hora_asistencia", asistencia.Hora);
                    command.Parameters.AddWithValue("@tipo_asistencia", asistencia.Tipo);
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
                string query = "SELECT * FROM asistencias WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", idUsuario);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Asistencia asistencia = new Asistencia
                            {
                                IdAsistencia = reader.GetInt32(reader.GetOrdinal("id_asistencia")),
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha_asistencia")),
                                Hora = reader.GetTimeSpan(reader.GetOrdinal("hora_asistencia")),
                                Tipo = reader.GetString(reader.GetOrdinal("tipo_asistencia"))
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
                string query = "DELETE FROM asistencias WHERE id_asistencia = @id_asistencia";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_asistencia", idAsistencia);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task actualizarAsistencia(Asistencia asistencia)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE asistencias SET fecha_asistencia = @fecha_asistencia, hora_asistencia = @hora_asistencia, tipo_asistencia = @tipo_asistencia WHERE id_asistencia = @id_asistencia";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_asistencia", asistencia.IdAsistencia);
                    command.Parameters.AddWithValue("@fecha_asistencia", asistencia.Fecha);
                    command.Parameters.AddWithValue("@hora_asistencia", asistencia.Hora);
                    command.Parameters.AddWithValue("@tipo_asistencia", asistencia.Tipo);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Asistencia>> ObtenerAtrasosAsync()
        {
            List<Asistencia> asistencias = new List<Asistencia>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM asistencias WHERE tipo_asistencia = 'entrada' AND hora_asistencia > '09:30:00'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Asistencia asistencia = new Asistencia
                            {
                                IdAsistencia = reader.GetInt32(reader.GetOrdinal("id_asistencia")),
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha_asistencia")),
                                Hora = reader.GetTimeSpan(reader.GetOrdinal("hora_asistencia")),
                                Tipo = reader.GetString(reader.GetOrdinal("tipo_asistencia"))
                            };
                            asistencias.Add(asistencia);
                        }
                    }
                }
            }
            return asistencias;
        }

        public async Task<List<Asistencia>> ObtenerSalidasAnticipadasAsync()
        {
            List<Asistencia> asistencias = new List<Asistencia>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM asistencias WHERE tipo_asistencia = 'salida' AND hora_asistencia < '17:30:00'";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Asistencia asistencia = new Asistencia
                            {
                                IdAsistencia = reader.GetInt32(reader.GetOrdinal("id_asistencia")),
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("fecha_asistencia")),
                                Hora = reader.GetTimeSpan(reader.GetOrdinal("hora_asistencia")),
                                Tipo = reader.GetString(reader.GetOrdinal("tipo_asistencia"))
                            };
                            asistencias.Add(asistencia);
                        }
                    }
                }
            }
            return asistencias;
        }

        public async Task<List<int>> ObtenerIdsUsuariosInasistentesAsync(DateTime fechaBusqueda)
        {
            List<int> idsConAsistencia = new List<int>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT DISTINCT id_usuario FROM asistencias WHERE fecha_asistencia = @fecha";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fecha", fechaBusqueda.Date);
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            idsConAsistencia.Add(reader.GetInt32(0));
                        }
                    }
                }
            }
            return idsConAsistencia;
        }
    }
}