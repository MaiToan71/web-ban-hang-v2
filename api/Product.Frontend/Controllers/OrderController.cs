using Microsoft.AspNetCore.Mvc;

namespace Product.Frontend.Controllers
{
    [Route("thanh-toan")]
    public class OrderController : Controller
    {


        public IActionResult Index()
        {
            IConfigurationRoot configurationBuilder = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            ViewBag.Domain = configurationBuilder["Domain:Api"];
            return View();
        }
    }
}
