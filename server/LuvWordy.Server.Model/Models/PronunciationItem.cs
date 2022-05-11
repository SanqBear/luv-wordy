namespace LuvWordy.Server.Model.Models
{
    /// <summary>
    /// 발음 모델
    /// </summary>
    public class PronunciationItem
    {
        public PronunciationItem()
        {
            Id = Guid.Empty;
            Pronunciation = string.Empty;
            SoundUrl = string.Empty;
        }

        /// <summary>
        /// 발음 ID
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 발음
        /// </summary>
        public string Pronunciation { get; set; }

        /// <summary>
        /// 발음 음성 URL
        /// </summary>
        public string SoundUrl { get; set; }
    }

    /// <summary>
    /// 활용 모델
    /// </summary>
    public class ConjugationItem : PronunciationItem
    {
        public ConjugationItem()
        {
            WrittenForm = string.Empty;
        }

        /// <summary>
        /// 표기 시, 단어의 모양
        /// </summary>
        public string WrittenForm { get; set; }
    }
}