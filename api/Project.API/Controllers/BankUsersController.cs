using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;

using Project.Services.BankUsers;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.BankUsers;

namespace Project.API.Controllers
{

    public class BankUsersController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IBankUserService _IBankUserService;


        public BankUsersController(IBankUserService IBankUserService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IBankUserService = IBankUserService;
        }
        /// <summary>
        /// GetSearch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Data.Entities.BankUser>))]
        public async Task<IActionResult> GetSearch([FromBody] GetBankUserPagingRequest request)
        {
            try
            {
                var data = await _IBankUserService.GetAllPaging(request: request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        /// <summary>
        /// AddNew
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("add")]
        [HttpPost]
        public async Task<ActionResult> AddNew([FromBody] BankUserCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {
                request.Workflow = Enums.Workflow.WAITING;
                bool item = await _IBankUserService.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("update")]
        [HttpPut]
        public async Task<ActionResult> Update([FromBody] BankUserUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IBankUserService.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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


        [Route("update/workflow")]
        [HttpPut]
        public async Task<ActionResult> UpdateWorkflow([FromBody] BankUserUpdateWorkflowRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IBankUserService.UpdateWorkflow(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);


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
