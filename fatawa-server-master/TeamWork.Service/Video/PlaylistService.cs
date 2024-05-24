using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Video;
using TeamWork.Core.Models.Video;
using TeamWork.Core.Repository;
using TeamWork.IService.Fatawa;

namespace TeamWork.Service.Fatawa
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<Playlist> _playlistRepository;

        public PlaylistService(IMapper imapper, IRepository<Playlist> playlistRepository)
        {
            _imapper = imapper;
            _playlistRepository = playlistRepository;
        }

        public List<PlaylistModel> GetAllPlaylists()
        {
            try
            {
                List<Playlist> playlists = _playlistRepository.GetAll();
                return _imapper.Map<List<PlaylistModel>>(playlists);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public PlaylistModel GetPlaylistDetails(int id)
        {
            try
            {
                Playlist playlist = _playlistRepository.GetSingle(id);
                return _imapper.Map<PlaylistModel>(playlist);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddPlaylist(PlaylistModel model)
        {
            try
            {
                Playlist playlist = _imapper.Map<Playlist>(model);
                return _playlistRepository.Insert(playlist);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdatePlaylist(PlaylistModel model)
        {
            try
            {
                Playlist playlist = _playlistRepository.GetSingle(model.Id);
                return _playlistRepository.Update(playlist);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeletePlaylist(int id)
        {
            try
            {
                Playlist playlist = _playlistRepository.GetSingle(id);
                return _playlistRepository.Delete(playlist);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
