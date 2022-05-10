namespace LuvWordy.Server.Model.Models
{
    public class PronunciationItem
    {
        public PronunciationItem()
        {
            Id = Guid.Empty;
            Pronunciation = string.Empty;
            SoundUrl = string.Empty;
        }

        public Guid Id { get; set; }

        public string Pronunciation { get; set; }

        public string SoundUrl { get; set; }
    }

    public class ConjugationItem : PronunciationItem
    {
        public ConjugationItem()
        {
            WrittenForm = string.Empty;
        }

        public string WrittenForm { get; set; }
    }
}