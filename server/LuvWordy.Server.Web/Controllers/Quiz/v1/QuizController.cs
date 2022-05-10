using Microsoft.AspNetCore.Mvc;

namespace LuvWordy.Server.Web.Controllers.Quiz
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/quiz")]
    public class QuizController : ControllerBase
    {
        private readonly ILogger<QuizController> _logger;

        public QuizController(ILogger<QuizController> logger)
        {
            _logger = logger;
        }

    }
}
