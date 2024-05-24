using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Library;
using TeamWork.Core.Models.Library;
using TeamWork.Core.Repository;
using TeamWork.IService.Library;

namespace TeamWork.Service.Fatawa
{
    public class LibraryTypeService : ILibraryTypeService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<LibraryType> _libraryTypeRepository;

        public LibraryTypeService(IMapper imapper, IRepository<LibraryType> libraryTypeRepository)
        {
            _imapper = imapper;
            _libraryTypeRepository = libraryTypeRepository;
        }

        public List<LibraryTypeModel> GetAllLibraryTypes()
        {
            try
            {
                List<LibraryType> libraryTypes = _libraryTypeRepository.GetAll();
                return _imapper.Map<List<LibraryTypeModel>>(libraryTypes);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddLibraryType(LibraryTypeModel model)
        {
            try
            {
                LibraryType libraryType = _imapper.Map<LibraryType>(model);
                return _libraryTypeRepository.Insert(libraryType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateLibraryType(LibraryTypeModel model)
        {
            try
            {
                LibraryType libraryType = _libraryTypeRepository.GetSingle(model.Id);
                return _libraryTypeRepository.Update(libraryType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteLibraryType(int id)
        {
            try
            {
                LibraryType libraryType = _libraryTypeRepository.GetSingle(id);
                return _libraryTypeRepository.Delete(libraryType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
