using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamWork.Core.Entity.Selection;
using TeamWork.Core.IServices.Selection;
using TeamWork.Core.Models.Selection;
using TeamWork.Core.Repository;

namespace TeamWork.Core.Services.Selection
{
    public class SelectionItemService : ISelectionItemService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<SelectionItem> _selectionItemRepository;

        public SelectionItemService(IMapper imapper, IRepository<SelectionItem> selectionItemRepository)
        {
            _imapper = imapper;
            _selectionItemRepository = selectionItemRepository;
        }

        public List<SelectionItemModel> GetAllSelectionItems()
        {
            try
            {
                List<SelectionItem> selectionItems = _selectionItemRepository.GetAll();
                return _imapper.Map<List<SelectionItemModel>>(selectionItems);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<SelectionItemModel> GetAllSelectionItems(string tags)
        {
            try
            {
                IEnumerable<SelectionItem> selectionItems = _selectionItemRepository.GetAll().Where(a => a.Tags.Contains(tags));
                return _imapper.Map<List<SelectionItemModel>>(selectionItems);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public SelectionItemModel GetSelectionItem(int id)
        {
            try
            {
                SelectionItem selectionItem = _selectionItemRepository.GetSingle(id);
                return _imapper.Map<SelectionItemModel>(selectionItem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddSelectionItem(SelectionItemModel model)
        {
            try
            {
                SelectionItem selectionItem = _imapper.Map<SelectionItem>(model);
                return _selectionItemRepository.Insert(selectionItem);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateSelectionItem(SelectionItemModel model)
        {
            try
            {
                SelectionItem selectionItem = _selectionItemRepository.GetSingle(model.Id);
                selectionItem.Order = model.Order;
                selectionItem.SelectionTypeId = model.SelectionTypeId;
                selectionItem.Tags = model.Tags;

                return _selectionItemRepository.Update(selectionItem);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteSelectionItem(SelectionItemModel model)
        {
            try
            {
                SelectionItem selectionItem = _selectionItemRepository.GetSingle(model.Id);
                return _selectionItemRepository.Delete(selectionItem);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public void ChangeLastSeen(int id)
        {
            try
            {
                SelectionItem selectionItem = _selectionItemRepository.GetSingle(id);
                selectionItem.LastSeen = DateTime.UtcNow;
                _selectionItemRepository.Update(selectionItem);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void IncreaseVistorsCount(int id)
        {
            try
            {
                SelectionItem selectionItem = _selectionItemRepository.GetSingle(id);
                selectionItem.Visitors = selectionItem.Visitors + 1;
                _selectionItemRepository.Update(selectionItem);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
