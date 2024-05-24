using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TeamWork.Core.Entity;
using TeamWork.Core.Sheard;

namespace TeamWork.Core.Repository
{
    public interface IRepository<T> : IDisposable where T : BaseEntity
    {
        List<T> GetAll();
        Task<List<T>> GetAllAsync();
        //List<T> GetAllAsync();
        PaginationRecord<T> GetAll(int pageIndex, int pageSize);
        Task<PaginationRecord<T>> GetAllAsync(int pageIndex, int pageSize);
        Task<PaginationRecord<T>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<T, int>> keySelector, OrderBy orderBy = OrderBy.Ascending);
        Task<PaginationRecord<T>> GetAllAsync(int pageIndex, int pageSize, Expression<Func<T, int>> keySelector, Expression<Func<T, bool>>[] predicate, OrderBy orderBy, params Expression<Func<T, object>>[] includeProperties);
        T GetSingle(int id);
        T FirstOrDefault(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetWhere(Expression<Func<T, bool>> predicate);
        Task<T> GetSingleAsync(int id);
        bool Update(T entity);
        bool UpdateRange(List<T> entity);
        int Insert(T entity);
        bool InsertRange(List<T> listEntities);
        bool Delete(T entity);
        bool DeleteRange(List<T> listEntities);
        Task<List<T>> GetAllIncludingWithPredicate(Expression<Func<T, object>>[] includeProperties, Expression<Func<T, bool>>[] predicateProperties);
        Task<List<T>> GetAllIncludingAsync(params Expression<Func<T, object>>[] includeProperties);
        int GetTotalCount();
        bool IsContains(Expression<Func<T, bool>>[] predicateProperties);
        Task<T> FindAsync(int id);
    }
}
