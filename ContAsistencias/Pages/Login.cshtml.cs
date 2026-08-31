using ContAsistencias.data;
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

            string? rolEncontrado = await _dhelperUsuario.ValidarUsuarioYObtenerRolAsync(correoDigitado, passwordDigitada);

            if (rolEncontrado != null)
            {
                // Guardar datos en la sesión como en tu proyecto anterior
                HttpContext.Session.SetString("SessionRol", rolEncontrado);
                HttpContext.Session.SetString("SessionCorreo", correoDigitado);

                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "El correo o la contraseña son incorrectos.");
                return Page();
            }
        }

        public IActionResult OnGetCerrarSesion()
        {
            HttpContext.Session.Clear();
            return RedirectToPage("/Login");
        }
    }
}