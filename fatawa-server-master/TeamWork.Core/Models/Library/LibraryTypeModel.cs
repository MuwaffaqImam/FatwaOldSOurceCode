using System.Collections.Generic;

namespace TeamWork.Core.Models.Library
{
    public class LibraryTypeModel : BaseModel
    {
        public List<LibraryTypeTranslationModel> LibraryTypeTranslations { get; set; }
    }

    public class LibraryTypeTranslationModel : BaseTranslationModel
    {
        public int LibraryTypetId { get; set; }
        public string TypeName { get; set; }
    }
}
