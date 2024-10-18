using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Controllers
{
    public class UILayoutController : Controller
    {
        public IActionResult _UILayout()
        {
            return View();
        }
    }
}
