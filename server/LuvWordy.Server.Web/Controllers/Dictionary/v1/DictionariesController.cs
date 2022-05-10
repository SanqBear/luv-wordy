using LuvWordy.Server.Model.Models;
using LuvWordy.Server.Model.Repositories;
using LuvWordy.Server.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace LuvWordy.Server.Web.Controllers.Dictionary
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/dictionaries")]
    public class DictionariesController : ControllerBase
    {
        private readonly ILogger<DictionariesController> _logger;
        private readonly IConfiguration _configuration;

        private readonly string _wordRepoConnectionString;

        public DictionariesController(ILogger<DictionariesController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;

            _wordRepoConnectionString = _configuration.GetConnectionString(WordRepository.KEY);
        }

        /// <summary>
        /// Get dictionaries
        /// </summary>
        /// <param name="page">page number</param>
        /// <param name="size">fetch count</param>
        /// <returns>dictionary summary items</returns>
        /// <remarks>
        /// Sample request :
        /// 
        ///     GET /api/v1/dictionaries
        /// 
        /// </remarks>
        /// <response code="200">return dictionaries successfully</response>
        /// <response code="500">occured unexpected error</response>
        [HttpGet]
        [Route("")]
        [ProducesResponseType(typeof(ApiPagedResult<WordItemSummary>), 200)]
        public async Task<IActionResult> GetDictionaries([FromQuery] int? page, [FromQuery] int? size)
        {
            try
            {
                ApiPagedResult<WordItemSummary> apiResult = new ApiPagedResult<WordItemSummary>();

                int pageProp = page ?? 1;
                int sizeProp = size ?? 15;
                int offsetProp = sizeProp * (pageProp - 1);

                await using (var repo = new WordRepository(_wordRepoConnectionString))
                {
                    (apiResult.TotalCount, apiResult.Data) = repo.GetWordItems(sizeProp, offsetProp);

                    apiResult.NextOffset = apiResult.TotalCount > offsetProp + sizeProp ? offsetProp + sizeProp : null;
                    apiResult.Success = true;

                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"occured unexpected error on [{nameof(DictionariesController)}] {nameof(GetDictionaries)}({nameof(page)}:'{page}',{nameof(size)}:'{size}')");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
