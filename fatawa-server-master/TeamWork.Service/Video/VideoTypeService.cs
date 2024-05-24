using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Video;
using TeamWork.Core.Models.Video;
using TeamWork.Core.Repository;
using TeamWork.IService.Fatawa;

namespace TeamWork.Service.Fatawa
{
    public class VideoTypeService : IVideoTypeService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<VideoType> _videoTypeRepository;

        public VideoTypeService(IMapper imapper, IRepository<VideoType> videoTypeRepository)
        {
            try
            {
                _imapper = imapper;
                _videoTypeRepository = videoTypeRepository;

            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public List<VideoTypeModel> GetAllVideoTypes()
        {
            try
            {
                List<VideoType> videoTypes = _videoTypeRepository.GetAll();
                return _imapper.Map<List<VideoTypeModel>>(videoTypes);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public VideoTypeModel GetVideoTypeDetails(int id)
        {
            try
            {
                VideoType videoType = _videoTypeRepository.GetSingle(id);
                return _imapper.Map<VideoTypeModel>(videoType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddVideoType(VideoTypeModel model)
        {
            try
            {
                VideoType videoType = _imapper.Map<VideoType>(model);
                return _videoTypeRepository.Insert(videoType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateVideoType(VideoTypeModel model)
        {
            try
            {
                VideoType videoType = _videoTypeRepository.GetSingle(model.Id);
                return _videoTypeRepository.Update(videoType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteVideoType(int id)
        {
            try
            {
                VideoType videoType = _videoTypeRepository.GetSingle(id);
                return _videoTypeRepository.Delete(videoType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
