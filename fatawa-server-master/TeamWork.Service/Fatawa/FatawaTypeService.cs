using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Repository;
using TeamWork.IService.Fatawa;

namespace TeamWork.Service.Fatawa
{
    internal class FatawaTypeService : IFatawaTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _imapper;

        internal FatawaTypeService(IUnitOfWork unitOfWork, IMapper imapper)
        {
            _unitOfWork = unitOfWork;
            _imapper = imapper;
        }

        public async Task<List<FatawaTypeModel>> GetAllFatawaTypes()
        {
            try
            {
                List<FatawaType> fatawaTypes = await _unitOfWork.GetRepository<FatawaType>().GetAllAsync();
                return _imapper.Map<List<FatawaTypeModel>>(fatawaTypes);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<FatawaTypeModel> GetFatawaType(int id)
        {
            try
            {
                FatawaType fatawaType = await _unitOfWork.GetRepository<FatawaType>().GetSingleAsync(id);
                return _imapper.Map<FatawaTypeModel>(fatawaType);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddFatawaType(FatawaTypeModel model)
        {
            try
            {
                FatawaType fatawaType = _imapper.Map<FatawaType>(model);
                _unitOfWork.GetRepository<FatawaType>().Insert(fatawaType);
                _unitOfWork.SaveChanges();

                return fatawaType.Id;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateFatawaType(FatawaTypeModel model)
        {
            try
            {
                FatawaType fatawaType = await _unitOfWork.GetRepository<FatawaType>().GetSingleAsync(model.Id);
                _unitOfWork.GetRepository<FatawaType>().Update(fatawaType);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteFatawaType(int id)
        {
            try
            {
                FatawaType fatawaType = await  _unitOfWork.GetRepository<FatawaType>().GetSingleAsync(id);
                _unitOfWork.GetRepository<FatawaType>().Delete(fatawaType);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
