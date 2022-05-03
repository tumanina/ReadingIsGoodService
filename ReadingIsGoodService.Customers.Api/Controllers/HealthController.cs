using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReadingIsGoodService.Common.Models;

namespace ReadingIsGoodService.Customers.Api.Controllers
{
    [Route("api/health")]
    [ApiController]
    [AllowAnonymous]
    public class HealthController : ControllerBase
    {
        /// <summary>
        /// Return the status of the application
        /// </summary>
        /// <returns>BaseApiModel</returns>
        [HttpGet]

        public BaseApiModel CheckHealth()
        {
            //check infrastructure (db, 3rd party services) if needed
            return new BaseApiModel();
        }
    }
}
