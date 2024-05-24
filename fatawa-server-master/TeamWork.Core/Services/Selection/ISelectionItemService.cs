using System.Collections.Generic;
using TeamWork.Core.Models.Selection;

namespace TeamWork.Core.IServices.Selection
{
    public interface ISelectionItemService
    {
        List<SelectionItemModel> GetAllSelectionItems();
        List<SelectionItemModel> GetAllSelectionItems(string tags);
        SelectionItemModel GetSelectionItem(int id);
        int AddSelectionItem(SelectionItemModel model);
        bool UpdateSelectionItem(SelectionItemModel model);
        bool DeleteSelectionItem(SelectionItemModel model);
        void ChangeLastSeen(int id);
        void IncreaseVistorsCount(int id);
    }
}
