using System.Collections.Generic;
using TeamWork.Core.Models.SystemDefinition;

namespace TeamWork.Core.IServices.SystemDefinition
{
    public interface ITagService
    {
        List<TagModel> GetAllTags();
        TagModel GetTag(int id);
        int GetCountRecord();
        bool AddTag(TagModel model);
        bool UpdateTage(TagModel model);
        bool DeleteTag(int id);
    }
}
