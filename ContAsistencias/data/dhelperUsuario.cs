namespace ContAsistencias.modelo;
using System.Data.SqlClient;

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
            string query = "INSERT INTO Usuarios (nombre, correo, password, rol) VALUES (@nombre, @correo, @password, @rol)";
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
            string query = "SELECT * FROM Usuarios";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        Usuario usuario = new Usuario
                        {
                            IdUsuario = reader.GetInt32(reader.GetOrdinal("idUsuario")),
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
            string query = "DELETE FROM Usuarios WHERE idUsuario = @idUsuario";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idUsuario", idUsuario);
                await command.ExecuteNonQueryAsync();
            }
        }
    }

    public async Task ActualizarUsuario(Usuario usuario)
    {
        using (SqlConnection connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            string query = "UPDATE Usuarios SET nombre = @nombre, correo = @correo, password = @password, rol = @rol WHERE idUsuario = @idUsuario";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@idUsuario", usuario.IdUsuario);
                command.Parameters.AddWithValue("@nombre", usuario.Nombre);
                command.Parameters.AddWithValue("@correo", usuario.Correo);
                command.Parameters.AddWithValue("@password", usuario.Password);
                command.Parameters.AddWithValue("@rol", usuario.Rol);
                await command.ExecuteNonQueryAsync();
            }
        }
    }






}

