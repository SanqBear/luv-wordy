using LuvWordy.Server.Model.Enums;
using System.Data;
using System.Text.Json.Serialization;

namespace LuvWordy.Server.Model.Models
{
    public class WordItemBase
    {
        public WordItemBase()
        {
            DataNumber = -1;
            WrittenForm = string.Empty;
        }

        public WordItemBase(DataRow row)
        {
            DataNumber = int.TryParse(row["DataNo"]?.ToString(), out var datano) ? datano : -1;
            WrittenForm = row["WrittenForm"]?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// Base id for <한국어 기초 사전>
        /// </summary>
        public int DataNumber { get; set; }

        /// <summary>
        /// Written form on this word.
        /// </summary>
        public string WrittenForm { get; set; }
    }

    public class WordItemSummary : WordItemBase
    {
        public WordItemSummary() : base()
        {
            Id = Guid.Empty;
            DefinitionId = Guid.Empty;
            HomonymNumber = -1;
            LexicalUnitText = String.Empty;
            PartOfSpeechText = String.Empty;
            VocabularyLevelText = String.Empty;
        }

        public WordItemSummary(DataRow row) : base(row)
        {
            Id = Guid.TryParse(row["Id"]?.ToString(), out Guid id) ? id : Guid.Empty;
            DefinitionId = Guid.TryParse(row["DefinitionId"]?.ToString(), out Guid defid) ? defid : Guid.Empty;
            HomonymNumber = int.TryParse(row["HomonymNumber"]?.ToString(), out int hn) ? hn : -1;
            LexicalUnitText = row["LexicalUnit"]?.ToString() ?? String.Empty;
            PartOfSpeechText = row["PartOfSpeech"]?.ToString() ?? String.Empty;
            VocabularyLevelText = row["VocabularyLevel"]?.ToString() ?? String.Empty;
        }

        /// <summary>
        /// Word id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Word definition Id
        /// </summary>
        public Guid DefinitionId { get; set; }

        /// <summary>
        /// How many homonym words i haved.
        /// </summary>
        public int HomonymNumber { get; set; }

        /// <summary>
        /// LexicalUnit StoredText on DB
        /// </summary>
        public string LexicalUnitText { get; set; }

        /// <summary>
        /// PartofSpeech StoredText on DB
        /// </summary>
        public string PartOfSpeechText { get; set; }

        /// <summary>
        /// VocabularyLevel StoredText on DB
        /// </summary>
        public string VocabularyLevelText { get; set; }

        /// <summary>
        /// LexicalUnit (translated to en from ko)
        /// </summary>
        public LexicalUnitType LexicalUnit
        {
            get
            {
                switch (LexicalUnitText?.TrimStart().TrimEnd())
                {
                    default:
                        return Enum.TryParse<LexicalUnitType>(LexicalUnitText, ignoreCase: true, out var unit) ? unit : LexicalUnitType.Unknown;

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

        /// <summary>
        /// PartOfSpeech (translated to en from ko)
        /// </summary>
        public PartOfSpeechType PartOfSpeech
        {
            get
            {
                switch (PartOfSpeechText?.TrimStart().TrimEnd())
                {
                    default:
                        return Enum.TryParse<PartOfSpeechType>(PartOfSpeechText, ignoreCase: true, out var partOfSpeech) ? partOfSpeech : PartOfSpeechType.Unknown;

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

        /// <summary>
        /// VocabulraryLevel (translated to en from ko)
        /// </summary>
        public VocabularyLevelType VocabularyLevel
        {
            get
            {
                switch (VocabularyLevelText?.TrimStart().TrimEnd())
                {
                    default:
                        return Enum.TryParse<VocabularyLevelType>(VocabularyLevelText, out VocabularyLevelType vocabularyLevelType) ? vocabularyLevelType : VocabularyLevelType.Unknown;

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

    public class WordItem : WordItemSummary
    {
        public WordItem()
        {
            Definition = String.Empty;
            PronunciationJSON = String.Empty;
            ConjugationJSON = String.Empty;
            EntryWord = null;
        }

        public WordItem(DataRow row) : base(row)
        {
            Definition = row["Definition"]?.ToString() ?? String.Empty;
            PronunciationJSON = row["PronunciationJSON"]?.ToString() ?? String.Empty;
            ConjugationJSON = row["ConjugationJSON"]?.ToString() ?? String.Empty;

            if (row["EntryWordWrittenForm"] != null)
            {
                EntryWord = new WordItemBase()
                {
                    DataNumber = int.TryParse(row["EntryWordDataNo"]?.ToString(), out int ewdn) ? ewdn : -1,
                    WrittenForm = row["EntryWordWrittenForm"]?.ToString() ?? String.Empty,
                };
            }
        }

        /// <summary>
        /// word definition
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// word pronunciation
        /// </summary>
        public string PronunciationJSON { get; set; }

        /// <summary>
        /// word pronunciations deserialized
        /// </summary>
        public List<PronunciationItem> Pronunciations
        {
            get
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<PronunciationItem>>(PronunciationJSON) ?? new List<PronunciationItem>();
            }
        }

        /// <summary>
        /// word conjugation
        /// </summary>
        public string ConjugationJSON { get; set; }

        /// <summary>
        /// word conjugations deserialized
        /// </summary>
        public List<ConjugationItem> Conjugations
        {
            get
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<ConjugationItem>>(ConjugationJSON) ?? new List<ConjugationItem>();
            }
        }

        /// <summary>
        /// Having EntryWord yes or no
        /// </summary>
        public bool HasEntryWord => EntryWord != null;

        /// <summary>
        /// entry word
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WordItemBase? EntryWord { get; set; }
    }
}