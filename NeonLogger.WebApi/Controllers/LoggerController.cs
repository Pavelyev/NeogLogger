using Microsoft.AspNetCore.Mvc;

namespace NeonLogger.WebApi.Controllers
{
    public class LoggerController : Controller
    {
        [HttpPost]
        public IActionResult Log([FromBody] MessageModel model)
        {
            _logger.Log(model.Message);
            return Ok();
        }

        [HttpPost]
        public IActionResult LogDeferred([FromBody] MessageModel model)
        {
            _logger.LogDeferred(model.Message);
            return Ok();
        }

        [HttpGet]
        public IActionResult Popular()
        {
            return Ok(_logger.PopularMessages());
        }

        private readonly NeonLogger _logger;

        public LoggerController(NeonLogger logger)
        {
            _logger = logger;
        }
    }

    public class MessageModel
    {
        public string Message { get; set; }
    }
}