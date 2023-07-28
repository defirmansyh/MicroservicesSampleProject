using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLog;
using TiVi.Logging.Utilities;

namespace TiVi.Logging.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoggingController : ControllerBase
    {
        private static Logger logger = LogManager.GetLogger("LoggingController");

        [Authorize]
        [HttpPost]
        [Route("LogInfo")]
        public ActionResult LogInfo(string message)
        {
            var user = ClaimIdentity.CurrentUserName;
            logger.Info($"{user} - {message}");
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("LogError")]
        public ActionResult LogError(string message)
        {
            var user = ClaimIdentity.CurrentUserName;
            logger.Error($"{user} - {message}");
            return Ok();
        }
    }
}
