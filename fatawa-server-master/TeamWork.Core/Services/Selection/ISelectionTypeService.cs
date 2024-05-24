using System.Collections.Generic;
using TeamWork.Core.Models.Selection;

namespace TeamWork.Core.IServices.Selection
{
    public interface ISelectionTypeService
    {
        List<SelectionTypeModel> GetAllSelectionTypes();
        SelectionTypeModel GetSelectionType(int id);
        int AddSelectionType(SelectionTypeModel model);
        bool UpdateSelectionType(SelectionTypeModel model);
        bool DeleteSelectionType(SelectionTypeModel model);
    }
}
