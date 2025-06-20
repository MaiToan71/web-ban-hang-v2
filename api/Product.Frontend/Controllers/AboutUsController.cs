using Microsoft.AspNetCore.Mvc;

namespace Product.Frontend.Controllers
{
    [Route("ve-chung-toi")]
    public class AboutUsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
