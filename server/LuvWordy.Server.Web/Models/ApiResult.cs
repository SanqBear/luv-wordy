using System.Text.Json.Serialization;

namespace LuvWordy.Server.Web.Models
{
    public class ApiResult
    {
        public bool Success { get; set; } = false;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public string? Message { get; set; } = null;
    }

    public class ApiResult<T> : ApiResult
    {
        private T? Data { get; set; } = default(T);
    }

    public class ApiPagedResult<T> : ApiResult
    {
        public List<T> Data { get; set; } = new List<T>();

        public int TotalCount { get; set; } = 0;

        public bool MoreAvailable => NextOffset != null;

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? NextOffset { get; set; } = null;
    }
}