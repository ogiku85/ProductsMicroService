using ProductsMicroService.Service.Utilities;
using ProductsMicroService.Data.Models;
using ProductsMicroService.Data.Utilities;
using ProductsMicroService.Repository.Repository;
using ProductsMicroService.Service.Factories;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductsMicroService.Service.Services
{
    /// <summary>
    /// C represents the Create Entity
    /// R represents the Read Entity
    /// U represents the Update Entity
    /// D represents the Default Entity
    /// This all are all the given state of any entity. Sometimes you don't need all fields of an enity
    /// to be editable therefore the create and update entity will be represnted by diffent classes
    /// </summary>
    /// <typeparam name="C"></typeparam>
    /// <typeparam name="R"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="D"></typeparam>
    public class GenericService<C,R,U,D> : IGenericService<C, R, U, D>
        where C : class, IAudit, new()
        where R : class, IAudit, new()
        where U : class, IAudit, new()
        where D : class, IAudit, new()
    {
        private GenericRepository<D> _repository;
        private IGenericObjectFactory<D, C> _factory;
        private IGenericObjectFactory<D, U> _factoryForUpdate;
        public GenericService(GenericRepository<D> repository, 
            IGenericObjectFactory<D, C> factory,
            IGenericObjectFactory<D, U> factoryForUpdate)
        {
            _repository = repository;
            _factory = factory;
            _factoryForUpdate = factoryForUpdate;
            Log.Information("Inside GenericService Constructor");
            Log.Information("{@repositoryType}", repository.GetType());
        }

        public  async Task<RequestActionResult<C>> Add(C entity)
        {
          //  var C = new C();
            var D = new D();
            try
            {
                Log.Information("Inside Add");
                Log.Information("{@entity}", entity);

                D = _factory.CreateGenericObject(entity) as D;
                // await _repository.Add(entity);
                Log.Information("{@D}", D);
                await _repository.Add(D);
                var result = await _repository.SaveAsync();
                if (result > 0)
                {
                    entity = _factory.CreateGenericObjectDTO(D) as C;
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<C>(entity, ActionStatus.Created));

                    return new RequestActionResult<C>(entity, ActionStatus.Created);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in Add");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<C>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<C>(entity, ActionStatus.NothingModified));

            return new RequestActionResult<C>(entity, ActionStatus.NothingModified);

        }

        public  async Task<RequestActionResult<IEnumerable<C>>> AddRange(List<C> entity)
        {
           // var CList = new List<C>();
            var DList = new List<D>();
            try
            {
                Log.Information("Inside AddRange");
                Log.Information("{@entity}", entity);

                DList = _factory.CreateGenericObjectList(entity) as List<D>;
                // await _repository.Add(entity);
                Log.Information("{@DList}", DList);

                await _repository.AddRange(DList);
                var result = await _repository.SaveAsync();
                if (result > 0)
                {
                    entity = _factory.CreateGenericObjectDTOList(DList) as List<C>;
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.Created));

                    return new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.Created);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in AddRange");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.NothingModified));

            return new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.NothingModified);

        }


        public async Task<RequestActionResult<IEnumerable<D>>> GetAll()
        {
            var entity = new List<D>();
            try
            {
                Log.Information("Inside GetAll");
                var resultTemp = await _repository.GetAll();
                entity = resultTemp.ToList();
                if (entity.Count > 0)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NothingModified));

                    return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NothingModified);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetAll");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NotFound));

            return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NotFound);

        }

        public async Task<RequestActionResult<IEnumerable<D>>> GetAll(QueryParameters queryParameters)
        {
            var entity = new List<D>();
            try
            {
                Log.Information("Inside GetAll");
                var resultTemp = await _repository.GetAll(queryParameters);
                entity = resultTemp.ToList();
                if (entity.Count > 0)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NothingModified));

                    return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NothingModified);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetAll");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NotFound));

            return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NotFound);

        }
        public async Task<RequestActionResult<PagedData<D>>> GetAllPagedData(QueryParameters queryParameters)
        {
            var entity =  new PagedData<D>();
            try
            {
                Log.Information("Inside GetAll");
                entity = await _repository.GetAllPagedData(queryParameters);
              
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetAllPagedData");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<PagedData<D>>(entity, ActionStatus.Error, ex);

            }

            Log.Information("Result {@RequestActionResult}", new RequestActionResult<PagedData<D>>(entity, ActionStatus.NothingModified));

            return new RequestActionResult<PagedData<D>>(entity, ActionStatus.NothingModified);

        }
        
          public async Task<RequestActionResult<PagedData<D>>> GetAllPagedDataAndIncludeRelatedEntities(QueryParameters queryParameters, IEnumerable<string> entitiesToIncludes)
        {
            var entity = new PagedData<D>();
            try
            {
                Log.Information("Inside GetAllPagedDataAndIncludeRelatedEntities");
                entity = await _repository.GetAllPagedDataAndIncludeRelatedEntities(queryParameters, entitiesToIncludes);

            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetAllPagedDataAndIncludeRelatedEntities");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<PagedData<D>>(entity, ActionStatus.Error, ex);

            }

            Log.Information("Result {@RequestActionResult}", new RequestActionResult<PagedData<D>>(entity, ActionStatus.NothingModified));

            return new RequestActionResult<PagedData<D>>(entity, ActionStatus.NothingModified);

        }
        public async Task<RequestActionResult<D>> GetByID(int id)
        {
            D entity = null;

            try
            {
                Log.Information("Inside GetByID");
                Log.Information("{@id}", id);
                //  var result = await _repository.GetByIDAsyncNonDeletedEntity(id);
                entity = await _repository.GetByID(id);

                // entity = result;
                if (entity != null)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(entity, ActionStatus.NothingModified));

                    return new RequestActionResult<D>(entity, ActionStatus.NothingModified);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetByID");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<D>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(entity, ActionStatus.NotFound));

            return new RequestActionResult<D>(entity, ActionStatus.NotFound);

        }

        public async Task<RequestActionResult<D>> GetAsNoTrackingByID(int id)
        {
            D entity = null;

            try
            {
                Log.Information("Inside GetAsNoTrackingByID");
                Log.Information("{@id}", id);
                entity = await _repository.GetAsNoTrackingByID(id);

                if (entity != null)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(entity, ActionStatus.NothingModified));

                    return new RequestActionResult<D>(entity, ActionStatus.NothingModified);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetAsNoTrackingByID");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<D>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(entity, ActionStatus.NotFound));

            return new RequestActionResult<D>(entity, ActionStatus.NotFound);

        }


        //new

        public async Task<RequestActionResult<IEnumerable<D>>> GetAllAndIncludeRelatedEntities(IEnumerable<string> entitiesToIncludes)
        {
            var entity = new List<D>();
            try
            {
                Log.Information("Inside GetAllAndIncludeRelatedEntities");
                var resultTemp = await _repository.GetAllAndIncludeRelatedEntities(entitiesToIncludes);
                entity = resultTemp.ToList();
                if (entity.Count > 0)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NothingModified));

                    return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NothingModified);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetAllAndIncludeRelatedEntities");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NotFound));

            return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NotFound);

        }

        public async Task<RequestActionResult<IEnumerable<D>>> GetAllAndIncludeRelatedEntities(QueryParameters queryParameters, IEnumerable<string> entitiesToIncludes)
        {
            var entity = new List<D>();
            try
            {
                Log.Information("Inside GetAllAndIncludeRelatedEntities");
                var resultTemp = await _repository.GetAllAndIncludeRelatedEntities(queryParameters, entitiesToIncludes);
                entity = resultTemp.ToList();
                if (entity.Count > 0)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NothingModified));

                    return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NothingModified);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetAllAndIncludeRelatedEntities");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NotFound));

            return new RequestActionResult<IEnumerable<D>>(entity, ActionStatus.NotFound);

        }
        public async Task<RequestActionResult<D>> GetByIDAndIncludeRelatedEntities(int id, IEnumerable<string> entitiesToIncludes)
        {
            D entity = null;

            try
            {
                Log.Information("Inside GetByIDAndIncludeRelatedEntities");
                Log.Information("{@id}", id);
                //  var result = await _repository.GetByIDAsyncNonDeletedEntity(id);
                entity = await _repository.GetByIDAndIncludeRelatedEntities(id, entitiesToIncludes);

                // entity = result;
                if (entity != null)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(entity, ActionStatus.NothingModified));

                    return new RequestActionResult<D>(entity, ActionStatus.NothingModified);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetByIDAndIncludeRelatedEntities");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<D>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(entity, ActionStatus.NotFound));

            return new RequestActionResult<D>(entity, ActionStatus.NotFound);

        }

        public async Task<RequestActionResult<D>> GetAsNoTrackingByIDAndIncludeRelatedEntities(int id, IEnumerable<string> entitiesToIncludes)
        {
            D entity = null;

            try
            {
                Log.Information("Inside GetAsNoTrackingByIDAndIncludeRelatedEntities");
                Log.Information("{@id}", id);
                entity = await _repository.GetAsNoTrackingByIDAndIncludeRelatedEntities(id, entitiesToIncludes);

                if (entity != null)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(entity, ActionStatus.NothingModified));

                    return new RequestActionResult<D>(entity, ActionStatus.NothingModified);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in GetAsNoTrackingByIDAndIncludeRelatedEntities");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<D>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(entity, ActionStatus.NotFound));

            return new RequestActionResult<D>(entity, ActionStatus.NotFound);

        }

        //end

        public async Task<int> SaveAsync()
        {
            return await _repository.SaveAsync();
        }

        public  async Task<RequestActionResult<D>> Update(U entity)
        {
            var D = new D();
            var audit = new Audit();
            try
            {
                Log.Information("Inside Update");
                Log.Information("{@entity}", entity);

               // D = _factoryForUpdate.CreateGenericObject(entity) as D;
                //new
                D = await _repository.GetAsNoTrackingByID(entity.Id);

                audit.Created = D.Created;
                audit.CreatedBy = D.CreatedBy;
                audit.Deleted = D.Deleted;
                audit.DeletedBy = D.CreatedBy;
                audit.IsDeleted = D.IsDeleted;
                audit.Modified = D.Modified;
                audit.ModifiedBy = D.ModifiedBy;
                D = _factoryForUpdate.CreateGenericObjectForUpdate(entity, D);
                //end
                Log.Information("{@D}", D);
                // _repository.Update(entity);

                //return audit values
                D.Created = audit.Created;
                D.CreatedBy = audit.CreatedBy;
                D.Deleted = audit.Deleted;
                D.DeletedBy = audit.CreatedBy;
                D.IsDeleted = audit.IsDeleted;
                D.Modified = audit.Modified;
                D.ModifiedBy = audit.ModifiedBy;
                //end
                _repository.Update(D);
                var result = await _repository.SaveAsync();
                if (result > 0)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(D, ActionStatus.Updated));

                    return new RequestActionResult<D>(D, ActionStatus.Updated);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in Update");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<D>(D, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<D>(D, ActionStatus.NothingModified));

            return new RequestActionResult<D>(D, ActionStatus.NothingModified);

        }

        public  async Task<RequestActionResult<IEnumerable<D>>> UpdateRange(IEnumerable<U> entity)
        {
            var updateIds = new List<int>();
            var DList = new List<D>();
            try
            {

                Log.Information("Inside UpdateRange");
                Log.Information("{@entity}", entity);
                if (entity != null && entity.Count() > 0)
                {
                    foreach (var e in entity)
                    {
                        updateIds.Add(e.Id);
                    }
                }

               // DList =  _repository.GetAllAsQuerable().Where(D => updateIds.Contains(D.Id)).ToList();
                DList = _repository.GetAllAsQuerableAsNoTracking().Where(D => updateIds.Contains(D.Id)).ToList();

                
                DList = _factoryForUpdate.CreateGenericObjectListForUpdate(entity, DList).ToList();
                //end
                Log.Information("{@DList}", DList);

               // _repository.UpdateRange(entity);
                _repository.UpdateRange(DList);
                var result = await _repository.SaveAsync();
                if (result > 0)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(DList, ActionStatus.Updated));

                    return new RequestActionResult<IEnumerable<D>>(DList, ActionStatus.Updated);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in UpdateRange");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<IEnumerable<D>>(DList, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<D>>(DList, ActionStatus.NothingModified));

            return new RequestActionResult<IEnumerable<D>>(DList, ActionStatus.NothingModified);

        }

        public async Task<RequestActionResult<C>> MarkAsDeleted(int Id)
        {
            C entity = null;
            try
            {
                Log.Information("Inside MarkAsDeleted");
                Log.Information("{@entityId}", Id);
                _repository.MarkAsDeleted(Id);
                var result = await _repository.SaveAsync();
                if (result > 0)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<C>(entity, ActionStatus.Deleted));

                    return new RequestActionResult<C>(entity, ActionStatus.Deleted);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in MarkAsDeleted");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<C>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<C>(entity, ActionStatus.NothingModified));

            return new RequestActionResult<C>(entity, ActionStatus.NothingModified);

        }

        public async Task<RequestActionResult<IEnumerable<C>>> MarkRangeAsDeleted(IEnumerable<int> Ids)
        {
            IEnumerable<C> entity = null;

            try
            {
                Log.Information("Inside MarkRangeAsDeleted");
                Log.Information("{@entityIds}", Ids);
                _repository.MarkRangeAsDeleted(Ids);
                var result = await _repository.SaveAsync();
                if (result > 0)
                {
                    Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.Deleted));

                    return new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.Deleted);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "An error has occured in MarkRangeAsDeleted");
                Log.Error(ex.Message);
                Log.Error(ex.StackTrace);
                return new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.Error, ex);

            }
            Log.Information("Result {@RequestActionResult}", new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.NothingModified));

            return new RequestActionResult<IEnumerable<C>>(entity, ActionStatus.NothingModified);

        }


    }
}
