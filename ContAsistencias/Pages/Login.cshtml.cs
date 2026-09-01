using ContAsistencias.data;
using ContAsistencias.modelo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages
{
    public class loginModel : PageModel
    {
        private readonly dhelperUsuario _dhelperUsuario;

        public loginModel(dhelperUsuario dhelperUsuario)
        {
            _dhelperUsuario = dhelperUsuario;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            string correoDigitado = Request.Form["txtCorreo"]!;
            string passwordDigitada = Request.Form["txtContrasena"]!;

            Usuario? usuario = await _dhelperUsuario.ValidarUsuarioAsync(correoDigitado, passwordDigitada);

            if (usuario != null)
            {
                string rolNormalizado = usuario.Rol.Trim().ToLowerInvariant();

                HttpContext.Session.SetInt32(SessionAccess.UsuarioIdKey, usuario.IdUsuario);
                HttpContext.Session.SetString(SessionAccess.NombreKey, usuario.Nombre);
                HttpContext.Session.SetString(SessionAccess.CorreoKey, usuario.Correo);
                HttpContext.Session.SetString(SessionAccess.RoleKey, rolNormalizado);

                if (rolNormalizado == "admin")
                {
                    return RedirectToPage("/Index");
                }

                if (rolNormalizado == "empleado")
                {
                    return RedirectToPage("/Empleado/VistaEmpleado");
                }

                HttpContext.Session.Clear();
            }

            ModelState.AddModelError(string.Empty, "El correo o la contraseña son incorrectos.");
            return Page();
        }

        public IActionResult OnGetCerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}