using ContAsistencias.data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            if (!HttpContext.IsAuthenticated())
            {
                return RedirectToPage("/Login");
            }

            if (HttpContext.IsEmpleado())
            {
                return RedirectToPage("/Empleado/VistaEmpleado");
            }

            if (!HttpContext.IsAdmin())
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Login");
            }

            return Page();
        }
    }
}
