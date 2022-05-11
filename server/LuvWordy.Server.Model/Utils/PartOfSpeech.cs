using LuvWordy.Server.Model.Enums;

namespace LuvWordy.Server.Model.Utils
{
    public class PartOfSpeech
    {
        public static string ToString(PartOfSpeechType partOfSpeech)
        {
            switch (partOfSpeech)
            {
                default:
                    return "Unknown";

                case PartOfSpeechType.Noun:
                    return "명사";

                case PartOfSpeechType.Verb:
                    return "동사";

                case PartOfSpeechType.Pronoun:
                    return "대명사";

                case PartOfSpeechType.Adjective:
                    return "형용사";

                case PartOfSpeechType.DependentNoun:
                    return "의존 명사";

                case PartOfSpeechType.AuxiliaryVerb:
                    return "보조 동사";

                case PartOfSpeechType.Numerals:
                    return "수사";

                case PartOfSpeechType.Postposition:
                    return "조사";

                case PartOfSpeechType.Affix:
                    return "접사";

                case PartOfSpeechType.Adverb:
                    return "부사";

                case PartOfSpeechType.Interjection:
                    return "감탄사";

                case PartOfSpeechType.Detective:
                    return "관형사";

                case PartOfSpeechType.AuxiliaryAdjective:
                    return "보조 형용사";

                case PartOfSpeechType.Termination:
                    return "어미";

                case PartOfSpeechType.None:
                    return "품사 없음";
            }
        }

        public static PartOfSpeechType ToEnum(string partOfSpeechText)
        {
            switch (partOfSpeechText?.TrimStart().TrimEnd())
            {
                default:
                    return Enum.TryParse<PartOfSpeechType>(partOfSpeechText, ignoreCase: true, out var partOfSpeech) ? partOfSpeech : PartOfSpeechType.Unknown;

                case "명사":
                    return PartOfSpeechType.Noun;

                case "동사":
                    return PartOfSpeechType.Verb;

                case "대명사":
                    return PartOfSpeechType.Pronoun;

                case "형용사":
                    return PartOfSpeechType.Adjective;

                case "의존 명사":
                    return PartOfSpeechType.DependentNoun;

                case "보조 동사":
                    return PartOfSpeechType.AuxiliaryVerb;

                case "수사":
                    return PartOfSpeechType.Numerals;

                case "조사":
                    return PartOfSpeechType.Postposition;

                case "접사":
                    return PartOfSpeechType.Affix;

                case "부사":
                    return PartOfSpeechType.Adverb;

                case "감탄사":
                    return PartOfSpeechType.Interjection;

                case "관형사":
                    return PartOfSpeechType.Detective;

                case "보조 형용사":
                    return PartOfSpeechType.AuxiliaryAdjective;

                case "어미":
                    return PartOfSpeechType.Termination;

                case "품사 없음":
                    return PartOfSpeechType.None;
            }
        }
    }
}