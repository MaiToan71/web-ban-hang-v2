using Microsoft.AspNetCore.Mvc;
using Project.Services.Dashboards;
using Project.ViewModels;

namespace Project.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardsController : ControllerBase
    {
        private readonly IDashboardService _IDashboardService;

        public DashboardsController(IDashboardService IDashboardService)
        {
            _IDashboardService = IDashboardService;
        }
        [Route("statistical")]
        [HttpGet]
        public async Task<ActionResult> GetStatistical()
        {
            var postResult = new PostResult();
            try
            {
                var data = await _IDashboardService.GetStatistical();
                postResult.Data = data;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("statistical/role")]
        [HttpGet]
        public ActionResult GetStatisticalRole()
        {
            var postResult = new PostResult();
            try
            {
                var data = _IDashboardService.GetStatisticalRole();

                postResult.Data = data;
            }
            catch (Exception ex)
            {
                postResult.Status = false;
                postResult.Data = ex.Message;
            }
            return Ok(postResult);
        }

        [Route("statistical/money/date")]
        [HttpPost]
        public ActionResult GetStatisticalUserMoneyInDate([FromBody] DashboardRequest request)
        {
            var postResult = new PostResult();
            try
            {
                var data = _IDashboardService.GetStatisticalUserMoneyInDate(request);

                postResult.Data = data;
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
