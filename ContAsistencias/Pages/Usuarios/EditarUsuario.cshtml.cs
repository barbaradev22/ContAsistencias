using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages.Usuarios
{
    public class EditarUsuarioModel : PageModel
    {
        private readonly dhelperUsuario _helperUsuario;

        public EditarUsuarioModel(dhelperUsuario helperUsuario)
        {
            _helperUsuario = helperUsuario;
        }

        public Usuario? UsuarioEditar { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            UsuarioEditar = await _helperUsuario.ObtenerUsuarioPorId(id);

            if (UsuarioEditar == null) return RedirectToPage("/Usuarios/VistaUsuario");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            int id = int.Parse(Request.Form["txtId"]!);
            string nombre = Request.Form["txtNombre"]!;
            string correo = Request.Form["txtCorreo"]!;
            string password = Request.Form["txtPassword"]!;
            string rol = Request.Form["txtRol"]!.ToString().Trim().ToLowerInvariant();

            var usuario = new Usuario
            {
                IdUsuario = id,
                Nombre = nombre,
                Correo = correo,
                Password = password,
                Rol = rol
            };

            await _helperUsuario.ActualizarUsuario(usuario);
            return RedirectToPage("/Usuarios/VistaUsuario");
        }

        public async Task<IActionResult> OnPostCambiarEstadoAsync(int id)
        {
            if (!HttpContext.IsAuthenticated())
                return RedirectToPage("/Login");

            if (!HttpContext.IsAdmin())
                return RedirectToPage("/Empleado/VistaEmpleado");

            var usuario = await _helperUsuario.ObtenerUsuarioPorId(id);
            if (usuario == null)
                return RedirectToPage("/Usuarios/VistaUsuario");

            // Cambiar estado: si estaba activo pasa a inactivo y viceversa
            int nuevoEstado = usuario.Activo ? 0 : 1;
            await _helperUsuario.CambiarEstadoActivo(id, nuevoEstado);

            return RedirectToPage("/Usuarios/VistaUsuario");
        }
    }
}
