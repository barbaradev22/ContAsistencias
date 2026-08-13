namespace ContAsistencias.modelo
{
    public class Usuario
    {
        private int idUsuario;
        private string nombre;
        private string correo;
        private string contraseña;
        private string direccion;
        private string telefono;
        private string rut;

        public Usuario(int idUsuario, string nombre, string correo, string contraseña, string direccion, string telefono, string rut)
        {
            this.idUsuario = idUsuario;
            this.nombre = nombre;
            this.correo = correo;
            this.contraseña = contraseña;
            this.direccion = direccion;
            this.telefono = telefono;
            this.rut = rut;
        }

        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public string Nombre { get => nombre; set => nombre = value; }
        public string Correo { get => correo; set => correo = value; }
        public string Contraseña { get => contraseña; set => contraseña = value; }
        public string Direccion { get => direccion; set => direccion = value; }
        public string Telefono { get => telefono; set => telefono = value; }
        public string Rut { get => rut; set => rut = value; }
    }
}
