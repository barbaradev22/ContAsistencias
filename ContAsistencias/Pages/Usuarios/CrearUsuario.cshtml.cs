using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages.Usuarios
{
    public class CrearUsuarioModel : PageModel
    {
        private readonly dhelperUsuario _helperUsuario;

        public CrearUsuarioModel(dhelperUsuario helperUsuario)
        {
            _helperUsuario = helperUsuario;
        }

        public IActionResult OnGet()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            string nombre = Request.Form["txtNombre"]!;
            string correo = Request.Form["txtCorreo"]!;
            string password = Request.Form["txtPassword"]!;
            string rol = Request.Form["txtRol"]!.ToString().Trim().ToLowerInvariant();

            var usuario = new Usuario
            {
                Nombre = nombre,
                Correo = correo,
                Password = password,
                Rol = rol
            };

            await _helperUsuario.InsertarUsuarios(usuario);
            return RedirectToPage("/Usuarios/VistaUsuario");
        }
    }
}
