using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Entity.Fatawa;
using TeamWork.Core.Models.Fatawa;
using TeamWork.Core.Repository.ICustomRepsitory;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TeamWork.Repository.Repository.CustomRepository
{
    internal class FatawaDepartmentRepository : Repository<FatawaDepartment>, IFatawaDepartmentRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly IMapper _mapper;

        internal FatawaDepartmentRepository(IDbFactory dbFactory, IMapper mapper) : base(dbFactory)
        {
            _dbFactory = dbFactory;
            _mapper = mapper;
        }

        public async Task<List<FatawaDepartmentModel>> GetDepartmentsByLanguageId()
        {
            try
            {
                List<FatawaDepartmentModel> departments = await (from fd in _dbFactory.GetDataContext.FatawaDepartments
                                                                 join fdt in _dbFactory.GetDataContext.FatawaDepartmentTranslations
                                                                 on fd.Id equals fdt.FatawaDepartmentId
                                                                 where fdt.LanguageId == _dbFactory.GetDataContext.GetUserLanguageId().Result
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
                                                                     LanguageId = fdt.LanguageId,
                                                                     Sort = fd.Sort

                                                                 }).OrderBy(x => x.ParentId).ThenBy(y => y.Sort).ToListAsync();

                return departments;
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
                FatawaDepartment department = await _dbFactory.GetDataContext.FatawaDepartments.Include(x => x.FatawaDepartmentTranslations).FirstOrDefaultAsync(s => s.Id == departmentId);

                FatawaDepartmentModel fatawaDepartmentModel = _mapper.Map<FatawaDepartmentModel>(department);

                if (fatawaDepartmentModel != null)
                {
                    fatawaDepartmentModel.NodeName = department.FatawaDepartmentTranslations.FirstOrDefault(s => s.LanguageId == languageId)?.NodeName;
                }

                fatawaDepartmentModel.LanguageId = languageId;
                return fatawaDepartmentModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
