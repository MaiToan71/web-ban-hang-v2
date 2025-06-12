using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Notification;
using Project.Services.Users.Interfaces;

using Project.ViewModels;
using Project.ViewModels.Notification;

namespace Project.API.Controllers
{

    public class NotificationController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly INotificationService _INotificationService;
        private readonly ILogger<NotificationController> _logger;
        public NotificationController(ILogger<NotificationController> logger, INotificationService INotificationService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _logger = logger;
            _INotificationService = INotificationService;
        }

        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetSearch([FromBody] GetNotificationPagingRequest request)
        {

            try
            {

                var data = await _INotificationService.GetAllPaging(request: request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> AddNew([FromBody] NotificationCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _INotificationService.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (!item)
                {
                    postResult.Status = false;
                    postResult.Data = "Cannot find item";
                }

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
        public async Task<ActionResult> Update([FromBody] NotificationUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _INotificationService.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (!item)
                {
                    postResult.Status = false;
                    postResult.Data = "Cannot find item";
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("delete")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _INotificationService.Delete(id, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
                if (!item)
                {
                    postResult.Status = false;
                    postResult.Data = "Cannot find item";
                }


            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

    }
}
