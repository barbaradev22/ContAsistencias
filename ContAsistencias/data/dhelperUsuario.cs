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
                string query = "SELECT * FROM usuario";
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
                                Rol = reader.GetString(reader.GetOrdinal("rol"))
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
                string query = "DELETE FROM usuario WHERE id_usuario = @id_usuario";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_usuario", idUsuario);
                    await command.ExecuteNonQueryAsync();
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
    }
}