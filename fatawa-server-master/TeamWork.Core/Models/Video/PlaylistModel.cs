using System.Collections.Generic;

namespace TeamWork.Core.Models.Video
{
    public class PlaylistModel : BaseModel
    {
        public List<PlaylistTranslationModel> PlaylistTranslations { get; set; }
    }

    public class PlaylistTranslationModel : BaseTranslationModel
    {
        public int PlaylistId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
