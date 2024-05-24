using System.Collections.Generic;
using TeamWork.Core.Entity;

namespace TeamWork.Core.Entity.Video
{
    public class Playlist : BaseEntity
    {
        public virtual ICollection<PlaylistTranslation> PlaylistTranslations { get; set; }
    }

    public class PlaylistTranslation : TranslateBase
    {
        public int PlaylistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
