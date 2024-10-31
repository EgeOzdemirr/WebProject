using Microsoft.AspNetCore.Mvc;

namespace WebProject.WebUI.Controllers
{
	public class LoginController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
