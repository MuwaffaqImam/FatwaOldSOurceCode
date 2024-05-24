using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TeamWork.Core.DTO;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Models.Notification;
using TeamWork.Core.Repository;
using TeamWork.Core.Sheard;
using TeamWork.IService.Fatawa;

namespace TeamWork.Service.Fatawa
{
    internal class FatawaService : IFatawaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _imapper;

        internal FatawaService(IUnitOfWork unitOfWork, IMapper imapper)
        {
            _unitOfWork = unitOfWork;
            _imapper = imapper;
        }

        public async Task<List<FatawaModel>> GetAllFatawas()
        {
            try
            {
                List<Core.Entity.Fatawa.Fatawa> fatawas = await _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().GetAllAsync();
                return _imapper.Map<List<FatawaModel>>(fatawas);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<FatawaModel>> GetAllFatawas(string tags)
        {
            try
            {
                List<Core.Entity.Fatawa.Fatawa> fatawas = await _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().GetAllAsync();
                return _imapper.Map<List<FatawaModel>>(fatawas);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FatawaModel> GetFatawaAsync(int id)
        {
            try
            {
                int currentLanguage = await _unitOfWork.FatawaRepository.GetUserLanguageId();
                return await GetFatawaByLanguageAsync(id, currentLanguage);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<FatawaModel> GetFatawaByLanguageAsync(int id, int languageId)
        {
            try
            {
                bool isUpdatedVisitore = false;
                FatawaModel fatawaModel = new FatawaModel();

                Core.Entity.Fatawa.Fatawa visitorFatawa = await _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().FindAsync(id);

                if (visitorFatawa != null)
                {
                    visitorFatawa.Visitors = visitorFatawa.Visitors + 1;
                    isUpdatedVisitore = _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().Update(visitorFatawa);
                }

                if (isUpdatedVisitore)
                {
                    fatawaModel = await _unitOfWork.FatawaRepository.GetFatawaByLanguageAsync(id, languageId);
                }

                return fatawaModel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddFatawa(FatawaModel model, int currentUserId)
        {
            try
            {
                Core.Entity.Fatawa.Fatawa fatawa = _imapper.Map<Core.Entity.Fatawa.Fatawa>(model);
                fatawa.ImageUrl = model.Url;
                fatawa.StatusId = model.StatusId.Value;
                fatawa.FatawaTranslations.Add(GetFatawaTranslationsModel(model));

                _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().Insert(fatawa);

                if (_unitOfWork.SaveChanges())
                {
                    _unitOfWork.NotificationRepository.AddNotificationItem(new NotificationItemModel
                    {
                        MessageText = "NewFatawaWasAdded",
                        IsRead = false,
                        Deleted = false,
                        NotificationTypeId = (int)SystemEnum.NotificationType.Fatawa,
                        RecipientRoleId = (int)SystemEnum.RolesType.SuperAdmin
                    });
                }

                if (model.QuestionId.HasValue)
                {
                    _unitOfWork.QuestionRepository.UpdateCloseQuestion((int)model.QuestionId, currentUserId);
                }

                FatawaModel fatawaModel = _imapper.Map<FatawaModel>(fatawa);
                return fatawaModel.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private FatawaTranslation GetFatawaTranslationsModel(FatawaModel model)
        {
            try
            {
                Language language = _unitOfWork.GetRepository<Language>().GetSingle(model.LanguageId);
                FatawaTranslation fatawaTranslation = new FatawaTranslation
                {
                    FatawaId = model.Id,
                    LanguageCode = language.LanguageCode,
                    LanguageId = language.Id,
                    Name = model.Name,
                    FatawaQuestion = model.FatawaQuestion,
                    Description = model.Description,
                    TagName = string.Join(";", model.Tags.Select(s => s.Name)),
                    TranslatorName = model.TranslatorName,
                };

                return fatawaTranslation;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private int AddTranslations(FatawaModel model)
        {
            Language language = _unitOfWork.GetRepository<Language>().GetSingle(model.LanguageId);
            FatawaTranslation fatawaTranslation = new FatawaTranslation
            {
                FatawaId = model.Id,
                LanguageCode = language.LanguageCode,
                LanguageId = language.Id,
                Name = model.Name,
                FatawaQuestion = model.FatawaQuestion,
                Description = model.Description,
                TagName = string.Join(";", model.Tags.Select(s => s.Name)),
                TranslatorName = model.TranslatorName,
            };

            _unitOfWork.GetRepository<FatawaTranslation>().Insert(fatawaTranslation);
            _unitOfWork.SaveChanges();

            return fatawaTranslation.Id;
        }

        public int UpdateFatawa(FatawaModel model)
        {
            try
            {
                int translateId = 0;
                Core.Entity.Fatawa.Fatawa fatawa = _unitOfWork.FatawaRepository.GetSingle(model.Id);
                fatawa.Order = model.Order;
                fatawa.ImageUrl = model.Url;
                fatawa.FatawaTypeId = model.FatawaTypeId;
                fatawa.FatawaMathhabId = model.FatawaMathhabId;
                fatawa.FatawaDepartmentId = model.FatawaDepartmentId;
                fatawa.MuftiId = model.MuftiId;
                fatawa.StatusId = model.StatusId.Value;

                _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().Update(fatawa);

                if (_unitOfWork.SaveChanges())
                {
                    translateId = UpdateTranslations(model);
                }

                if (translateId > 0 && model.StatusId == (int)Core.DTO.SystemEnum.FatawaStatusId.Approved)
                {
                    QuestionModel question = _unitOfWork.QuestionRepository.GetQuestionById((int)model.QuestionId).Result;

                    _unitOfWork.NotificationRepository.AddNotificationItem(new NotificationItemModel
                    {
                        MessageText = "NewFatawaWasPublished",
                        IsRead = false,
                        Deleted = false,
                        NotificationTypeId = (int)SystemEnum.NotificationType.Fatawa,
                        RecipientId = question.CreatedBy,
                        ReferenceMassageId = model.Id
                    });
                }
                return model.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool updateImportedFatawa(FatawaImportModel model)
        {
            try
            {
                List<Core.Entity.Fatawa.Fatawa> fatawaImported = _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().GetWhere(s => s.FatawaTypeId == 3).ToList();

                fatawaImported.ForEach(read =>
                {
                    read.FatawaDepartmentId = model.fatawaDepartmentId;
                    read.MuftiId = model.muftiId;
                    read.FatawaMathhabId = model.fatawaMathhabId;
                    read.FatawaTypeId = model.fatawaTypeId;
                });
                _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().UpdateRange(fatawaImported);

                _unitOfWork.SaveChanges();

                return true;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }
        private int UpdateTranslations(FatawaModel model)
        {
            FatawaTranslation entity = _unitOfWork.GetRepository<FatawaTranslation>().FirstOrDefault(s => s.FatawaId == model.Id && s.LanguageId == model.LanguageId);

            if (entity == null)
            {
                return AddTranslations(model);
            }

            entity.Name = model.Name;
            entity.FatawaQuestion = model.FatawaQuestion;
            entity.Description = model.Description;
            entity.TranslatorName = model.TranslatorName;
            entity.TagName = string.Join(";", model.Tags.Select(s => s.Name));

            _unitOfWork.GetRepository<FatawaTranslation>().Update(entity);
            _unitOfWork.SaveChanges();

            return model.Id;
        }

        public bool DeleteFatawa(int id)
        {
            try
            {
                Core.Entity.Fatawa.Fatawa fatawa = _unitOfWork.FatawaRepository.GetSingle(id);
                _unitOfWork.GetRepository<FatawaTranslation>().DeleteRange(_unitOfWork.GetRepository<FatawaTranslation>().GetWhere(ft => ft.FatawaId == id).ToList());
                _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().Delete(fatawa);

                return _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ChangeLastSeen(int id)
        {
            try
            {
                Core.Entity.Fatawa.Fatawa fatawa = _unitOfWork.FatawaRepository.GetSingle(id);
                fatawa.LastSeen = DateTime.UtcNow;
                _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().Update(fatawa);
                _unitOfWork.SaveChanges();
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
                Core.Entity.Fatawa.Fatawa fatawa = _unitOfWork.FatawaRepository.GetSingle(id);
                fatawa.Visitors = fatawa.Visitors + 1;
                _unitOfWork.GetRepository<Core.Entity.Fatawa.Fatawa>().Update(fatawa);
                _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<FatawaModel>> GetAllFatawaLiveAsync(int pageIndex, int pageSize, int CurrentUserId)
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetAllFatawaLiveAsync(pageIndex, pageSize, CurrentUserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> GetAllFatawaArchivedCount()
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetAllFatawaArchivedCount();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<List<FatawaModel>> GetAllFatawaArchivedAsync(int pageIndex, int pageSize)
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetAllFatawaArchivedAsync(pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> GetAllFatawaLiveCount()
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetAllFatawaLiveCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<PaginationRecord<FatawaModel>> GetFatawaFilteredAsync(int departmentId, int typeId, int pageIndex, int pageSize)
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaFilteredAsync(departmentId, typeId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task<PaginationRecord<FatawaExportModel>> getFatawaFilteredWithMathabandMufti(int departmentId, int typeId, int mathhabId, int muftiId, int pageIndex, int pageSize)
        {
            try
            {
                return _unitOfWork.FatawaRepository.getFatawaFilteredWithMathabandMufti(departmentId, typeId, mathhabId, muftiId, pageIndex, pageSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task<List<FatawaTypeModel>> GetFatawaTypesAsync()
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaTypesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Task<List<FatawaDepartmentModel>> GetFatawaDeparmentsAsync()
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaDeparmentsAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }

        public Task<List<FatawaMathhabModel>> GetFatawaMathhabsAsync()
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaMathhabsAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<StatusModel>> GetFatawaStatusAsync()
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaStatusAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<string>> GetTranslators()
        {
            return _unitOfWork.FatawaRepository.GetTranslators();
        }

        public Task<FatawaDefaultSettingModel> GetFatawaDefaultSettings(int id)
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaDefaultSettings(id);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<FatawaDefaultSettingModel> GetFatawaDefaultSettingsByUserAsync()
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaDefaultSettingsByUserAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int AddFatawaDefaultSettings(FatawaDefaultSettingModel model)
        {
            try
            {
                FatawaDefaultSetting fatawaDefaultSetting = _imapper.Map<FatawaDefaultSetting>(model);
                fatawaDefaultSetting.ImageUrl = model.Url;
                _unitOfWork.GetRepository<FatawaDefaultSetting>().Insert(fatawaDefaultSetting);

                if (_unitOfWork.SaveChanges())
                {
                    AddDefaultSettingTranslations(model);
                }
                return model.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int UpdateFatawaDefaultSettings(FatawaDefaultSettingModel model)
        {
            try
            {
                FatawaDefaultSetting fatawaDefaultSetting = _unitOfWork.GetRepository<FatawaDefaultSetting>().GetSingle(model.Id);
                fatawaDefaultSetting.ImageUrl = model.Url;
                fatawaDefaultSetting.FatawaTypeId = model.FatawaTypeId;
                fatawaDefaultSetting.FatawaMathhabId = model.FatawaMathhabId;
                fatawaDefaultSetting.FatawaDepartmentId = model.FatawaDepartmentId;
                fatawaDefaultSetting.MuftiId = model.MuftiId;

                _unitOfWork.GetRepository<FatawaDefaultSetting>().Update(fatawaDefaultSetting);

                if (_unitOfWork.SaveChanges())
                {
                    UpdateDefaultSettingTranslations(model);
                }
                return model.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool AddDefaultSettingTranslations(FatawaDefaultSettingModel model)
        {
            try
            {
                Language language = _unitOfWork.GetRepository<Language>().GetSingle(model.LanguageId);
                _unitOfWork.GetRepository<FatawaDefaultSettingTranslation>().Insert(new FatawaDefaultSettingTranslation
                {
                    FatawaDefaultSettingId = model.Id,
                    LanguageCode = language.LanguageCode,
                    LanguageId = language.Id,
                    Name = model.Name,
                    Description = model.Description,
                    TagName = model.Tags == null ? string.Empty : string.Join(";", model.Tags.Select(s => s.Name)),
                    TranslatorName = model.TranslatorName,
                });

                return _unitOfWork.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        private bool UpdateDefaultSettingTranslations(FatawaDefaultSettingModel model)
        {
            FatawaDefaultSettingTranslation entity = _unitOfWork.GetRepository<FatawaDefaultSettingTranslation>().
                                                        FirstOrDefault(s => s.FatawaDefaultSettingId == model.Id && s.LanguageId == model.LanguageId);
            if (entity == null)
            {
                return AddDefaultSettingTranslations(model);
            }
            entity.TagName = model.Tags == null ? string.Empty : string.Join(";", model.Tags.Select(s => s.Name));
            _unitOfWork.GetRepository<FatawaDefaultSettingTranslation>().Update(entity);
            return _unitOfWork.SaveChanges();
        }

        public async Task<List<FatawaDefaultSettingModel>> GetAllFatawasDefaultSettings(int pageIndex = 1, int pageSize = 10)
        {
            try
            {
                List<FatawaDefaultSetting> fatawaDefaultSettings = await _unitOfWork.GetRepository<FatawaDefaultSetting>().GetAllAsync();
                return _imapper.Map<List<FatawaDefaultSettingModel>>(fatawaDefaultSettings);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool DeleteFatawaDefaultSettings(int id)
        {
            try
            {
                FatawaDefaultSetting fatawaDefaultSetting = _unitOfWork.GetRepository<FatawaDefaultSetting>().GetSingle(id);
                _unitOfWork.GetRepository<FatawaDefaultSetting>().Delete(fatawaDefaultSetting);
                return _unitOfWork.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public Task<int> GetAllFatawasDefaultSettingsCount()
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetAllFatawasDefaultSettingsCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<PaginationRecord<FatawaModel>> GetClientFatawaFilteredAsync(string clientSearchText, int mathhabId, int fatwaTypeId, int pageIndex, int pageSize,bool SameTitle)
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetClientFatawaFilteredAsync(clientSearchText, mathhabId, fatwaTypeId, pageIndex, pageSize, SameTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task<List<FatawaModel>> GetFatawaRelations(int fatwaId, int languageId)
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaRelations(fatwaId, languageId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<Language>> GetFatawaLanguages(int fatwaId)
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaLanguages(fatwaId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<PaginationRecord<FatawaModel>> GetFatawaLiveAndArchivedFilteredAsync(int departmentId, string searchText, int typeId, int pageIndex, int pageSize,bool SameTitle)
        {
            try
            {
                return _unitOfWork.FatawaRepository.GetFatawaLiveAndArchivedFilteredAsync(departmentId, searchText, typeId, pageIndex, pageSize, SameTitle);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
