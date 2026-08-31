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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionRol")))
                return RedirectToPage("/Login");

            ListaAsistencias = await _helperAsistencia.ObtenerTodasLasAsistencias();
            return Page();
        }

        public async Task<IActionResult> OnGetEliminarAsync(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionRol")))
                return RedirectToPage("/Login");

            await _helperAsistencia.EliminarAsistencia(id);
            return RedirectToPage("/Asistencias/VistaAsistencia");
        }
    }
}
