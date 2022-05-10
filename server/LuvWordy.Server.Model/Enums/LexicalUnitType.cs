using System.Text.Json.Serialization;

namespace LuvWordy.Server.Model.Enums
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LexicalUnitType
    {
        // ?
        Unknown,

        // 단어
        Word,

        // 구
        Phrase,

        // 관용구
        Idiom,

        // 속담
        Proverb,

        // 표현?문법
        Expression
    }
}