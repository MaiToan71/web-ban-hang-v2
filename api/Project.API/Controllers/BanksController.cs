using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Banks;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.BankConfig;

namespace Project.API.Controllers
{
    public class BanksController : BaseController
    {

        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IBankService _IBankService;


        public BanksController(IBankService IBankService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IBankService = IBankService;
        }


        /// <summary>
        /// GetSearch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Data.Entities.BankConfig>))]
        public async Task<IActionResult> GetSearch([FromBody] GetBankPagingRequest request)
        {
            try
            {
                var data = await _IBankService.GetAllPaging(request: request);
                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        /// <summary>
        /// GetSearch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("qrcode")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Data.Entities.BankConfig>))]
        public async Task<IActionResult> GetQrCodeinBill([FromBody] GetQrCodeRequest request)
        {
            try
            {
                // request.StoreId ??= StoreId;

                var data = await _IBankService.GetQrCodeinBill(request);
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
        public async Task<ActionResult> AddNew([FromBody] BankCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IBankService.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        public async Task<ActionResult> Update([FromBody] BankUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IBankService.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        /// Delete
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("delete")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IBankService.Delete(id, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
