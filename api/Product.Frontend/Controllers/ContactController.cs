using Microsoft.AspNetCore.Mvc;

namespace Product.Frontend.Controllers
{
    [Route("lien-he")]
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
