using ContAsistencias.modelo;
using System.Data.SqlClient;

namespace ContAsistencias.data
{
    public class dhelperUsuario
    {
        private readonly string _connectionString;

        public dhelperUsuario(IConfiguration configuracion)
        {
            string conexionBase = configuracion.GetConnectionString("DefaultConnection") ?? "";
            _connectionString = conexionBase.Replace("|DataDirectory|", AppDomain.CurrentDomain.GetData("DataDirectory")?.ToString() ?? Directory.GetCurrentDirectory());
        }

        public async Task InsertarUsuarios(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "INSERT INTO usuario (nombre, correo, password, rol) VALUES (@nombre, @correo, @password, @rol)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@correo", usuario.Correo);
                    command.Parameters.AddWithValue("@password", usuario.Password);
                    command.Parameters.AddWithValue("@rol", usuario.Rol);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Usuario>> ObtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM usuario WHERE activo = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Usuario usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Rol = reader.GetString(reader.GetOrdinal("rol")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
                            };
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }

        public async Task<List<Usuario>> ObtenerTodosLosUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM usuario ORDER BY activo DESC, nombre ASC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Usuario usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Rol = reader.GetString(reader.GetOrdinal("rol")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
                            };
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }

        public async Task EliminarUsuario(int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (var tx = connection.BeginTransaction())
                {
                    try
                    {
                        using (SqlCommand command = connection.CreateCommand())
                        {
                            command.Transaction = tx;

                            // 1) Eliminar asistencias del usuario
                            command.CommandText = "DELETE FROM asistencias WHERE id_usuario = @id_usuario";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id_usuario", idUsuario);
                            await command.ExecuteNonQueryAsync();

                            // 2) Marcar usuario como inactivo
                            command.CommandText = "UPDATE usuario SET activo = 0 WHERE id_usuario = @id_usuario";
                            command.Parameters.Clear();
                            command.Parameters.AddWithValue("@id_usuario", idUsuario);
                            await command.ExecuteNonQueryAsync();

                            tx.Commit();
                        }
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task ActualizarUsuario(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE usuario SET nombre = @nombre, correo = @correo, password = @password, rol = @rol WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", usuario.IdUsuario);
                    command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                    command.Parameters.AddWithValue("@correo", usuario.Correo);
                    command.Parameters.AddWithValue("@password", usuario.Password);
                    command.Parameters.AddWithValue("@rol", usuario.Rol);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Usuario?> ValidarUsuarioAsync(string correo, string password)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT id_usuario, nombre, correo, password, rol, activo FROM usuario WHERE correo = @correo AND password = @password AND activo = 1";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@correo", correo);
                    command.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Rol = reader.GetString(reader.GetOrdinal("rol")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
                            };
                        }
                    }
                }
            }

            return null;
        }

        public async Task<string?> ValidarUsuarioYObtenerRolAsync(string correo, string password)
        {
            Usuario? usuario = await ValidarUsuarioAsync(correo, password);
            return usuario?.Rol;
        }

        public async Task ReactivarUsuario(int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE usuario SET activo = 1 WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", idUsuario);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<List<Usuario>> ObtenerUsuariosInactivos()
        {
            List<Usuario> usuarios = new List<Usuario>();
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM usuario WHERE activo = 0";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            Usuario usuario = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Rol = reader.GetString(reader.GetOrdinal("rol")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
                            };
                            usuarios.Add(usuario);
                        }
                    }
                }
            }
            return usuarios;
        }

        public async Task<Usuario?> ObtenerUsuarioPorId(int idUsuario)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "SELECT * FROM usuario WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", idUsuario);

                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                Correo = reader.GetString(reader.GetOrdinal("correo")),
                                Password = reader.GetString(reader.GetOrdinal("password")),
                                Rol = reader.GetString(reader.GetOrdinal("rol")),
                                Activo = reader.GetBoolean(reader.GetOrdinal("activo"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public async Task CambiarEstadoActivo(int idUsuario, int activo)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = "UPDATE usuario SET activo = @activo WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", idUsuario);
                    command.Parameters.AddWithValue("@activo", activo);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}