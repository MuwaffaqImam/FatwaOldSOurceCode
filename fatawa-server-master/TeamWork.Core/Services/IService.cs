using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamWork.Core.Entity;
using TeamWork.Core.Repository;

namespace TeamWork.Core.Services
{
    public interface IService<T> : IDisposable where T : BaseEntity
    {
        List<T> GetAll();
        List<T> GetAll(int pageIndex, int pageSize);
        Task<List<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(int pageIndex, int pageSize);
        Task<IEnumerable<T>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<T, int>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        Task<IEnumerable<T>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<T, int>> keySelector, Expression<Func<T, bool>>[] predicate, OrderBy orderBy, params Expression<Func<T, object>>[] includeProperties);
        T GetSingle(int id);
        Task<T> GetSingleAsync(int id);
        void Insert(T entity);
        void InsertRange(List<T> ListEntities);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(List<T> ListEntities);
    }
}
