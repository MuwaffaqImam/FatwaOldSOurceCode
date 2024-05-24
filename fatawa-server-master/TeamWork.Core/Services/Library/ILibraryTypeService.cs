using System.Collections.Generic;
using TeamWork.Core.Models.Library;

namespace TeamWork.IService.Library
{
    public interface ILibraryTypeService
    {
        List<LibraryTypeModel> GetAllLibraryTypes();
        int AddLibraryType(LibraryTypeModel model);
        bool UpdateLibraryType(LibraryTypeModel model);
        bool DeleteLibraryType(int id);
    }
}
