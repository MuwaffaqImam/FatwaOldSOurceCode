using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TeamWork.Core.DTO;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Entity.Security;
using TeamWork.Core.Entity.SystemDefinition;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Repository;
using TeamWork.Core.Repository.ICustomRepsitory;
using TeamWork.Core.Sheard;

namespace TeamWork.Repository.Repository.CustomRepository
{
    internal class FatawaRepository : Repository<Fatawa>, IFatawaRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IMapper _mapper;

        internal FatawaRepository(IDbFactory dbFactory, IMapper mapper) : base(dbFactory)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
        }

        public async Task<int> GetUserLanguageId()
        {
            try
            {
                return await _dbFactory.GetDataContext.GetUserLanguageId();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public async Task<FatawaModel> GetFatawaByLanguageAsync(int id, int languageId)
        {
            try
            {
                FatawaModel fatawaModel = new FatawaModel();

                Fatawa fatawa = await _dbFactory.GetDataContext.Fatawa.Include(s => s.FatawaTranslations)
                                          .Include(s => s.FatawaDepartment.FatawaDepartmentTranslations)
                                          .Include(s => s.FatawaMathhab.FatawaMathhabTranslations)
                                          .FirstOrDefaultAsync(s => s.Id == id);

                fatawaModel = _mapper.Map<FatawaModel>(fatawa);

                if (fatawaModel != null)
                {
                    var fatwaByUserLanguage = fatawa.FatawaTranslations.FirstOrDefault(s => s.LanguageId == languageId);
                    fatawaModel.Url = fatawa.ImageUrl;
                    fatawaModel.Name = fatwaByUserLanguage?.Name;
                    fatawaModel.FatawaQuestion = fatwaByUserLanguage?.FatawaQuestion;
                    fatawaModel.Description = fatwaByUserLanguage?.Description;
                    fatawaModel.MathhabName = fatawa.FatawaMathhab.FatawaMathhabTranslations.FirstOrDefault(s => s.LanguageId == languageId)?.Name;
                    fatawaModel.Visitors = fatawa.Visitors;
                    fatawaModel.UpdatedDate = fatawa.UpdatedDate;
                    fatawaModel.MuftiName = GetMuftiNameById(fatawa.MuftiId).Result;
                    fatawaModel.Tags = fatwaByUserLanguage?.TagName?.Split(";").Select(x => new TagModel
                    {
                        Name = x
                    }).ToList();
                }

                return fatawaModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<string> GetMuftiNameById(int userId)
        {
            try
            {
                User user = await _dbFactory.GetDataContext.Users.FindAsync(userId);

                return user != null ? user.FullName : string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<FatawaModel>> GetAllFatawaByTypeAsync(int fatawaType, int pageIndex, int pageSize, int CurrentUserId)
        {
            List<Core.Entity.Fatawa.Fatawa> fatawas = null;
            if (CurrentUserId != -1)
            {
                fatawas = await _dbFactory.GetDataContext.Fatawa
                               .Where(s => s.FatawaTypeId == fatawaType && s.MuftiId == CurrentUserId)
                               .Include(s => s.FatawaTranslations)
                               .Include(s => s.FatawaStatus.FatawaStatusTranslations)
                               .Include(s => s.FatawaDepartment.FatawaDepartmentTranslations)
                               .Include(s => s.FatawaMathhab.FatawaMathhabTranslations)
                               .OrderByDescending(x => x.Id)
                               .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                               .ToListAsync();
            }
            else
            {
                fatawas = await _dbFactory.GetDataContext.Fatawa
                             .Where(s => s.FatawaTypeId == fatawaType)
                             .Include(s => s.FatawaTranslations)
                             .Include(s => s.FatawaStatus.FatawaStatusTranslations)
                             .Include(s => s.FatawaDepartment.FatawaDepartmentTranslations)
                             .Include(s => s.FatawaMathhab.FatawaMathhabTranslations)
                             .OrderByDescending(x => x.Id)
                             .Skip(pageSize * (pageIndex - 1)).Take(pageSize)
                             .ToListAsync();
            }


            return fatawas.Select(x => new FatawaModel
            {
                Id = x.Id,
                FatwaId = x.Id,
                Name = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result)?.Name,
                StatusId = x.StatusId,
                StatusName = x.FatawaStatus.FatawaStatusTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).Name,
                FatawaQuestion = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result)?.FatawaQuestion,
                Description = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result)?.Description,
                FatawaDepartmentName = x.FatawaDepartment.FatawaDepartmentTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result)?.NodeName,
                MathhabName = x.FatawaMathhab.FatawaMathhabTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result)?.Name,
                LastSeen = x.LastSeen,
                CreatedDate = x.CreatedDate,
                QuestionId = x.QuestionId
            }).ToList();
        }

        public Task<List<FatawaModel>> GetAllFatawaLiveAsync(int pageIndex, int pageSize, int CurrentUserId)
        {
            try
            {
                return this.GetAllFatawaByTypeAsync((int)SystemEnum.FatawaType.FatawaLive, pageIndex, pageSize, CurrentUserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetAllFatawaArchivedCount()
        {
            try
            {
                return await _dbFactory.GetDataContext.Fatawa.CountAsync(s => s.FatawaTypeId == (int)SystemEnum.FatawaType.FatawaArchived);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<FatawaModel>> GetAllFatawaArchivedAsync(int pageIndex, int pageSize)
        {
            try
            {
                return this.GetAllFatawaByTypeAsync((int)SystemEnum.FatawaType.FatawaArchived, pageIndex, pageSize, -1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<int> GetAllFatawaLiveCount()
        {
            try
            {
                return await _dbFactory.GetDataContext.Fatawa.CountAsync(s => s.FatawaTypeId == (int)SystemEnum.FatawaType.FatawaLive);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaginationRecord<FatawaModel>> GetFatawaFilteredAsync(int departmentId, int typeId, int pageIndex, int pageSize, string searchText = "", bool justApproved = true, bool orderById = false, bool SameTitle = false)
        {
            try
            {
                
                
                if (searchText == null)
                    searchText = "";


                if (searchText.Contains("في"))
                {
                    searchText = searchText.Replace("في", "");

                }
                searchText = searchText.Replace("  ", " ");

                if (searchText != "")
                {
                    if (SameTitle == true)
                    {
                        searchText = searchText.Replace(" ", " and ");

                    }
                    else
                    {
                        searchText = searchText.Replace(" ", " or ");

                    }
                }

                var fatawaQuery = _dbFactory.GetDataContext.Fatawa.AsQueryable();

                fatawaQuery = fatawaQuery.Where(x => (departmentId == 0 || x.FatawaDepartmentId == departmentId) &&
                                                    (typeId == 0 || x.FatawaTypeId == typeId));

                if (justApproved)
                {
                    fatawaQuery = fatawaQuery.Where(x => x.StatusId == (int)Core.DTO.SystemEnum.FatawaStatusId.Approved);
                }

                if (!string.IsNullOrEmpty(searchText))
                {
                    fatawaQuery = fatawaQuery.Where(x => x.FatawaTranslations.Any(s => (EF.Functions.Contains(s.FatawaQuestion, searchText)) ||
                                                                                        (EF.Functions.Contains(s.Description, searchText)) ||
                                                                                        (EF.Functions.Contains(s.Name, searchText)) ||
                                                                                        (s.TagName.Contains(searchText)))
                                                                                 );
                }
                fatawaQuery = fatawaQuery.Include(s => s.FatawaTranslations)
               .Include(s => s.FatawaDepartment.FatawaDepartmentTranslations)
               .Include(s => s.FatawaMathhab.FatawaMathhabTranslations);

                fatawaQuery = orderById ? fatawaQuery.OrderByDescending(s => s.Id) : fatawaQuery.OrderByDescending(s => s.UpdatedDate).OrderByDescending(s => s.CreatedDate);

                var fatawTotalCount = fatawaQuery.Count();
                fatawaQuery = fatawaQuery.Paginate(pageIndex, pageSize);

                PaginationRecord<FatawaModel> paginationRecord = new PaginationRecord<FatawaModel>
                {
                    DataRecord = await fatawaQuery.Select(x => new FatawaModel
                    {
                        Id = x.Id,
                        FatwaId = x.Id,
                        Name = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).Name ?? x.FatawaTranslations.FirstOrDefault().Name,
                        FatawaQuestion = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).FatawaQuestion ?? x.FatawaTranslations.FirstOrDefault().FatawaQuestion,
                        Description = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).Description ?? x.FatawaTranslations.FirstOrDefault().Description,
                        LastSeen = x.LastSeen,
                        CreatedDate = x.CreatedDate,
                        UpdatedDate = x.UpdatedDate ?? x.CreatedDate,
                        FatawaDepartmentName = x.FatawaDepartment.FatawaDepartmentTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).NodeName ?? x.FatawaDepartment.FatawaDepartmentTranslations.FirstOrDefault().NodeName,
                        MathhabName = x.FatawaMathhab.FatawaMathhabTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).Name ?? x.FatawaMathhab.FatawaMathhabTranslations.FirstOrDefault().Name

                    }).ToListAsync(),

                    CountRecord = fatawTotalCount
                };

                return paginationRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FatawaTypeModel>> GetFatawaTypesAsync()
        {
            try
            {
                List<FatawaType> list = await _dbFactory.GetDataContext.FatawaTypes.Include(s => s.FatawaTypeTranslations).ToListAsync();
                return list.Select(x => new FatawaTypeModel
                {
                    Id = x.Id,
                    Name = x.FatawaTypeTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result)?.Name,
                }).ToList();
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
                return (from fd in _dbFactory.GetDataContext.FatawaDepartments
                        join fdt in _dbFactory.GetDataContext.FatawaDepartmentTranslations
                        on fd.Id equals fdt.FatawaDepartmentId
                        where fdt.LanguageId == GetUserLanguageId().Result
                        select new FatawaDepartmentModel
                        {
                            Id = fd.Id,
                            CreatedBy = fd.CreatedBy,
                            CreatedDate = fd.CreatedDate,
                            UpdatedBy = fd.UpdatedBy,
                            UpdatedDate = fd.UpdatedDate,
                            NodeNumber = fd.NodeNumber,
                            NodeMain = fd.NodeMain,
                            NodeLevelNumber = fd.NodeLevelNumber,
                            ParentId = fd.ParentId,
                            NodeName = fdt.NodeName,
                            FatawaDepartmentTranslateId = fdt.FatawaDepartmentId,
                            LanguageId = fdt.LanguageId

                        }).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<FatawaMathhabModel>> GetFatawaMathhabsAsync()
        {
            try
            {
                List<FatawaMathhab> list = await _dbFactory.GetDataContext.FatawaMathhab.Include(s => s.FatawaMathhabTranslations).ToListAsync();
                return list.Select(x => new FatawaMathhabModel
                {
                    Id = x.Id,
                    Name = x.FatawaMathhabTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result)?.Name,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<StatusModel>> GetFatawaStatusAsync()
        {
            try
            {
                var list = await _dbFactory.GetDataContext.FatawaStatus.Include(s => s.FatawaStatusTranslations).ToListAsync();
                return list.Select(x => new StatusModel
                {
                    Id = x.Id,
                    Name = x.FatawaStatusTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result)?.Name,
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<List<string>> GetTranslators()
        {
            try
            {
                return _dbFactory.GetDataContext.FatawaTranslation.Select(f => f.TranslatorName).Distinct().ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<FatawaDefaultSettingModel> GetFatawaDefaultSettings(int id)
        {
            try
            {
                FatawaDefaultSetting fatawaDefaultSetting = await _dbFactory.GetDataContext.FatawaDefaultSetting
                                     .Include(s => s.FatawaDefaultSettingTranslations)
                                     .Include(s => s.FatawaDepartment.FatawaDepartmentTranslations)
                                     .Include(s => s.FatawaStatus.FatawaStatusTranslations)
                                     .Include(s => s.FatawaMathhab.FatawaMathhabTranslations)
                                     .FirstOrDefaultAsync(s => s.Id == id);

                FatawaDefaultSettingModel fatawaDefaultSettingModel = _mapper.Map<FatawaDefaultSettingModel>(fatawaDefaultSetting);

                if (fatawaDefaultSettingModel != null)
                {
                    fatawaDefaultSettingModel.Url = fatawaDefaultSetting.ImageUrl;
                    fatawaDefaultSettingModel.Tags = fatawaDefaultSetting.FatawaDefaultSettingTranslations.FirstOrDefault()?.TagName?.Split(";").Select(x => new TagModel
                    {
                        Name = x
                    }).ToList();
                }

                return fatawaDefaultSettingModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<FatawaDefaultSettingModel> GetFatawaDefaultSettingsByUserAsync()
        {
            try
            {
                FatawaDefaultSetting fatawaDefaultSetting = await _dbFactory.GetDataContext.FatawaDefaultSetting
                                     .Include(s => s.FatawaDefaultSettingTranslations)
                                     .Include(s => s.FatawaDepartment.FatawaDepartmentTranslations)
                                     .Include(s => s.FatawaStatus.FatawaStatusTranslations)
                                     .Include(s => s.FatawaMathhab.FatawaMathhabTranslations)
                                     .FirstOrDefaultAsync(s => s.CreatedBy == _dbFactory.GetDataContext.UserId);

                FatawaDefaultSettingModel fatawaDefaultSettingModel = _mapper.Map<FatawaDefaultSettingModel>(fatawaDefaultSetting);

                if (fatawaDefaultSettingModel != null)
                {
                    fatawaDefaultSettingModel.Url = fatawaDefaultSetting.ImageUrl;

                    if (fatawaDefaultSetting.FatawaDefaultSettingTranslations.Count > 0)
                    {
                        fatawaDefaultSettingModel.Tags = fatawaDefaultSetting.FatawaDefaultSettingTranslations.
                                        Where(t => t.LanguageId == GetUserLanguageId().Result && !string.IsNullOrEmpty(t.TagName)).FirstOrDefault()?.TagName.Split(";").Select(x => new TagModel
                                        {
                                            Name = x
                                        }).ToList();
                    }
                }
                return fatawaDefaultSettingModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Task<int> GetAllFatawasDefaultSettingsCount()
        {
            try
            {
                return _dbFactory.GetDataContext.FatawaDefaultSetting.CountAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<PaginationRecord<FatawaModel>> GetClientFatawaFilteredAsync(string clientSearchText, int mathhabId, int fatwaTypeId, int pageIndex, int pageSize, bool SameTitle)
        {
            try
            {
                PaginationRecord<FatawaModel> paginationRecord = new PaginationRecord<FatawaModel>();

                if (clientSearchText != null)
                {
                    if (clientSearchText != "")
                    {

                        clientSearchText = clientSearchText.Replace(" ", " and ");

                    }
                }

                    
                List<FatawaModel> fatawaModels = await (from f in _dbFactory.GetDataContext.Fatawa
                                                        join ft in _dbFactory.GetDataContext.FatawaTranslation
                                                        on f.Id equals ft.FatawaId
                                                        where (mathhabId == 0 || f.FatawaMathhabId == mathhabId) && ft.LanguageId == GetUserLanguageId().Result &&
                                                        (f.StatusId == (int)Core.DTO.SystemEnum.FatawaStatusId.Approved) &&
                                                        (EF.Functions.Contains(ft.FatawaQuestion, clientSearchText) || EF.Functions.Contains(ft.Description, clientSearchText)
                                                        || EF.Functions.Contains(ft.Name, clientSearchText) || ft.TagName.Contains(clientSearchText))
                                                        where (fatwaTypeId == 0 || f.FatawaTypeId == fatwaTypeId)
                                                        select new FatawaModel
                                                        {
                                                            Id = f.Id,
                                                            CreatedBy = f.CreatedBy,
                                                            CreatedDate = f.CreatedDate,
                                                            UpdatedBy = f.UpdatedBy,
                                                            UpdatedDate = f.UpdatedDate,
                                                            Name = ft.Name,
                                                            FatawaQuestion = ft.FatawaQuestion,
                                                            Description = ft.Description,
                                                        }).ToListAsync();


                paginationRecord.CountRecord = fatawaModels.Count();
                paginationRecord.DataRecord = fatawaModels.Skip(pageSize * (pageIndex - 1)).Take(pageSize);

                return paginationRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        public async Task<List<TagModel>> GetFatawaTags(int fatawaId, int languageId)
        {
            try
            {
                Fatawa fatawa = await _dbFactory.GetDataContext.Fatawa.Include(s => s.FatawaTranslations)
                                          .FirstOrDefaultAsync(s => s.Id == fatawaId);

                FatawaTranslation fatwaByUserLanguage = fatawa.FatawaTranslations.FirstOrDefault(s => s.LanguageId == languageId);
                return fatwaByUserLanguage?.TagName?.Split(";").Select(x => new TagModel
                {
                    Name = x
                }).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<string> SplitTagsIfExist(string tagName)
        {
            if (string.IsNullOrEmpty(tagName) || string.IsNullOrWhiteSpace(tagName))
            {
                return new List<string>();
            }
            return tagName.Split(";").ToList();
        }

        public async Task<List<FatawaModel>> GetFatawaRelations(int fatwaId, int languageId)
        {
            try
            {
                List<TagModel> fatawaTags = await this.GetFatawaTags(fatwaId, languageId);
                string firstTag = fatawaTags.FirstOrDefault()?.Name;
                string lastTag = fatawaTags.LastOrDefault()?.Name;

                return (from f in _dbFactory.GetDataContext.Fatawa
                        join ft in _dbFactory.GetDataContext.FatawaTranslation
                        on f.Id equals ft.FatawaId
                        where (ft.FatawaId != fatwaId &&
                            ft.LanguageId == languageId &&
                        (!string.IsNullOrEmpty(firstTag) && ft.TagName.Contains(firstTag) ||
                         !string.IsNullOrEmpty(lastTag) && ft.TagName.Contains(lastTag)) &&
                        f.StatusId == (int)Core.DTO.SystemEnum.FatawaStatusId.Approved)
                        select new FatawaModel
                        {
                            Id = f.Id,
                            CreatedBy = f.CreatedBy,
                            CreatedDate = f.CreatedDate,
                            UpdatedBy = f.UpdatedBy,
                            UpdatedDate = f.UpdatedDate,
                            FatwaId = ft.Id,
                            Name = ft.Name,
                            FatawaQuestion = ft.FatawaQuestion,
                            Description = ft.Description,
                        }).Take(10).ToList();

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

                return _dbFactory.GetDataContext.FatawaTranslation.Where(x => x.FatawaId == fatwaId)
                                                                                .Include(x => x.Language)
                                                                                .Select(x => x.Language)
                                                                                .ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<PaginationRecord<FatawaModel>> GetFatawaLiveAndArchivedFilteredAsync(int departmentId, string searchText, int typeId, int pageIndex, int pageSize, bool SameTitle)
        {
            try
            {
                return this.GetFatawaFilteredAsync(departmentId, typeId, pageIndex, pageSize, searchText, false, true, SameTitle);
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
                return this.getFatawaFilteredWithMathabandMuftiAsync(departmentId, typeId, mathhabId, muftiId, pageIndex, pageSize, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<PaginationRecord<FatawaExportModel>> getFatawaFilteredWithMathabandMuftiAsync(int departmentId, int typeId, int mathhabId, int muftiId, int pageIndex, int pageSize, bool orderById = false)
        {
            try
            {
                var fatawaQuery = _dbFactory.GetDataContext.Fatawa.AsQueryable();

                fatawaQuery = fatawaQuery.Where(x => (departmentId == 0 || x.FatawaDepartmentId == departmentId) &&
                                                    (typeId == 0 || x.FatawaTypeId == typeId) &&
                                                    (mathhabId == 0 || x.FatawaMathhabId == mathhabId) &&
                                                    (muftiId == 0 || x.MuftiId == muftiId));


                fatawaQuery = fatawaQuery.Include(s => s.FatawaTranslations)
               .Include(s => s.FatawaDepartment.FatawaDepartmentTranslations)
               .Include(s => s.FatawaMathhab.FatawaMathhabTranslations);

                fatawaQuery = orderById ? fatawaQuery.OrderByDescending(s => s.Id) : fatawaQuery.OrderByDescending(s => s.UpdatedDate).OrderByDescending(s => s.CreatedDate);

                var fatawTotalCount = fatawaQuery.Count();
                fatawaQuery = fatawaQuery.Paginate(pageIndex, pageSize);

                PaginationRecord<FatawaExportModel> paginationRecord = new PaginationRecord<FatawaExportModel>
                {
                    DataRecord = await fatawaQuery.Select(x => new FatawaExportModel
                    {
                        FatwaId = x.Id,
                        Name = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).Name ?? x.FatawaTranslations.FirstOrDefault().Name,
                        FatawaQuestion = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).FatawaQuestion ?? x.FatawaTranslations.FirstOrDefault().FatawaQuestion,
                        Description = x.FatawaTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).Description ?? x.FatawaTranslations.FirstOrDefault().Description,
                        FatawaDepartmentName = x.FatawaDepartment.FatawaDepartmentTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).NodeName ?? x.FatawaDepartment.FatawaDepartmentTranslations.FirstOrDefault().NodeName,
                        MathhabName = x.FatawaMathhab.FatawaMathhabTranslations.FirstOrDefault(s => s.LanguageId == GetUserLanguageId().Result).Name ?? x.FatawaMathhab.FatawaMathhabTranslations.FirstOrDefault().Name,
                    }).ToListAsync(),

                    CountRecord = fatawTotalCount
                };

                return paginationRecord;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
