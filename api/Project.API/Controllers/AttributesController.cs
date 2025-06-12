using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Attributes;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Attributes;

namespace Project.API.Controllers
{
    public class AttributesController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IAttributeService _IAttributeService;


        public AttributesController(IAttributeService IAttributeService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IAttributeService = IAttributeService;
        }


        /// <summary>
        /// GetSearch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Data.Entities.Attribute>))]
        public async Task<IActionResult> GetSearch([FromBody] GetAttributePagingRequest request)
        {
            try
            {
                var data = await _IAttributeService.GetAllPaging(request: request);
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
        public async Task<ActionResult> AddNew([FromBody] AttributeCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IAttributeService.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        public async Task<ActionResult> Update([FromBody] AttributeUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IAttributeService.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> Delete(int id)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IAttributeService.Delete(id, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
