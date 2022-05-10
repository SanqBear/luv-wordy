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
        /// 단어 목록을 가져옵니다
        /// </summary>
        /// <param name="page">페이지 번호</param>
        /// <param name="size">가져올 아이템 수</param>
        /// <returns>단어 목록 (Summary)</returns>
        /// <remarks>
        /// 호출 예 :
        ///
        ///     GET /api/v1/dictionaries
        ///
        /// </remarks>
        /// <response code="200">단어 목록 및 총 아이템 수를 반환</response>
        /// <response code="500">오류 메시지 반환</response>
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

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(typeof(ApiResult<Dictionary<Guid, WordItem>>), 200)]
        public async Task<IActionResult> GetDictionary(string id, [FromQuery]string? definitionId)
        {
            try
            {
                if(Guid.TryParse(id, out Guid idProp))
                {
                    ApiResult<Dictionary<Guid, WordItem>> apiResult = new ApiResult<Dictionary<Guid,WordItem>>();

                    Guid definitionIdProp = Guid.TryParse(definitionId, out definitionIdProp) ? definitionIdProp : Guid.Empty;

                    await using(var repo = new WordRepository(_wordRepoConnectionString))
                    {
                        List<WordItem> items = repo.GetWordItemDetails(idProp, definitionIdProp);

                        apiResult.Data = items.ToDictionary(o => o.Id);
                        apiResult.Success = true;
                    }

                    return Ok(apiResult);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, $"occured unexpected error on [{nameof(DictionariesController)}] {nameof(GetDictionary)}({nameof(id)}:'{id}',{nameof(definitionId)}:'{definitionId}')");
                return StatusCode(500, ex.Message);
            }
        }
    }
}