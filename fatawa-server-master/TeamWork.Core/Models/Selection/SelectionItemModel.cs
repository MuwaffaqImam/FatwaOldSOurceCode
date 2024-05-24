using System;
using System.Collections.Generic;

namespace TeamWork.Core.Models.Selection
{
    public class SelectionItemModel : BaseModel
    {
        public int Order { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }
        public string Tags { get; set; }

        public int SelectionTypeId { get; set; }
        public string SelectionTypeName { get; set; }

        public List<SelectionItemTranslationModel> SelectionItemTranslations { get; set; }
    }

    public class SelectionItemTranslationModel : BaseTranslationModel
    {
        public int SelectionItemId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
