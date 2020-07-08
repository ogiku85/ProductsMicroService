using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ProductsMicroService.Data.Models;
using ProductsMicroService.Data.Utilities;

namespace ProductsMicroService.Repository.Repository
{
    public interface IGenericRepository<T> where T : class, IAudit
    {
        Task Add(T Entity);
        Task AddRange(List<T> Entities);
        void Delete(int Id);
        void Delete2(T Entity);
        void DeleteRange(IEnumerable<int> EntitiesId);
        void DeleteRange2(List<T> Entities);
        void DeleteRangeOld(IEnumerable<int> EntitiesId);
        IEnumerable<T> Find(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> FindAndIncludeRelatedEntities(Expression<Func<T, bool>> predicate, IEnumerable<string> entitiesToIncludes);
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetAllAsQuerable();
        IQueryable<T> GetAllAsQuerableAsNoTracking();
        Task<IEnumerable<T>> GetAll(QueryParameters queryParameters);
        Task<PagedData<T>> GetAllPagedData(QueryParameters queryParameters);

        Task<IEnumerable<T>> GetAllAndIncludeRelatedEntities(IEnumerable<string> entitiesToIncludes);
        Task<IEnumerable<T>> GetAllAndIncludeRelatedEntities(QueryParameters queryParameters, IEnumerable<string> entitiesToIncludes);
        Task<T> GetAsNoTrackingByID(int id);
        Task<T> GetAsNoTrackingByIDAndIncludeRelatedEntities(int id, IEnumerable<string> entitiesToIncludes);
        Task<T> GetByID(int id);
        Task<T> GetByIDAndIncludeRelatedEntities(int id, IEnumerable<string> entitiesToIncludes);
        void MarkAsDeleted(int Id);
        void MarkRangeAsDeleted(IEnumerable<int> EntitiesId);
        int Save();
        Task<int> SaveAsync();
        void Update(T Entity);
        void UpdateRange(IEnumerable<T> Entities);

        Task<PagedData<T>> GetAllPagedDataAndIncludeRelatedEntities(QueryParameters queryParameters, IEnumerable<string> entitiesToIncludes);
    }
}