using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
