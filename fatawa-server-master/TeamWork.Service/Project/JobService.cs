using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TeamWork.Core.Entity;
using TeamWork.Core.IServices.Project;
using TeamWork.Core.Models.Project;
using TeamWork.Core.Repository;
using TeamWork.Core.Sheard;

namespace TeamWork.Service.Project
{
    internal class JobService : IJobService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        internal JobService(IUnitOfWork unitOfWork, IMapper imapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = imapper;
        }

        public async Task<List<JobModel>> GetAllAsync()
        {
            try
            {
                List<Job> job = await _unitOfWork.GetRepository<Job>().GetAllAsync();
                List<JobModel> jobModelList = _mapper.Map<List<JobModel>>(job);
                return jobModelList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<PaginationRecord<JobModel>> GetAllAsync(int pageIndex, int pageSize)
        {
            try
            {
                PaginationRecord<Job> job = await _unitOfWork.GetRepository<Job>().GetAllAsync(pageIndex, pageSize, x => x.Id, OrderBy.Descending);
                PaginationRecord<JobModel> paginationRecordModel = new PaginationRecord<JobModel>
                {
                    DataRecord = _mapper.Map<IEnumerable<JobModel>>(job.DataRecord),
                    CountRecord = job.CountRecord,
                };
                return paginationRecordModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetCountRecord()
        {
            try
            {
                return _unitOfWork.GetRepository<Job>().GetTotalCount();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(JobModel jobModel)
        {
            int id = 0;
            try
            {
                Job job = _mapper.Map<Job>(jobModel);
                if (job != null)
                {
                    _unitOfWork.GetRepository<Job>().Insert(job);
                    _unitOfWork.SaveChanges();
                }

                return job.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(JobModel jobModel)
        {
            try
            {
                Job job = _unitOfWork.GetRepository<Job>().GetSingle(jobModel.Id);

                if (job != null)
                {
                    job.JobName = jobModel.JobName;
                    _unitOfWork.GetRepository<Job>().Update(job);
                    _unitOfWork.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(int id)
        {
            try
            {
                Job job = _unitOfWork.GetRepository<Job>().GetSingle(id);
                if (job != null)
                {
                    _unitOfWork.GetRepository<Job>().Delete(job);
                    _unitOfWork.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

