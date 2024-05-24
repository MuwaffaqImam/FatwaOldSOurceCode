using AutoMapper;
using System.Collections.Generic;
using TeamWork.Core.Entity.Selection;
using TeamWork.Core.IServices.Selection;
using TeamWork.Core.Models.Selection;
using TeamWork.Core.Repository;

namespace TeamWork.Core.Services.Selection
{
    public class SelectionTypeService : ISelectionTypeService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<SelectionType> _selectionTypeRepository;

        public SelectionTypeService(IMapper imapper, IRepository<SelectionType> selectionTypeRepository)
        {
            _imapper = imapper;
            _selectionTypeRepository = selectionTypeRepository;
        }

        public List<SelectionTypeModel> GetAllSelectionTypes()
        {
            try
            {
                List<SelectionType> selectionTypes = _selectionTypeRepository.GetAll();
                return _imapper.Map<List<SelectionTypeModel>>(selectionTypes);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public SelectionTypeModel GetSelectionType(int id)
        {
            try
            {
                SelectionType selectionType = _selectionTypeRepository.GetSingle(id);
                return _imapper.Map<SelectionTypeModel>(selectionType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddSelectionType(SelectionTypeModel model)
        {
            try
            {
                SelectionType selectionType = _imapper.Map<SelectionType>(model);
                return _selectionTypeRepository.Insert(selectionType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateSelectionType(SelectionTypeModel model)
        {
            try
            {
                SelectionType selectionType = _selectionTypeRepository.GetSingle(model.Id);
                return _selectionTypeRepository.Update(selectionType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteSelectionType(SelectionTypeModel model)
        {
            try
            {
                SelectionType selectionType = _selectionTypeRepository.GetSingle(model.Id);
                return _selectionTypeRepository.Delete(selectionType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
