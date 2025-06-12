
using Microsoft.AspNetCore.Mvc;
using Project.Enums;
using Project.Services.Users.Interfaces;
using Project.ViewModel.Usermanagers.Users;
using Project.ViewModels;
using Project.ViewModels.Usermanagers.Users;


namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ResponseHeader("UserType", "IsManage")]
    [ApiController]
    public class AuthsController : ControllerBase
    {
        private readonly IAppUserService _userService;

        public AuthsController(IAppUserService userService)
        {
            _userService = userService;
        }

        [Route("login")]
        [HttpPost]
        public async Task<ActionResult> Authenticate([FromBody] LoginRequest request)
        {
            var res = new ViewDataLoginViewModel();
            try
            {
                var resultToken = await _userService.Authencate(request);
                res.Data = resultToken.Data;
                res.Token = resultToken.Token;
            }
            catch (Exception ex)
            {
                res.Status = false;
                res.Error = (ex.Message);
            }
            return Ok(res);
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromBody] UserCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                request.UserType = UserType.Client;
                request.Workflow = Workflow.PROCESSING;
                request.IsCustomer = true;
                var resultToken = await _userService.Register(request);

            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = (ex.Message);
            }
            return Ok(postResult);
        }



    }
}
