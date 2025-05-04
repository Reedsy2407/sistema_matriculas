using Microsoft.AspNetCore.Mvc;

namespace Matriculas.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
