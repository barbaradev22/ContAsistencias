using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages.Empleado
{
    public class VistaEmpleadoModel : PageModel
    {
        private readonly dhelperAsistencias _helperAsistencias;

        public VistaEmpleadoModel(dhelperAsistencias helperAsistencias)
        {
            _helperAsistencias = helperAsistencias;
        }

        public string? NombreEmpleado { get; set; }
        public List<Asistencia> MisAsistencias { get; set; } = new();
        public string? Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (!HttpContext.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (HttpContext.GetUsuarioId() is not int idUsuario)
            {
                return RedirectToPage("/Login");
            }

            await CargarDatosAsync(idUsuario);
            return Page();
        }

        public async Task<IActionResult> OnPostRegistrarAsync()
        {
            if (!HttpContext.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (HttpContext.GetUsuarioId() is not int idUsuario)
            {
                return RedirectToPage("/Login");
            }

            string tipo = Request.Form["txtTipo"]!.ToString().Trim().ToLowerInvariant();
            if (tipo != "entrada" && tipo != "salida")
            {
                Mensaje = "Tipo de asistencia inválido.";
                await CargarDatosAsync(idUsuario);
                return Page();
            }

            var registros = await _helperAsistencias.ObtenerAsistenciasPorUsuario(idUsuario);
            DateTime fechaHoy = DateTime.Today;
            var registrosHoy = registros.Where(a => a.Fecha.Date == fechaHoy).ToList();

            if (registrosHoy.Any(a => a.Tipo.Equals(tipo, StringComparison.OrdinalIgnoreCase)))
            {
                Mensaje = $"Ya registraste la {tipo} de hoy.";
                await CargarDatosAsync(idUsuario);
                return Page();
            }

            if (tipo == "salida" && !registrosHoy.Any(a => a.Tipo.Equals("entrada", StringComparison.OrdinalIgnoreCase)))
            {
                Mensaje = "Primero debes registrar la entrada de hoy.";
                await CargarDatosAsync(idUsuario);
                return Page();
            }

            await _helperAsistencias.InsertarAsistencias(new Asistencia
            {
                IdUsuario = idUsuario,
                Fecha = fechaHoy,
                Hora = DateTime.Now.TimeOfDay,
                Tipo = tipo
            });

            Mensaje = $"Se registró la {tipo} correctamente.";
            await CargarDatosAsync(idUsuario);
            return Page();
        }

        private async Task CargarDatosAsync(int idUsuario)
        {
            NombreEmpleado = HttpContext.GetNombre();
            MisAsistencias = await _helperAsistencias.ObtenerAsistenciasPorUsuario(idUsuario);
        }
    }
}
