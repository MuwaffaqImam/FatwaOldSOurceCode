using System.Collections.Generic;
using TeamWork.Core.Models.Video;

namespace TeamWork.IService.Fatawa
{
    public interface IPlaylistService
    {
        List<PlaylistModel> GetAllPlaylists();
        PlaylistModel GetPlaylistDetails(int id);
        int AddPlaylist(PlaylistModel model);
        bool UpdatePlaylist(PlaylistModel model);
        bool DeletePlaylist(int id);
    }
}
