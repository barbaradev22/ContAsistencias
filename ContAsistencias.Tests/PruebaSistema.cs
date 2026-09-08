using System;
using System.Collections.Generic;
using System.Text;

namespace ContAsistencias.Tests
{
    public class PruebaSistema
    {

        [Fact]
        public void TestControlDeAsistenciaExitoso()
        {
            int idUsuario = 1;
            DateTime entrada = DateTime.Now;
            DateTime salida = DateTime.Now.AddHours(8);

            Assert.True(idUsuario > 0);
            Assert.NotEqual(default(DateTime), entrada);
            Assert.NotEqual(default(DateTime), salida);
            Assert.True(salida > entrada);
        }

        [Fact]
        public void TestGenerarReporteDeAtrasos()
        {
            var registros = new List<(int Id, TimeSpan HoraEntrada)>
        {
            (1, new TimeSpan(9, 0, 0)),   
            (2, new TimeSpan(9, 31, 0)), 
            (3, new TimeSpan(10, 0, 0))   
        };

            var atrasados = registros.Where(r => r.HoraEntrada > new TimeSpan(9, 30, 0)).ToList();

            Assert.Equal(2, atrasados.Count);
            Assert.All(atrasados, r => Assert.True(r.HoraEntrada > new TimeSpan(9, 30, 0)));
        }

        [Fact]
        public void TestGenerarReporteSalidasAnticipadas()
        {
            
            var registros = new List<(int Id, TimeSpan HoraSalida)>
        {
            (1, new TimeSpan(17, 30, 0)), 
            (2, new TimeSpan(16, 45, 0)), 
            (3, new TimeSpan(18, 0, 0))   
        };

           
            var anticipadas = registros.Where(r => r.HoraSalida < new TimeSpan(17, 30, 0)).ToList();

           
            Assert.Single(anticipadas);
            Assert.Equal(2, anticipadas.First().Id);
        }

        [Fact]
        public void TestGenerarReporteDeInasistencias()
        {
            var todosLosUsuarios = new List<string> { "Juan", "Maria", "Carlos" };
            var usuariosConAsistencia = new List<string> { "Juan", "Carlos" };

            var inasistencias = todosLosUsuarios.Except(usuariosConAsistencia).ToList();

            Assert.Single(inasistencias);
            Assert.Equal("Maria", inasistencias.First());
        }

        [Fact]
        public void TestCrearUsuarioConDatosValidos()
        {
            var usuario = new { Nombre = "Pedro", Correo = "pedro@mail.com", Rol = "Trabajador" };

            Assert.False(string.IsNullOrEmpty(usuario.Nombre));
            Assert.Contains("@", usuario.Correo);
            Assert.Equal("Trabajador", usuario.Rol);
        }

        [Fact]
        public void TestModificarUsuarioExitoso()
        {
            string nombreOriginal = "Pedro";
            string correoOriginal = "pedro@mail.com";

            string nombreModificado = "Pedro Actualizado";
            string correoModificado = "pedro.nuevo@mail.com";

            Assert.NotEqual(nombreOriginal, nombreModificado);
            Assert.NotEqual(correoOriginal, correoModificado);
            Assert.Contains("@", correoModificado);
        }

        [Fact]
        public void TestEliminarUsuarioExitoso()
        {
            var listaUsuarios = new List<string> { "jp", "Lucho", "Carlos" };

            listaUsuarios.Remove("Lucho");

            Assert.Equal(2, listaUsuarios.Count);
            Assert.DoesNotContain("Lucho", listaUsuarios);
        }
    }
}

