using LuvWordy.Server.Model.Enums;

namespace LuvWordy.Server.Model.Utils
{
    public class LexicalUnit
    {
        public static string ToString(LexicalUnitType lexicalUnit)
        {
            switch (lexicalUnit)
            {
                default:
                    return "Unknown";

                case LexicalUnitType.Word:
                    return "단어";

                case LexicalUnitType.Phrase:
                    return "구";

                case LexicalUnitType.Idiom:
                    return "관용구";

                case LexicalUnitType.Proverb:
                    return "속담";

                case LexicalUnitType.Expression:
                    return "문법?표현";
            }
        }

        public static LexicalUnitType ToEnum(string lexicalUnitText)
        {
            switch (lexicalUnitText?.TrimStart().TrimEnd())
            {
                default:
                    return Enum.TryParse<LexicalUnitType>(lexicalUnitText, ignoreCase: true, out var unit) ? unit : LexicalUnitType.Unknown;

                case "단어":
                    return LexicalUnitType.Word;

                case "구":
                    return LexicalUnitType.Phrase;

                case "관용구":
                    return LexicalUnitType.Idiom;

                case "속담":
                    return LexicalUnitType.Proverb;

                case "문법?표현":
                    return LexicalUnitType.Expression;
            }
        }
    }
}