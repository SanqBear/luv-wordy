using System.Text.Json.Serialization;

namespace LuvWordy.Server.Web.Models
{
    public class ApiResult
    {
        /// <summary>
        /// 작업 성공 여부. API 콜 응답성공여부는 HTTP ResponseCode 로 응답
        /// </summary>
        public bool Success { get; set; } = false;

        /// <summary>
        /// 오류 메시지
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Message { get; set; } = null;
    }

    public class ApiResult<T> : ApiResult
    {
        /// <summary>
        /// 데이터
        /// </summary>
        public T? Data { get; set; } = default(T);
    }

    public class ApiPagedResult<T> : ApiResult
    {
        /// <summary>
        /// 데이터 (List)
        /// </summary>
        public List<T> Data { get; set; } = new List<T>();

        /// <summary>
        /// 총 아이템 수
        /// </summary>
        public int TotalCount { get; set; } = 0;

        /// <summary>
        /// Offset 이후로 Item이 더 있는지
        /// </summary>
        public bool MoreAvailable => NextOffset != null;

        /// <summary>
        /// 다음 Offset
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NextOffset { get; set; } = null;
    }
}