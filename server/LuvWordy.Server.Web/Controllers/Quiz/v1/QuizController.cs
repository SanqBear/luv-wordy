using Microsoft.AspNetCore.Mvc;

namespace LuvWordy.Server.Web.Controllers.Quiz
{
    [ApiController]
    [Route("api/quiz")]
    public class QuizController : ControllerBase
    {
        private readonly ILogger<QuizController> _logger;

        public QuizController(ILogger<QuizController> logger)
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
