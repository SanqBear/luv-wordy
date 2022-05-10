using System.Text.Json.Serialization;

namespace LuvWordy.Server.Model.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum VocabularyLevelType
    {
        Unknown,

        // 없음
        None,

        // 초급
        Easy,

        // 중급
        Normal,

        // 고급
        Hard
    }
}