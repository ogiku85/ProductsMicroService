using System.Collections.Generic;
using System.Threading.Tasks;
using ProductsMicroService.Data.Models;
using ProductsMicroService.Data.Utilities;
using ProductsMicroService.Service.Utilities;

namespace ProductsMicroService.Service.Services
{
    public interface IGenericService<C, R, U, D>
        where C : class, IAudit, new()
        where R : class, IAudit, new()
        where U : class, IAudit, new()
        where D : class, IAudit, new()
    {
        Task<RequestActionResult<C>> Add(C entity);
        Task<RequestActionResult<IEnumerable<C>>> AddRange(List<C> entity);
        Task<RequestActionResult<IEnumerable<D>>> GetAll();
        Task<RequestActionResult<IEnumerable<D>>> GetAll(QueryParameters queryParameters);
        Task<RequestActionResult<PagedData<D>>> GetAllPagedData(QueryParameters queryParameters);
        Task<RequestActionResult<IEnumerable<D>>> GetAllAndIncludeRelatedEntities(IEnumerable<string> entitiesToIncludes);
        Task<RequestActionResult<IEnumerable<D>>> GetAllAndIncludeRelatedEntities(QueryParameters queryParameters, IEnumerable<string> entitiesToIncludes);
        Task<RequestActionResult<D>> GetAsNoTrackingByID(int id);
        Task<RequestActionResult<D>> GetAsNoTrackingByIDAndIncludeRelatedEntities(int id, IEnumerable<string> entitiesToIncludes);
        Task<RequestActionResult<D>> GetByID(int id);
        Task<RequestActionResult<D>> GetByIDAndIncludeRelatedEntities(int id, IEnumerable<string> entitiesToIncludes);
        Task<RequestActionResult<C>> MarkAsDeleted(int Id);
        Task<RequestActionResult<IEnumerable<C>>> MarkRangeAsDeleted(IEnumerable<int> Ids);
        Task<int> SaveAsync();
        Task<RequestActionResult<D>> Update(U entity);
        Task<RequestActionResult<IEnumerable<D>>> UpdateRange(IEnumerable<U> entity);

        Task<RequestActionResult<PagedData<D>>> GetAllPagedDataAndIncludeRelatedEntities(QueryParameters queryParameters, IEnumerable<string> entitiesToIncludes);
    }
}