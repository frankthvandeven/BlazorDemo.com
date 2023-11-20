using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Kenova.KenSys.Pages
{
    public class Page1Model : PageModel
    {
        public string Name;

        public void OnGet()
        {
            Name = "This is variable Name";
        }
    }
}
