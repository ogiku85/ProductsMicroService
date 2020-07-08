using ProductsMicroService.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductsMicroService.Data.Utilities;

namespace ProductsMicroService.Repository.Repository
{
    
    public class GenericRepository<T> : IGenericRepository<T> where T : class, IAudit
    {
        protected readonly DbContext _context;
      //  protected readonly IdentityDbContext<ApplicationUser, ApplicationRole, string> _context;

        public GenericRepository(DbContext context)
        {
            _context = context;
        }
        public async Task Add(T Entity)
        {
           await _context.Set<T>().AddAsync(Entity);
        }
        public void Update(T Entity)
        {
            _context.Entry<T>(Entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        }
        public void UpdateRange(IEnumerable<T> Entities)
        {
            foreach (var Entity in Entities)
            {
                _context.Entry<T>(Entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            }
        }
        public async Task AddRange(List<T> Entities)
        {
           await _context.Set<T>().AddRangeAsync(Entities);
        }

        public void Delete(int Id)
        {
            T Entity = _context.Set<T>().Find(Id);
            if (Entity != null)
            {
                _context.Set<T>().Remove(Entity);
            }

        }
        public void MarkAsDeleted(int Id)
        {
            T Entity = _context.Set<T>().Find(Id);
            if (Entity != null)
            {
                Entity.IsDeleted = true;
                _context.Entry<T>(Entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

            }

        }
        public void MarkRangeAsDeleted(IEnumerable<int> EntitiesId)
        {
            List<T> Entities = new List<T>();

            Entities = _context.Set<T>().Where(e => EntitiesId.Contains(e.Id)).ToList();
            if (Entities.Count > 0)
            {
                foreach (var Entity in Entities)
                {
                    Entity.IsDeleted = true;
                    _context.Entry<T>(Entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                }

            }


        }
        public void Delete2(T Entity)
        {
            _context.Set<T>().Remove(Entity);
        }

        public void DeleteRange(IEnumerable<int> EntitiesId)
        {

            List<T> Entities = new List<T>();

            Entities = _context.Set<T>().Where(e => EntitiesId.Contains(e.Id)).ToList();
            if (Entities.Count > 0)
            {
                _context.Set<T>().RemoveRange(Entities);

            }


        }
        public void DeleteRangeOld(IEnumerable<int> EntitiesId)
        {
            T Entity;
            foreach (int Id in EntitiesId)
            {
                Entity = _context.Set<T>().Find(Id);
                if (Entity != null)
                {
                    _context.Set<T>().Remove(Entity);
                }
            }

        }
        public void DeleteRange2(List<T> Entities)
        {
            _context.Set<T>().RemoveRange(Entities);

        }

        public IEnumerable<T> Find(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().Where(e => e.IsDeleted == false).ToListAsync();
        }
        public IQueryable<T> GetAllAsQuerable()
        {
            return  _context.Set<T>().Where(e => e.IsDeleted == false);
        }
        public IQueryable<T> GetAllAsQuerableAsNoTracking()
        {
            return _context.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false);
        }
        public async Task<IEnumerable<T>> GetAll(QueryParameters queryParameters)
        {
            return await _context.Set<T>().Where(e => e.IsDeleted == false)
                .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();
        }
        public async Task<PagedData<T>> GetAllPagedData(QueryParameters queryParameters)
        {
            PagedData<T> pagedData = new PagedData<T>();
            try
            {
                pagedData.queryParameters = queryParameters;
                pagedData.DataCount = await _context.Set<T>().CountAsync();

                pagedData.PagedDataList = await _context.Set<T>().Where(e => e.IsDeleted == false)
                    .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
                    .Take(queryParameters.PageSize)
                    .ToListAsync();
            }
            catch(Exception ex)
            {

            }

            return pagedData;
        }
        public async Task<PagedData<T>> GetAllPagedDataAndIncludeRelatedEntities(QueryParameters queryParameters, IEnumerable<string> entitiesToIncludes)
        {
            PagedData<T> pagedData = new PagedData<T>();
            try
            {
                pagedData.queryParameters = queryParameters;
                pagedData.DataCount = await _context.Set<T>().CountAsync();

                //pagedData.PagedDataList = await _context.Set<T>().Where(e => e.IsDeleted == false)
                //    .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
                //    .Take(queryParameters.PageSize)
                //    .ToListAsync();

                // make sure all entit implement audit so taht you can select only those that are not deleted when using include with string
                var query = _context.Set<T>().Where(e => e.IsDeleted == false);
                if (entitiesToIncludes != null && entitiesToIncludes.Count() > 0)
                {
                    foreach (var e in entitiesToIncludes)
                    {
                        query = query.Include(e);
                    }
                }
                pagedData.PagedDataList = await query.Skip((queryParameters.Page - 1) * queryParameters.PageSize)
                    .Take(queryParameters.PageSize)
                    .ToListAsync();

            }
            catch (Exception ex)
            {

            }

            return pagedData;
        }

        public async Task<PagedData<T>> GetAllPagedDataKEEP(QueryParameters queryParameters)
        {
            PagedData<T> pagedData = new PagedData<T>();
            pagedData.queryParameters = queryParameters;
            pagedData.DataCount = await _context.Set<T>().CountAsync();

            pagedData.PagedDataList = await _context.Set<T>().Where(e => e.IsDeleted == false)
                .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync();

            return pagedData;
        }
        public async Task<T> GetByID(int id)
        {
            return await _context.Set<T>().Where(e => e.IsDeleted == false && e.Id == id).FirstOrDefaultAsync();
        }
        public async Task<T> GetAsNoTrackingByID(int id)
        {
            return await _context.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false && e.Id == id).FirstOrDefaultAsync();
        }

        //new 
        public async Task<IEnumerable<T>> FindAndIncludeRelatedEntities(System.Linq.Expressions.Expression<Func<T, bool>> predicate, IEnumerable<string> entitiesToIncludes)
        {
            var query =  _context.Set<T>().Where(predicate);
            if (entitiesToIncludes != null && entitiesToIncludes.Count() > 0)
            {
                foreach (var e in entitiesToIncludes)
                {
                    query = query.Include(e);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAndIncludeRelatedEntities(IEnumerable<string> entitiesToIncludes)
        {
            var query = _context.Set<T>().Where(e => e.IsDeleted == false);
            if (entitiesToIncludes != null && entitiesToIncludes.Count() > 0)
            {
                foreach (var e in entitiesToIncludes)
                {
                    query = query.Include(e);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAndIncludeRelatedEntities(QueryParameters queryParameters, IEnumerable<string> entitiesToIncludes)
        {
            var query =  _context.Set<T>().Where(e => e.IsDeleted == false)
                .Skip((queryParameters.Page - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize);
            if (entitiesToIncludes != null && entitiesToIncludes.Count() > 0)
            {
                foreach (var e in entitiesToIncludes)
                {
                    query = query.Include(e);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIDAndIncludeRelatedEntities(int id, IEnumerable<string> entitiesToIncludes)
        {
            var query =  _context.Set<T>().Where(e => e.IsDeleted == false && e.Id == id);
            if (entitiesToIncludes != null && entitiesToIncludes.Count() > 0)
            {
                foreach (var e in entitiesToIncludes)
                {
                    query = query.Include(e);
                   // query = query.Include(e).Where(s=> s.IsDeleted == false);

                }
            }
            return await query.FirstOrDefaultAsync();
        }
        public async Task<T> GetAsNoTrackingByIDAndIncludeRelatedEntities(int id, IEnumerable<string> entitiesToIncludes)
        {
            var query = _context.Set<T>().AsNoTracking().Where(e => e.IsDeleted == false && e.Id == id);
            if (entitiesToIncludes != null && entitiesToIncludes.Count() > 0)
            {
                foreach (var e in entitiesToIncludes)
                {
                    query = query.Include(e);
                  //  query = query.Include(e).Where(s => s.IsDeleted == false);

                }
            }
            return await query.FirstOrDefaultAsync();

        }
        public int Save()
        {
            int saved = 0;
            saved = _context.SaveChanges();
            
            return saved;
        }
        public async Task<int> SaveAsync()
        {
            int saved = 0;
            
            saved = await _context.SaveChangesAsync();
            
            return saved;
        }
        //end
    }

}
