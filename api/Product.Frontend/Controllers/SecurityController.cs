using Microsoft.AspNetCore.Mvc;

namespace Product.Frontend.Controllers
{
    public class SecurityController : Controller
    {
        [Route("chinh-sach-bao-mat")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
