namespace TeamWork.Core.Models
{
    public class BaseTranslationModel
    {
        public int LanguageId { get; set; }
        public string LanguageCode { get; set; }
        public string LanguageName { get; set; }
        public bool IsDefault { get; set; }
    }
}
