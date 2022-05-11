using LuvWordy.Server.Model.Enums;
using System.Data;
using System.Text.Json.Serialization;

namespace LuvWordy.Server.Model.Models
{
    /// <summary>
    /// 단어. 약식 (참조용)
    /// </summary>
    public class WordItemBase
    {
        #region Constructor
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
        #endregion

        /// <summary>
        /// 한국어 기초 사전 기준 ID 컬럼 (중복값이 존재하므로, 참조용으로 사용)
        /// </summary>
        public int DataNumber { get; set; }

        /// <summary>
        /// 표기 시, 단어의 모양
        /// </summary>
        public string WrittenForm { get; set; }
    }

    /// <summary>
    /// 단어. 요약 (목록용)
    /// </summary>
    public class WordItemSummary : WordItemBase
    {
        #region Constructor
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
        #endregion

        /// <summary>
        /// 단어 ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 단어의 정의 ID
        /// </summary>
        public Guid DefinitionId { get; set; }

        /// <summary>
        /// 동음이의어의 개수
        /// </summary>
        public int HomonymNumber { get; set; }

        /// <summary>
        /// 어휘의 단위 (DB 저장 값)
        /// </summary>
        public string LexicalUnitText { get; set; }

        /// <summary>
        /// 품사 (DB 저장 값)
        /// </summary>
        public string PartOfSpeechText { get; set; }

        /// <summary>
        /// 표현의 난이도 (DB 저장 값)
        /// </summary>
        public string VocabularyLevelText { get; set; }

        /// <summary>
        /// 어휘의 단위
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
        /// 품사
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
        /// 표현의 난이도
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

    /// <summary>
    /// 단어 모델
    /// </summary>
    public class WordItem : WordItemSummary
    {
        #region Constructor
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
        #endregion

        /// <summary>
        /// 단어의 정의
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// 단어의 발음 JSON Blob (Non-serialized)
        /// </summary>
        [JsonIgnore]
        public string PronunciationJSON { get; set; }

        /// <summary>
        /// 단어의 발음
        /// </summary>
        public List<PronunciationItem> Pronunciations
        {
            get
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<PronunciationItem>>(PronunciationJSON) ?? new List<PronunciationItem>();
            }
        }

        /// <summary>
        /// 단어의 활용 JSON Blob (Non-serialized)
        /// </summary>
        [JsonIgnore]
        public string ConjugationJSON { get; set; }

        /// <summary>
        /// 단어의 활용
        /// </summary>
        public List<ConjugationItem> Conjugations
        {
            get
            {
                return System.Text.Json.JsonSerializer.Deserialize<List<ConjugationItem>>(ConjugationJSON) ?? new List<ConjugationItem>();
            }
        }

        /// <summary>
        /// 표제어 존재 여부
        /// </summary>
        public bool HasEntryWord => EntryWord != null;

        /// <summary>
        /// 표제어
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public WordItemBase? EntryWord { get; set; }
    }
}