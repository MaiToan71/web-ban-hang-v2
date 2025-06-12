using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Positions;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Positions;

namespace Project.API.Controllers
{
    public class PositionsController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IPositionService _IPositionService;

        public PositionsController(IPositionService IPositionService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IPositionService = IPositionService;
        }
        [HttpPost]
        [Route("search")]
        public async Task<IActionResult> GetSearch([FromBody] GetPositionPagingRequest request)
        {

            try
            {

                var data = await _IPositionService.GetAllPaging(request: request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> AddNew([FromForm] PositionCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IPositionService.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        public async Task<ActionResult> Update([FromForm] PositionUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IPositionService.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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

        [Route("delete")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IPositionService.Delete(id, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
