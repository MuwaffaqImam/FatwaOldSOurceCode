using System.Collections.Generic;

namespace TeamWork.Core.Models.Video
{
    public class VideoTypeModel : BaseModel
    {
        public List<VideoTypeTranslationModel> VideoTypeTranslations { get; set; }
    }

    public class VideoTypeTranslationModel : BaseTranslationModel
    {
        public int VideoTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
