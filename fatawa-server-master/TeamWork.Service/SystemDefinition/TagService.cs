using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.IServices.SystemDefinition;
using TeamWork.Core.Models.SystemDefinition;
using TeamWork.Core.Repository;
using TeamWork.Repository.DataContext;

namespace TeamWork.Service.SystemDefinition
{
    internal class TagService : ITagService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<Tag> _tagRepository;
        private readonly IRepository<TagTranslation> _tagTranslationRepository;
        private readonly TeamDataContext _context;

        internal TagService(IMapper imapper, IRepository<Tag> tagRepository, IRepository<TagTranslation> tagTranslationRepository, TeamDataContext context)
        {
            _imapper = imapper;
            _tagRepository = tagRepository;
            _context = context;
            _tagTranslationRepository = tagTranslationRepository;
        }

        public List<TagModel> GetAllTags()
        {
            try
            {
                List<Tag> tags = _context.Tag.Include(s => s.TagTranslations).ToList();
                return _imapper.Map<List<TagModel>>(tags);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public TagModel GetTag(int id)
        {
            try
            {
                Tag tag = _context.Tag.Include(s => s.TagTranslations).FirstOrDefault(s => s.Id == id);
                return _imapper.Map<TagModel>(tag);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool AddTag(TagModel model)
        {
            try
            {
                Tag tag = _imapper.Map<Tag>(model);
                model.Id = _tagRepository.Insert(tag);
                if (model.Id > 0)
                {
                    AddTagTranslations(model);
                }

                return model.Id == 0 ? false : true;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateTage(TagModel model)
        {
            Tag tag = _tagRepository.GetSingle(model.Id);
            tag.Enabled = model.Enabled;
            
            return _tagRepository.Update(tag);
        }

        private bool AddTagTranslations(TagModel model)
        {
            return _tagTranslationRepository.Insert(new TagTranslation
            {
                LanguageCode = "EN",
                LanguageId = 2,
                TagName = model.TagName,
                TagId = model.Id,
            }) == 0 ? false : true;
        }

        public bool DeleteTag(int id)
        {
            try
            {
                bool isDeleted = false;
                Tag tag = _tagRepository.GetSingle(id);
                if (_tagRepository.Delete(tag))
                {
                    isDeleted = DeleteTagTranslations(id);
                }

                return isDeleted;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private bool DeleteTagTranslations(int id)
        {
            try
            {
                var tagTranslations = _tagTranslationRepository.GetWhere(s => s.TagId == id).ToList();
                return _tagTranslationRepository.DeleteRange(tagTranslations);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int GetCountRecord()
        {
            return _context.Tag.Count();
        }
    }
}
