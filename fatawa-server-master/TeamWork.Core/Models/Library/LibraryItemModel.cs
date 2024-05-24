using System;
using System.Collections.Generic;

namespace TeamWork.Core.Models.Library
{
    public class LibraryItemModel : BaseModel
    {
        public int Order { get; set; }
        public decimal Version { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }
        public string Tags { get; set; }
        public string URLDownLoad { get; set; }

        public int ExtensionType { get; set; }
        public string ExtensionTypeName { get; set; }

        public List<LibraryItemTranslationModel> LibraryItemTranslations { get; set; }
    }

    public class LibraryItemTranslationModel : BaseTranslationModel
    {
        public int LibraryItemId { get; set; }
        public string ItemName { get; set; }
        public string AuthorName { get; set; }
        public string AuditingName { get; set; }
    }
}
