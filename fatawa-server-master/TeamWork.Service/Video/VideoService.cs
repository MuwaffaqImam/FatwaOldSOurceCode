using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamWork.Core.Entity.Video;
using TeamWork.Core.Models.Video;
using TeamWork.Core.Repository;
using TeamWork.IService.Video;

namespace TeamWork.Service.Video
{
    public class VideoService : IVideoService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<YoutubeVideo> _videoRepository;

        public VideoService(IMapper imapper, IRepository<YoutubeVideo> videoRepository)
        {
            _imapper = imapper;
            _videoRepository = videoRepository;
        }

        public List<YoutubeVideoModel> GetAllVideos()
        {
            try
            {
                List<YoutubeVideo> videos = _videoRepository.GetAll();
                return _imapper.Map<List<YoutubeVideoModel>>(videos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<YoutubeVideoModel> GetAllVideos(string tags)
        {
            try
            {
                IEnumerable<YoutubeVideo> videos = _videoRepository.GetAll().Where(a => a.Tags.Contains(tags));
                return _imapper.Map<List<YoutubeVideoModel>>(videos);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public YoutubeVideoModel GetVideoDetails(int id)
        {
            try
            {
                YoutubeVideo video = _videoRepository.GetSingle(id);
                return _imapper.Map<YoutubeVideoModel>(video);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int AddVideo(YoutubeVideoModel model)
        {
            try
            {
                YoutubeVideo video = _imapper.Map<YoutubeVideo>(model);
                return _videoRepository.Insert(video);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateVideo(YoutubeVideoModel model)
        {
            try
            {
                YoutubeVideo video = _videoRepository.GetSingle(model.Id);
                video.Order = model.Order;
                video.URL = model.URL;
                video.Tags = model.Tags;
                video.VideoTypeId = model.VideoTypeId;
                video.PlaylistId = model.PlaylistId;


                return _videoRepository.Update(video);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteVideo(int id)
        {
            try
            {
                YoutubeVideo video = _videoRepository.GetSingle(id);
                return _videoRepository.Delete(video);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void ChangeLastSeen(int id)
        {
            try
            {
                YoutubeVideo video = _videoRepository.GetSingle(id);
                video.LastSeen = DateTime.UtcNow;
                _videoRepository.Update(video);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void IncreaseVistorsCount(int id)
        {
            try
            {
                YoutubeVideo video = _videoRepository.GetSingle(id);
                video.Visitors = video.Visitors + 1;
                _videoRepository.Update(video);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
