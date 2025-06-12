using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Project.Caching.Interfaces;
using Project.Services.Users.Interfaces;
using Project.ViewModel.Usermanagers.Users;
using Project.ViewModels;

namespace Project.API.Controllers
{

    public class UsersController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IAppUserService _IAppUserService;
        public UsersController(IAppUserService IAppUserService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IAppUserService = IAppUserService;
        }
        [Route("TimeShiftTags")]
        [HttpPost]
        public async Task<ActionResult> GetListTimeShiftTags([FromBody] GetUserPagingRequest query)
        {
            var postResult = new PostResult();
            try
            {
                query.UserId = CurrentUserViewModel.UserId;
                var res = new ViewDataLoginViewModel();

                var data = await _IAppUserService.GetListTimeShiftTags(query);
                return Ok(data);
            }
            catch (Exception ex)
            {
                postResult.Data = ex.Message;
                postResult.Status = false;
            }
            return Ok(postResult);
        }

        [Route("StatisticalByDay")]
        [HttpPost]
        public async Task<ActionResult> GetStatisticalByDay([FromQuery] GetUserPagingRequest query)
        {
            var postResult = new PostResult();
            try
            {
                query.UserId = CurrentUserViewModel.UserId;
                var res = new ViewDataLoginViewModel();

                var data = await _IAppUserService.GetStatisticalByDay(query);
                postResult.Data = data;

            }
            catch (Exception ex)
            {
                postResult.Data = ex.Message;
                postResult.Status = false;
            }
            return Ok(postResult);
        }

        [Route("statistical/TimeShiftType")]
        [HttpPost]
        public async Task<ActionResult> GetStatisticalTimeShiftType([FromQuery] GetUserPagingRequest query)
        {
            var postResult = new PostResult();
            try
            {
                query.UserId = CurrentUserViewModel.UserId;
                var res = new ViewDataLoginViewModel();

                var data = await _IAppUserService.GetStatisticalTimeShiftType(query);
                postResult.Data = data;

            }
            catch (Exception ex)
            {
                postResult.Data = ex.Message;
                postResult.Status = false;
            }
            return Ok(postResult);
        }
        [Route("statistical/job")]
        [HttpPost]
        public async Task<ActionResult> GetStatisticalJob([FromQuery] GetUserPagingRequest query)
        {
            var postResult = new PostResult();
            try
            {
                query.UserId = CurrentUserViewModel.UserId;
                var res = new ViewDataLoginViewModel();

                var data = await _IAppUserService.GetStatisticalJob(query);
                postResult.Data = data;

            }
            catch (Exception ex)
            {
                postResult.Data = ex.Message;
                postResult.Status = false;
            }
            return Ok(postResult);
        }

        [Route("info")]
        [HttpGet]
        public async Task<ActionResult> GetUserInfo()
        {
            var postResult = new PostResult();
            try
            {
                var userId = Guid.Parse(HttpContext.User.Identities.FirstOrDefault().Name);
                var res = new ViewDataLoginViewModel();
                if (!ModelState.IsValid)
                {
                    res.Status = false;
                    res.Error = ModelState.IsValid;
                }
                var user = await _IAppUserService.GetUserInfo(userId);
                postResult.Data = user;
            }
            catch (Exception ex)
            {
                postResult.Data = ex.Message;
                postResult.Status = false;
            }
            return Ok(postResult);
        }


        [Route("client/update")]
        [HttpPut]
        public async Task<ActionResult> ClientUpdate([FromBody] ClientUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                var userId = Guid.Parse(HttpContext.User.Identities.FirstOrDefault().Name);
                request.UserId = userId;
                bool item = await _IAppUserService.ClientUpdate(request);


            }
            catch (Exception ex)
            {
                postResult.Data = ex.Message;
                postResult.Status = false;
            }
            return Ok(postResult);
        }

        [Route("client/changepassword")]
        [HttpPatch]
        public async Task<ActionResult> ClientChangePassword([FromForm] ClientUpdatePasswordRequest request)
        {

            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                var userId = Guid.Parse(HttpContext.User.Identities.FirstOrDefault().Name);
                request.UserId = userId;
                ResponseViewModel item = await _IAppUserService.ClientChangePassword(request);

                return Ok(item);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("changepassword")]
        [HttpPut]
        public async Task<ActionResult> ChangePassword([FromBody] UpdatePasswordRequest request)
        {

            try
            {

                ResponseViewModel item = await _IAppUserService.ChangePassword(request);

                return Ok(item);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("register")]
        [HttpPost]
        public async Task<ActionResult> Register([FromForm] UserCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                if (!ModelState.IsValid)
                {
                    postResult.Status = false;
                    postResult.Data = ModelState.ToString();
                }
                if (request.Password != request.ConfirmPassword)
                {
                    postResult.Status = false;
                    postResult.Data = "Thông tin mật khẩu không trùng khớp";
                };
                request.IsCustomer = true;
                var result = await _IAppUserService.Register(request);
                if (!result.Succeeded)
                {
                    postResult.Status = false;
                    postResult.Data = JsonConvert.DeserializeObject(result.Errors);
                }
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }


        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetSearch([FromBody] GetUserPagingRequest request)
        {

            try
            {
                var data = await _IAppUserService.GetAllPaging(request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetById(Guid id)
        {
            var postResult = new PostResult();
            try
            {
                var data = await _IAppUserService.GetById(id);
                postResult.Data = data;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }



        [Route("update")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] UserUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IAppUserService.Update(request);


            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }
        [Route("update/workflow")]
        [HttpPut]
        public async Task<ActionResult> UpdateWorkflow([FromBody] UserUpdateworkdlowRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IAppUserService.UpdateWorkflow(request);


            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<ActionResult> Delete(Guid id)
        {
            var postResult = new PostResult();
            try
            {

                int item = await _IAppUserService.Delete(id);

            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("open")]
        [HttpPost]
        public async Task<ActionResult> Open(Guid id)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                int item = await _IAppUserService.OpenUser(id);
                if (item == 0)
                {
                    return BadRequest("Cannot find item");
                }
                return Ok(true);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}
