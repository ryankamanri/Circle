using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace dotnet.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet()
        {
            return Page();
        }
    }
}