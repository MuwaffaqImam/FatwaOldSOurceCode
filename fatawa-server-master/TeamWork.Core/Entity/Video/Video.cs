using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Video
{
    public class YoutubeVideo : BaseEntity
    {
        public int Order { get; set; }
        public string URL { get; set; }
        public int Visitors { get; set; }
        public DateTime? LastSeen { get; set; }
        public string Tags { get; set; }

        public int VideoTypeId { get; set; }
        [ForeignKey("VideoTypeId")]
        public virtual VideoType VideoType { get; set; }

        public int PlaylistId { get; set; }
        [ForeignKey("PlaylistId")]
        public virtual Playlist Playlist { get; set; }

        public virtual ICollection<YoutubeVideoTranslation> YoutubeVideoTranslations { get; set; }
    }

    public class YoutubeVideoTranslation : TranslateBase
    {
        public int YoutubeVideoId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
