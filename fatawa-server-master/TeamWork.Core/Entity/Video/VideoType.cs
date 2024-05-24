using System.Collections.Generic;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Video
{
    public class VideoType : BaseEntity
    {
        public virtual ICollection<VideoTypeTranslation> VideoTypeTranslations { get; set; }
    }

    public class VideoTypeTranslation : TranslateBase
    {
        public int VideoTypeId { get; set; }
        public string TypeName { get; set; }
    }
}
