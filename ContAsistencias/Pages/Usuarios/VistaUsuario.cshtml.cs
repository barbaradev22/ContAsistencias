using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages.Usuarios
{
    public class VistaUsuarioModel : PageModel
    {
        private readonly dhelperUsuario _helperUsuario;

        public VistaUsuarioModel(dhelperUsuario helperUsuario)
        {
            _helperUsuario = helperUsuario;
        }

        public List<Usuario> ListaUsuarios { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            ListaUsuarios = await _helperUsuario.ObtenerUsuarios();
            return Page();
        }

        public async Task<IActionResult> OnGetEliminarAsync(int id)
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            await _helperUsuario.EliminarUsuario(id);
            return RedirectToPage("/Usuarios/VistaUsuario");
        }
    }
}
