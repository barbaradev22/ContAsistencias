using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ContAsistencias.Pages
{
    public class TimeOutModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? Message { get; set; }

        public void OnGet()
        {
            // Message se recibe por querystring
        }

        public IActionResult OnGetClear()
        {
            HttpContext.Session.Clear();
            return new JsonResult(true);
        }
    }
}
