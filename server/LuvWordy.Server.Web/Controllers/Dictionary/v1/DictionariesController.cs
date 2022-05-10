using Microsoft.AspNetCore.Mvc;

namespace LuvWordy.Server.Web.Controllers.Dictionary
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dictionaries")]
    public class DictionariesController : ControllerBase
    {
        private readonly ILogger<DictionariesController> _logger;

        public DictionariesController(ILogger<DictionariesController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("hello")]
        public async Task<IActionResult> HelloWorld()
        {
            return Ok("world");
        }

    }
}
