using System;
using System.Collections.Generic;

namespace TeamWork.Core.Models.Video
{
    public class YoutubeVideoModel : BaseModel
    {
        public int Order { get; set; }
        public string URL { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }
        public string Tags { get; set; }

        public int VideoTypeId { get; set; }
        public string VideoTypeName { get; set; }

        public int PlaylistId { get; set; }
        public string PlaylistName { get; set; }

        public List<YoutubeVideoTranslationModel> YoutubeVideoTranslations { get; set; }
    }

    public class YoutubeVideoTranslationModel : BaseTranslationModel
    {
        public int YoutubeVideoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
