using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages.Asistencias
{
    public class VistaAsistenciaModel : PageModel
    {
        private readonly dhelperAsistencias _helperAsistencia;

        public VistaAsistenciaModel(dhelperAsistencias helperAsistencia)
        {
            _helperAsistencia = helperAsistencia;
        }

        public List<Asistencia> ListaAsistencias { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            ListaAsistencias = await _helperAsistencia.ObtenerTodasLasAsistencias();
            return Page();
        }

        public async Task<IActionResult> OnGetEliminarAsync(int id)
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            await _helperAsistencia.EliminarAsistencia(id);
            return RedirectToPage("/Asistencias/VistaAsistencia");
        }
    }
}
