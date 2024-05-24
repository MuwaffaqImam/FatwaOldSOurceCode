using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.Models.DragDrop;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Repository;
using TeamWork.IService.Fatawa;

namespace TeamWork.Service.Fatawa
{
    internal class FatawaDepartmentService : IFatawaDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _imapper;

        internal FatawaDepartmentService(IUnitOfWork unitOfWork, IMapper imapper)
        {
            _unitOfWork = unitOfWork;
            _imapper = imapper;
        }

        public async Task<List<FatawaDepartmentModel>> GetDepartmentsByLanguageId()
        {
            try
            {
                return await _unitOfWork.FatawaDepartmentRepository.GetDepartmentsByLanguageId();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FatawaDepartmentModel> GetFatawaDepartment(int id)
        {
            try
            {
                FatawaDepartment department = await _unitOfWork.GetRepository<FatawaDepartment>().GetSingleAsync(id);
                return _imapper.Map<FatawaDepartmentModel>(department);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FatawaDepartmentModel> GetFatawaDepartmentBylanguage(int departmentId, int languageId)
        {
            try
            {
                return await _unitOfWork.FatawaDepartmentRepository.GetFatawaDepartmentBylanguage(departmentId, languageId);
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        public int AddFatawaDepartment(FatawaDepartmentModel model)
        {
            try
            {
                FatawaDepartment fatawaDepartment = _unitOfWork.GetRepository<FatawaDepartment>().GetSingle(model.ParentId);
                FatawaDepartment fatawaDepartmentLastCreatedNode = _unitOfWork.GetRepository<FatawaDepartment>().GetWhere(x => x.ParentId == fatawaDepartment.Id).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                FatawaDepartment fatawaDepartmentLastSortNode = _unitOfWork.GetRepository<FatawaDepartment>().GetWhere(x => x.ParentId == fatawaDepartment.Id).OrderByDescending(o => o.Sort).FirstOrDefault();

                if (fatawaDepartmentLastCreatedNode != null && fatawaDepartmentLastCreatedNode.NodeLevelNumber != 0)
                {
                    int lengthMain = fatawaDepartmentLastCreatedNode.NodeMain.Length;
                    string siblingNodeNumber = fatawaDepartmentLastCreatedNode.NodeNumber;
                    string mainNumber = siblingNodeNumber.Substring(0, lengthMain);
                    string childNumber = siblingNodeNumber.Substring(lengthMain);

                    model.NodeNumber = mainNumber + (Convert.ToInt32(childNumber) + 1);
                    model.NodeMain = fatawaDepartmentLastCreatedNode.NodeMain;
                    model.Sort = fatawaDepartmentLastSortNode.Sort + 1;
                }
                else
                {
                    model.NodeNumber = fatawaDepartment.NodeNumber + "1";
                    model.NodeMain = fatawaDepartment.NodeNumber;
                    model.Sort = 0;
                }
                FatawaDepartment department = _imapper.Map<FatawaDepartment>(model);
                _unitOfWork.GetRepository<FatawaDepartment>().Insert(department);

                if (_unitOfWork.SaveChanges())
                {
                    model.Id = department.Id;
                    AddTranslations(model);
                }

                return department.Id;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private bool AddTranslations(FatawaDepartmentModel model)
        {
            try
            {
                Language language = _unitOfWork.GetRepository<Language>().GetSingle(model.LanguageId);

                FatawaDepartmentTranslation fatawaDepartmentTranslation = new FatawaDepartmentTranslation
                {
                    FatawaDepartmentId = model.Id,
                    NodeName = model.NodeName,
                    LanguageId = model.LanguageId,
                    LanguageCode = language.LanguageCode,
                };

                _unitOfWork.GetRepository<FatawaDepartmentTranslation>().Insert(fatawaDepartmentTranslation);
                return _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool UpdateFatawaDepartment(FatawaDepartmentModel model)
        {
            try
            {
                bool updateTransaction = false;
                FatawaDepartment department = _unitOfWork.GetRepository<FatawaDepartment>().GetSingle(model.Id);
                department.ParentId = model.ParentId;
                _unitOfWork.GetRepository<FatawaDepartment>().Update(department);

                if (_unitOfWork.SaveChanges())
                {
                    updateTransaction = UpdateTranslations(model);
                }

                return updateTransaction;
            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        private bool UpdateTranslations(FatawaDepartmentModel model)
        {
            try
            {
                FatawaDepartmentTranslation departmentTranslation = _unitOfWork.GetRepository<FatawaDepartmentTranslation>().FirstOrDefault(s => s.FatawaDepartmentId == model.FatawaDepartmentTranslateId && s.LanguageId == model.LanguageId);

                if (departmentTranslation == null)
                {
                    return AddTranslations(model);
                }

                FatawaDepartmentTranslation fatawaDepartmentTranslation = _unitOfWork.GetRepository<FatawaDepartmentTranslation>().FirstOrDefault(s => s.FatawaDepartmentId == model.FatawaDepartmentTranslateId && s.LanguageId == model.LanguageId);
                fatawaDepartmentTranslation.NodeName = model.NodeName;
                _unitOfWork.GetRepository<FatawaDepartmentTranslation>().Update(fatawaDepartmentTranslation);

                return _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool DeleteFatawaDepartment(int id)
        {
            try
            {
                FatawaDepartment department = _unitOfWork.GetRepository<FatawaDepartment>().GetSingle(id);

                List<FatawaDepartment> childrenfatawaDepartments = _unitOfWork.GetRepository<FatawaDepartment>().GetWhere(x => x.ParentId == department.Id).ToList();

                bool isDeleted = false;

                if (department.NodeLevelNumber == 0 || childrenfatawaDepartments.Count > 0)
                {
                    return false;
                }

                if (department != null)
                {
                    isDeleted = DeleteTranslations(id);

                    if (isDeleted)
                    {
                        _unitOfWork.GetRepository<FatawaDepartment>().Delete(department);
                        isDeleted = _unitOfWork.SaveChanges();
                    }
                }

                return isDeleted;
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        private bool DeleteTranslations(int id)
        {
            try
            {
                bool isDeleted = false;
                var translations = _unitOfWork.GetRepository<FatawaDepartmentTranslation>().GetWhere(a => a.FatawaDepartmentId == id);

                if (translations.Any())
                {
                    _unitOfWork.GetRepository<FatawaDepartmentTranslation>().DeleteRange(translations.ToList());
                    isDeleted = _unitOfWork.SaveChanges();
                }

                return isDeleted;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<FatawaDepartmentModel>> GetDepartmentsByLevelId(int levelNo)
        {
            try
            {
                List<FatawaDepartment> fatawaDepartments = await _unitOfWork.GetRepository<FatawaDepartment>()
                    .GetAllIncludingWithPredicate(new Expression<Func<FatawaDepartment, object>>[] { x => x.FatawaDepartmentTranslations }, new Expression<Func<FatawaDepartment, bool>>[] { x => x.NodeLevelNumber == levelNo });
                return _imapper.Map<List<FatawaDepartmentModel>>(fatawaDepartments);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<FatawaDepartmentModel>> GetDepartmentsByDepartmentIdLevelId(int depId, int levelNo)
        {
            try
            {
                List<FatawaDepartment> fatawaDepartments = await _unitOfWork.GetRepository<FatawaDepartment>()
                    .GetAllIncludingWithPredicate(new Expression<Func<FatawaDepartment, object>>[] { x => x.FatawaDepartmentTranslations }, new Expression<Func<FatawaDepartment, bool>>[] { x => x.NodeLevelNumber == levelNo && x.ParentId == depId });
                return _imapper.Map<List<FatawaDepartmentModel>>(fatawaDepartments);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool UpdateDropDepartment(DragDropModel dragDropModel)
        {
            try
            {
                FatawaDepartment previousDepartmentNode = _unitOfWork.GetRepository<FatawaDepartment>().GetSingle(dragDropModel.PreviousNodeId);
                FatawaDepartment currentDepartmentNode = _unitOfWork.GetRepository<FatawaDepartment>().GetSingle(dragDropModel.CurrentNodeId);
                FatawaDepartment currentParentNode = _unitOfWork.GetRepository<FatawaDepartment>().GetSingle(currentDepartmentNode.ParentId);
                List<FatawaDepartment> currentMovedNodes = _unitOfWork.GetRepository<FatawaDepartment>().GetWhere(x => (x.ParentId == currentParentNode.Id) && (x.Id != previousDepartmentNode.Id) && (x.NodeMain != "0")).OrderBy(o => o.Sort).ToList();

                int indexOfCurrent = currentMovedNodes.FindIndex((x) => x.Id == currentDepartmentNode.Id);
                //chek if node moved from up to down or down to up.
                currentMovedNodes.Insert(dragDropModel.IsMoveToDown ? indexOfCurrent + 1 : indexOfCurrent, previousDepartmentNode);
                int indexUpdateNode = 0;

                foreach (FatawaDepartment currentDepartmentMovedNode in currentMovedNodes)
                {
                    currentDepartmentMovedNode.Sort = indexUpdateNode++;
                }
                _unitOfWork.GetRepository<FatawaDepartment>().UpdateRange(currentMovedNodes);
                return _unitOfWork.SaveChanges();
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}