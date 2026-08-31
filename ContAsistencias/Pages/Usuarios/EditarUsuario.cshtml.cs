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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionRol")))
                return RedirectToPage("/Login");

            var usuarios = await _helperUsuario.ObtenerUsuarios();
            UsuarioEditar = usuarios.FirstOrDefault(u => u.IdUsuario == id);

            if (UsuarioEditar == null) return RedirectToPage("/VistaUsuarios");

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int id = int.Parse(Request.Form["txtId"]!);
            string nombre = Request.Form["txtNombre"]!;
            string correo = Request.Form["txtCorreo"]!;
            string password = Request.Form["txtPassword"]!;
            string rol = Request.Form["txtRol"]!;

            var usuario = new Usuario
            {
                IdUsuario = id,
                Nombre = nombre,
                Correo = correo,
                Password = password,
                Rol = rol
            };

            await _helperUsuario.ActualizarUsuario(usuario);
            return RedirectToPage("/VistaUsuarios");
        }
    }
}
