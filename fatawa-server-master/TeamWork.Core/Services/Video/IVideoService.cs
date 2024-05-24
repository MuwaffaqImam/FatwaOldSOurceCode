using System.Collections.Generic;
using TeamWork.Core.Models.Video;

namespace TeamWork.IService.Video
{
    public interface IVideoService
    {
        List<YoutubeVideoModel> GetAllVideos();
        List<YoutubeVideoModel> GetAllVideos(string tags);
        YoutubeVideoModel GetVideoDetails(int id);
        int AddVideo(YoutubeVideoModel model);
        bool UpdateVideo(YoutubeVideoModel model);
        bool DeleteVideo(int id);
        void ChangeLastSeen(int id);
        void IncreaseVistorsCount(int id);
    }
}
