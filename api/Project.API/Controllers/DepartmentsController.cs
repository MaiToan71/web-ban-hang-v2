using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Departments;
using Project.Services.Users.Interfaces;
using Project.ViewModels;
using Project.ViewModels.Departments;

namespace Project.API.Controllers
{

    public class DepartmentsController : BaseController
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;
        private readonly IDepartmentService _IDepartmentService;


        public DepartmentsController(IDepartmentService IDepartmentService, IAppUserService userService,
            ICachingExtension cachingExtension) : base(cachingExtension, userService)
        {
            _IDepartmentService = IDepartmentService;
        }


        /// <summary>
        /// GetSearch
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PagedResult<Data.Entities.Department>))]
        public async Task<IActionResult> GetSearch([FromBody] GetDepartmentPagingRequest request)
        {
            try
            {
                var data = await _IDepartmentService.GetAllPaging(request: request);
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
        public async Task<ActionResult> AddNew([FromBody] DepartmentCreateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IDepartmentService.AddNew(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
        public async Task<ActionResult> Update([FromBody] DepartmentUpdateRequest request)
        {
            var postResult = new PostResult();
            try
            {

                bool item = await _IDepartmentService.Update(request, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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

                bool item = await _IDepartmentService.Delete(id, CurrentUserViewModel.UserId, CurrentUserViewModel.FullName);
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
