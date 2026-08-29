namespace ContAsistencias.modelo
{
    public class Asistencia
    {
        private int idAsistencia;
        private int idUsuario;
        private System.DateTime fecha;
        private System.TimeSpan hora;
        private string tipo;

        public Asistencia()
        {
        }

        public Asistencia(int idAsistencia, int idUsuario, System.DateTime fecha, System.TimeSpan hora, string tipo)
        {
            this.idAsistencia = idAsistencia;
            this.idUsuario = idUsuario;
            this.fecha = fecha;
            this.hora = hora;
            this.tipo = tipo;
        }

        public int IdAsistencia { get => idAsistencia; set => idAsistencia = value; }
        public int IdUsuario { get => idUsuario; set => idUsuario = value; }
        public System.DateTime Fecha { get => fecha; set => fecha = value; }
        public System.TimeSpan Hora { get => hora; set => hora = value; }
        public string Tipo { get => tipo; set => tipo = value; }
    }
}
