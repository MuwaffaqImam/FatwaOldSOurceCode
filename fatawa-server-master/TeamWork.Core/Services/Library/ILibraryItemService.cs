using System.Collections.Generic;
using TeamWork.Core.Models.Library;

namespace TeamWork.Core.IServices.Library
{
    public interface ILibraryItemService
    {
        List<LibraryItemModel> GetAllLibraryItems();
        List<LibraryItemModel> GetAllLibraryItems(string tags);
        int AddLibraryItem(LibraryItemModel model);
        bool UpdateLibraryItem(LibraryItemModel model);
        bool DeleteLibraryItem(int id);
        void ChangeLastSeen(int id);
        void IncreaseVistorsCount(int id);
    }
}
