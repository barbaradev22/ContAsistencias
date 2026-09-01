using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages.Reportes
{
    public class VistaReporteModel : PageModel
    {
        private readonly dhelperAsistencias _helperAsistencias;
        private readonly dhelperUsuario _helperUsuario;

        public VistaReporteModel(dhelperAsistencias helperAsistencias, dhelperUsuario helperUsuario)
        {
            _helperAsistencias = helperAsistencias;
            _helperUsuario = helperUsuario;
        }

        public List<Asistencia> ListaAtrasos { get; set; } = new();
        public List<Asistencia> ListaSalidasAntictipadas { get; set; } = new();
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            FechaInicio = DateTime.Now.AddDays(-7);
            FechaFin = DateTime.Now;

            // Obtener todos los registros de asistencia
            var todasLasAsistencias = await _helperAsistencias.ObtenerTodasLasAsistencias();

            // Filtrar por rango de fechas
            var asistenciasRango = todasLasAsistencias
                .Where(a => a.Fecha.Date >= FechaInicio.Date && a.Fecha.Date <= FechaFin.Date)
                .ToList();

            // Filtrar atrasos (entrada después de 09:30:00)
            ListaAtrasos = asistenciasRango
                .Where(a => a.Tipo.ToLower() == "entrada" && a.Hora > TimeSpan.Parse("09:30:00"))
                .ToList();

            // Filtrar salidas anticipadas (salida antes de 17:30:00)
            ListaSalidasAntictipadas = asistenciasRango
                .Where(a => a.Tipo.ToLower() == "salida" && a.Hora < TimeSpan.Parse("17:30:00"))
                .ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            string fechaInicioStr = Request.Form["txtFechaInicio"]!;
            string fechaFinStr = Request.Form["txtFechaFin"]!;

            if (DateTime.TryParse(fechaInicioStr, out DateTime fechaInicio) &&
                DateTime.TryParse(fechaFinStr, out DateTime fechaFin))
            {
                FechaInicio = fechaInicio;
                FechaFin = fechaFin;

                // Obtener todos los registros de asistencia
                var todasLasAsistencias = await _helperAsistencias.ObtenerTodasLasAsistencias();

                // Filtrar por rango de fechas
                var asistenciasRango = todasLasAsistencias
                    .Where(a => a.Fecha.Date >= FechaInicio.Date && a.Fecha.Date <= FechaFin.Date)
                    .ToList();

                // Filtrar atrasos
                ListaAtrasos = asistenciasRango
                    .Where(a => a.Tipo.ToLower() == "entrada" && a.Hora > TimeSpan.Parse("09:30:00"))
                    .ToList();

                // Filtrar salidas anticipadas
                ListaSalidasAntictipadas = asistenciasRango
                    .Where(a => a.Tipo.ToLower() == "salida" && a.Hora < TimeSpan.Parse("17:30:00"))
                    .ToList();
            }

            return Page();
        }
    }
}
