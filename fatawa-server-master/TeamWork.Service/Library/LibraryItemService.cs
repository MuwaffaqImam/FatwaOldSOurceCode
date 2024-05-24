using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using TeamWork.Core.Entity.Library;
using TeamWork.Core.IServices.Library;
using TeamWork.Core.Models;
using TeamWork.Core.Repository;
using TeamWork.Core.Models.Library;

namespace TeamWork.Core.Services.Library
{
    public class LibraryItemService : ILibraryItemService
    {
        private readonly IMapper _imapper;
        private readonly IRepository<LibraryItem> _libraryItemRepository;

        public LibraryItemService(IMapper imapper, IRepository<LibraryItem> libraryItemRepository)
        {
            _imapper = imapper;
            _libraryItemRepository = libraryItemRepository;
        }

        public List<LibraryItemModel> GetAllLibraryItems()
        {
            try
            {
                List<LibraryItem> libraryItems = _libraryItemRepository.GetAll();
                return _imapper.Map<List<LibraryItemModel>>(libraryItems);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<LibraryItemModel> GetAllLibraryItems(string tags)
        {
            try
            {
                IEnumerable<LibraryItem> libraryItems = _libraryItemRepository.GetAll().Where(a => a.Tags.Contains(tags));
                return _imapper.Map<List<LibraryItemModel>>(libraryItems);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddLibraryItem(LibraryItemModel model)
        {
            try
            {
                LibraryItem libraryItem = _imapper.Map<LibraryItem>(model);
                return _libraryItemRepository.Insert(libraryItem);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool UpdateLibraryItem(LibraryItemModel model)
        {
            try
            {
                LibraryItem libraryItem = _libraryItemRepository.GetSingle(model.Id);
                libraryItem.Order = model.Order;
                libraryItem.Tags = model.Tags;
                libraryItem.Version = model.Version;
                libraryItem.URLDownLoad = model.URLDownLoad;
                libraryItem.ExtensionType = model.ExtensionType;

                return _libraryItemRepository.Update(libraryItem);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public bool DeleteLibraryItem(int id)
        {
            try
            {
                LibraryItem libraryItem = _libraryItemRepository.GetSingle(id);
                return _libraryItemRepository.Delete(libraryItem);
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
                LibraryItem libraryItem = _libraryItemRepository.GetSingle(id);
                libraryItem.LastSeen = DateTime.UtcNow;
                _libraryItemRepository.Update(libraryItem);
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
                LibraryItem libraryItem = _libraryItemRepository.GetSingle(id);
                libraryItem.Visitors = libraryItem.Visitors + 1;
                _libraryItemRepository.Update(libraryItem);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
