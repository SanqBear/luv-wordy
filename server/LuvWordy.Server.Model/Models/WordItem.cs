using LuvWordy.Server.Model.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LuvWordy.Server.Model.Models
{
    public class WordItemBase
    {
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
        /// <summary>
        /// word definition
        /// </summary>
        public string Definition { get; set; }

        /// <summary>
        /// word pronunciation
        /// </summary>
        public string PronunciationJSON { get; set; }

        /// <summary>
        /// word conjugation
        /// </summary>
        public string ConjugationJSON { get; set; }

        /// <summary>
        /// entry word
        /// </summary>
        public WordItemBase? EntryWord { get; set; }

    }
}
