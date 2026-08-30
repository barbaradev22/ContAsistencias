namespace ContAsistencias.modelo
{
    public class Usuario
    {
        private int idUsuario;
        private string nombre;
        private string correo;
        private string password;
        private string rol;

        public Usuario()
        {
        }

        public Usuario(int idUsuario, string nombre, string correo, string password, string rol)
        {
            this.idUsuario = idUsuario;
            this.nombre = nombre;
            this.correo = correo;
            this.password = password;
            this.rol = rol;
        }

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Password { get => password; set => password = value; }
        public string Rol { get => rol; set => rol = value; }
    }

}
