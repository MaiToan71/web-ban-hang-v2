using Microsoft.AspNetCore.Mvc;

namespace Product.Frontend.Controllers
{
    public class CartController : Controller
    {
        [Route("gio-hang")]
        public async Task<IActionResult> Index()
        {
            IConfigurationRoot configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ViewBag.Domain = configurationBuilder["Domain:Api"];

            return View();
        }
    }
}
