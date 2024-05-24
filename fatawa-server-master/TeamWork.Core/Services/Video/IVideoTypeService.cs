using System.Collections.Generic;
using TeamWork.Core.Models.Video;

namespace TeamWork.IService.Fatawa
{
    public interface IVideoTypeService
    {
        List<VideoTypeModel> GetAllVideoTypes();
        VideoTypeModel GetVideoTypeDetails(int id);
        int AddVideoType(VideoTypeModel model);
        bool UpdateVideoType(VideoTypeModel model);
        bool DeleteVideoType(int id);
    }
}
