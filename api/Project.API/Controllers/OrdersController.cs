using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Enums;
using Project.Services.Orders;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Orders;

namespace Project.API.Controllers
{
    public class OrdersController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IOrderService _IOrderService;

        public OrdersController(IOrderService IOrderService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IOrderService = IOrderService;
        }


        /// <summary>
        /// GetSearch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Orderdetail/search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Data.Entities.Order>))]
        public async Task<IActionResult> GetOrderDetailSearch([FromBody] GetOrderPagingRequest request)
        {
            try
            {
                var data = await _IOrderService.GetAllOrderDetailPaging(request: request);
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
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Data.Entities.Order>))]
        public async Task<IActionResult> GetSearch([FromBody] GetOrderPagingRequest request)
        {
            try
            {
                var data = await _IOrderService.GetAllPaging(request: request);
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
        public async Task<ActionResult> AddNew([FromBody] OrderCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IOrderService.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        [Route("{id}")]
        [HttpGet]
        public async Task<ActionResult> GetById(int id)
        {
            var postResult = new PostResult();
            try
            {
                var item = _IOrderService.GetById(id);
                postResult.Data = item;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                // postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("SendWorkflow")]
        [HttpPost]
        public async Task<ActionResult> SendWorkflow(int OrderId, Workflow workflow)
        {
            var postResult = new PostResult();
            try
            {
                var item = await _IOrderService.SendWorkflow(OrderId, workflow);
                postResult.Data = item;
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
        public async Task<ActionResult> Update([FromBody] OrderUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IOrderService.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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

                bool item = await _IOrderService.Delete(id, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
