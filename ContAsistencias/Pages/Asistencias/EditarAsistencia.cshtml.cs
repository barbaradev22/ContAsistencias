using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages.Asistencias
{
    public class EditarAsistenciaModel : PageModel
    {
        private readonly dhelperAsistencias _helperAsistencias;

        public EditarAsistenciaModel(dhelperAsistencias helperAsistencias)
        {
            _helperAsistencias = helperAsistencias;
        }

        public Asistencia? AsistenciaEditar { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            var asistencias = await _helperAsistencias.ObtenerTodasLasAsistencias();
            AsistenciaEditar = asistencias.FirstOrDefault(a => a.IdAsistencia == id);

            if (AsistenciaEditar == null) return RedirectToPage("/Asistencias/VistaAsistencia");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            int id = int.Parse(Request.Form["txtId"]!);
            int idUsuario = int.Parse(Request.Form["txtIdUsuario"]!);
            string fechaStr = Request.Form["txtFecha"]!;
            string horaStr = Request.Form["txtHora"]!;
            string tipo = Request.Form["txtTipo"]!;

            if (DateTime.TryParse(fechaStr, out DateTime fecha) &&
                TimeSpan.TryParse(horaStr, out TimeSpan hora))
            {
                var asistencia = new Asistencia
                {
                    IdAsistencia = id,
                    IdUsuario = idUsuario,
                    Fecha = fecha,
                    Hora = hora,
                    Tipo = tipo.Trim().ToLowerInvariant()
                };

                await _helperAsistencias.actualizarAsistencia(asistencia);
                return RedirectToPage("/Asistencias/VistaAsistencia");
            }

            return Page();
        }
    }
}
