using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages.Asistencias
{
    public class CrearAsistenciaModel : PageModel
    {
        private readonly dhelperAsistencias _helperAsistencias;
        private readonly dhelperUsuario _helperUsuario;

        public CrearAsistenciaModel(dhelperAsistencias helperAsistencias, dhelperUsuario helperUsuario)
        {
            _helperAsistencias = helperAsistencias;
            _helperUsuario = helperUsuario;
        }

        public List<Usuario> ListaUsuarios { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionRol")))
                return RedirectToPage("/Login");

            ListaUsuarios = await _helperUsuario.ObtenerUsuarios();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string idUsuarioStr = Request.Form["txtIdUsuario"]!;
            string fechaStr = Request.Form["txtFecha"]!;
            string horaStr = Request.Form["txtHora"]!;
            string tipo = Request.Form["txtTipo"]!;

            if (int.TryParse(idUsuarioStr, out int idUsuario) &&
                DateTime.TryParse(fechaStr, out DateTime fecha) &&
                TimeSpan.TryParse(horaStr, out TimeSpan hora))
            {
                var asistencia = new Asistencia
                {
                    IdUsuario = idUsuario,
                    Fecha = fecha,
                    Hora = hora,
                    Tipo = tipo
                };

                await _helperAsistencias.InsertarAsistencias(asistencia);
                return RedirectToPage("/Asistencias/VistaAsistencia");
            }

            ListaUsuarios = await _helperUsuario.ObtenerUsuarios();
            return Page();
        }
    }
}
