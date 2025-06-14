using Microsoft.AspNetCore.Mvc;
using Project.Services.Users.Interfaces;

namespace Product.Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAppUserService _userService;

        public AuthController(IAppUserService userService)
        {
            _userService = userService;
        }
        [Route("dang-nhap")]
        public IActionResult Index()
        {

            return Redirect("/");
        }
    }
}
