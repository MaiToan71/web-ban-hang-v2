using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Orders;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Orders;

namespace Project.API.Controllers.Clients
{
    [Route("api/client/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IOrderService _IOrderService;

        public OrderController(IOrderService IOrderService, IAppUserService userService,
            ICachingExtension cachingExtension)
        {
            _IOrderService = IOrderService;
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

                bool item = await _IOrderService.AddNew(request, new Guid(), "Giỏ hàng");
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
