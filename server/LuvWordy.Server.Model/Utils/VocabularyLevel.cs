using LuvWordy.Server.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvWordy.Server.Model.Utils
{
    public class VocabularyLevel
    {
        public static string ToString(VocabularyLevelType vocabularyLevel)
        {
            switch (vocabularyLevel)
            {
                default:
                    return "Unknown";
                case VocabularyLevelType.None:
                    return "없음";
                case VocabularyLevelType.Easy:
                    return "초급";
                case VocabularyLevelType.Normal:
                    return "중급";
                case VocabularyLevelType.Hard:
                    return "고급";
            }
        }

        public static VocabularyLevelType ToEnum(string vocabularyLevelText)
        {
            switch (vocabularyLevelText?.TrimStart().TrimEnd())
            {
                default:
                    return Enum.TryParse<VocabularyLevelType>(vocabularyLevelText, out VocabularyLevelType vocabularyLevelType) ? vocabularyLevelType : VocabularyLevelType.Unknown;

                case "없음":
                    return VocabularyLevelType.None;

                case "초급":
                    return VocabularyLevelType.Easy;

                case "중급":
                    return VocabularyLevelType.Normal;

                case "고급":
                    return VocabularyLevelType.Hard;
            }
        }
    }
}
