using Microsoft.AspNetCore.Mvc;

namespace Product.Frontend.Controllers
{

    public class GuaranteeController : Controller
    {
        [Route("chinh-sach-bao-hanh")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
