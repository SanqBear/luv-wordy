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

        #endregion Constructor

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

        #endregion Constructor

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
                return Utils.LexicalUnit.ToEnum(LexicalUnitText);
            }
        }

        /// <summary>
        /// 품사
        /// </summary>
        public PartOfSpeechType PartOfSpeech
        {
            get
            {
                return Utils.PartOfSpeech.ToEnum(PartOfSpeechText);
            }
        }

        /// <summary>
        /// 표현의 난이도
        /// </summary>
        public VocabularyLevelType VocabularyLevel
        {
            get
            {
                return Utils.VocabularyLevel.ToEnum(VocabularyLevelText);
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

        #endregion Constructor

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