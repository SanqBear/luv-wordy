using LuvWordy.Server.Model.Enums;
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
        /// <param name="offset">건너 뛸 아이템 수 (페이지보다 우선함)</param>
        /// <returns>단어 목록 (Summary)</returns>
        /// <remarks>
        /// 호출 예 :
        ///
        ///     GET /api/v1/dictionaries
        ///
        /// </remarks>
        /// <response code="200">단어 목록 및 총 아이템 수를 반환</response>
        /// <response code="500">오류 발생</response>
        [HttpGet]
        [Route("", Name = nameof(GetDictionaries))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiPagedResult<WordItemSummary>), 200)]
        public async Task<IActionResult> GetDictionaries([FromQuery] int? page, [FromQuery] int? size, [FromQuery] int? offset)
        {
            try
            {
                ApiPagedResult<WordItemSummary> apiResult = new ApiPagedResult<WordItemSummary>();

                int pageProp = page ?? 1;
                int sizeProp = size ?? 15;
                int offsetProp = (offset != null) ? (int)offset : sizeProp * (pageProp - 1);

                await using (var repo = new WordRepository(_wordRepoConnectionString))
                {
                    (apiResult.TotalCount, apiResult.Data) = repo.GetWordItems(sizeProp, offsetProp);

                    apiResult.NextOffset = apiResult.TotalCount > offsetProp + sizeProp ? offsetProp + sizeProp : null;
                    apiResult.Success = apiResult.TotalCount != -1;

                    return Ok(apiResult);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"occured unexpected error on [{nameof(DictionariesController)}] {nameof(GetDictionaries)}({nameof(page)}:'{page}',{nameof(size)}:'{size}')");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 특정 ID의 단어들을 가져옵니다. (동음이의어 포함)
        /// </summary>
        /// <param name="id">단어 ID</param>
        /// <param name="definitionId">정의 ID</param>
        /// <returns>id : key , word : value</returns>
        /// <remarks>
        /// 호출 예 :
        ///
        ///     GET /api/v1/dictionaries/9D63F6D9-52E1-4447-A7DD-0008E52FBBC9
        ///
        /// </remarks>
        /// <response code="200">단어를 반환</response>
        /// <response code="400">ID를 파싱할 수 없음</response>
        /// <response code="500">오류 발생</response>
        [HttpGet]
        [Route("{id}", Name = nameof(GetDictionary))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiResult<Dictionary<Guid, WordItem>>), 200)]
        public async Task<IActionResult> GetDictionary(string id, [FromQuery] string? definitionId)
        {
            try
            {
                if (Guid.TryParse(id, out Guid idProp))
                {
                    ApiResult<Dictionary<Guid, WordItem>> apiResult = new ApiResult<Dictionary<Guid, WordItem>>();

                    Guid definitionIdProp = Guid.TryParse(definitionId, out definitionIdProp) ? definitionIdProp : Guid.Empty;

                    await using (var repo = new WordRepository(_wordRepoConnectionString))
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
            catch (Exception ex)
            {
                _logger.LogError(ex, $"occured unexpected error on [{nameof(DictionariesController)}] {nameof(GetDictionary)}({nameof(id)}:'{id}',{nameof(definitionId)}:'{definitionId}')");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// 사전 검색 파라메터
        /// </summary>
        /// <param name="keyword">검색 키워드</param>
        /// <param name="lexicalUnit">단어 단위</param>
        /// <param name="partOfSpeech">품사</param>
        /// <param name="vocabularyLevel">표현 난이도</param>
        public record DictionarySearchParams(string? keyword, string? lexicalUnit, string? partOfSpeech, string? vocabularyLevel);

        /// <summary>
        /// 단어 목록을 검색합니다
        /// </summary>
        /// <param name="page">페이지 번호</param>
        /// <param name="size">가져올 아이템 수</param>
        /// <param name="offset">건너 뛸 아이템 수 (페이지보다 우선함)</param>
        /// <param name="params">검색 파라메터</param>
        /// <returns>단어 목록 (Summary)</returns>
        /// <remarks>
        /// 호출 예 :
        ///
        ///     POST /api/v1/dictionaries
        ///     {
        ///         "keyword": "나라",
        ///         "lexicalUnit": "word",
        ///         "partOfSpeech": "noun",
        ///         "vocabularyLevel": "easy"
        ///     }
        ///     
        /// </remarks>
        /// <response code="200">색인된 단어 목록 및 총 아이템 수를 반환</response>
        /// <response code="500">오류 발생</response>
        [HttpPost]
        [Route("search", Name = nameof(SearchDictionary))]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ApiPagedResult<WordItemSummary>), 200)]
        public async Task<IActionResult> SearchDictionary([FromQuery] int? page, [FromQuery] int? size, [FromQuery] int? offset, [FromBody] DictionarySearchParams @params)
        {
            try
            {
                ApiPagedResult<WordItemSummary> apiResult = new ApiPagedResult<WordItemSummary>();

                int pageProp = page ?? 1;
                int sizeProp = size ?? 15;
                int offsetProp = (offset != null) ? (int)offset : sizeProp * (pageProp - 1);
                string keywordProp;
                LexicalUnitType lexicalUnitProp;
                PartOfSpeechType partOfSpeechProp;
                VocabularyLevelType vocabularyLevelProp;

                keywordProp = @params?.keyword ?? string.Empty;
                lexicalUnitProp = int.TryParse(@params?.lexicalUnit, out int lunum) ? (LexicalUnitType)lunum : Enum.TryParse(@params?.lexicalUnit, ignoreCase: true, out LexicalUnitType lu) ? lu : LexicalUnitType.NotInterested;
                partOfSpeechProp = int.TryParse(@params?.partOfSpeech, out int posnum) ? (PartOfSpeechType)posnum : Enum.TryParse(@params?.partOfSpeech, ignoreCase: true, out PartOfSpeechType pos) ? pos : PartOfSpeechType.NotInterested;
                vocabularyLevelProp = int.TryParse(@params?.vocabularyLevel, out int volnum) ? (VocabularyLevelType)volnum : Enum.TryParse(@params?.vocabularyLevel, ignoreCase: true, out VocabularyLevelType vol) ? vol : VocabularyLevelType.NotInterested;

                await using (var repo = new WordRepository(_wordRepoConnectionString))
                {
                    (apiResult.TotalCount, apiResult.Data) = repo.SearchWordItems(size: sizeProp, offset: offsetProp, keyword: keywordProp, lexicalUnit: lexicalUnitProp, partOfSpeech: partOfSpeechProp, vocabularyLevel: vocabularyLevelProp);
                    apiResult.Success = apiResult.TotalCount != -1;
                    apiResult.NextOffset = apiResult.TotalCount > offsetProp + sizeProp ? offsetProp + sizeProp : null;
                }

                return Ok(apiResult);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"occured unexpected error on [{nameof(DictionariesController)}] {nameof(SearchDictionary)}({System.Text.Json.JsonSerializer.Serialize(@params)})");
                return StatusCode(500, ex.Message);
            }
        }
    }
}