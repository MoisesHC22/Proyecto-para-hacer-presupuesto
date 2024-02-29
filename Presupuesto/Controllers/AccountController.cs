using Microsoft.AspNetCore.Mvc;

namespace Presupuesto.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult SingIn()
        {
            return View();
        }
    }
}
