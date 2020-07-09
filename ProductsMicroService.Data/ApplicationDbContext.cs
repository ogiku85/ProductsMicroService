using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ProductsMicroService.Data.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductsMicroService.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IHttpContextAccessor _contextAccessor;
        public DbSet<Product> Products { get; set; }
        //END


        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor contextAccessor)
        : base(options)
        {
            _contextAccessor = contextAccessor;
        }
        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            int saved = 0;
            var currentUser = _contextAccessor.HttpContext.User.Identity.Name;
            var currentDate = DateTime.Now;
            try
            {
                foreach (var entry in ChangeTracker.Entries<IAudit>()
               .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    if (entry.State == EntityState.Added)
                    {

                        entry.Entity.Created = currentDate;
                        entry.Entity.CreatedBy = currentUser;
                        entry.Entity.Modified = currentDate;
                        entry.Entity.ModifiedBy = currentUser;


                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.Modified = currentDate;
                        entry.Entity.ModifiedBy = currentUser;
                        if (entry.Entity.IsDeleted == true && entry.Entity.Deleted == null)
                        {
                            entry.Entity.Deleted = currentDate;
                            entry.Entity.DeletedBy = currentUser;
                        }
                    }
                }
                saved = await base.SaveChangesAsync(cancellationToken);
            }
            catch (Exception Ex)
            {
                //Log.Error(Ex, "An error has occured in SaveChanges");
                //Log.Error(Ex.InnerException, "An error has occured in SaveChanges InnerException");
                //Log.Error(Ex.StackTrace, "An error has occured in SaveChanges StackTrace");

            }

            return saved;
            //   return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            int saved = 0;
            var currentUser = _contextAccessor.HttpContext.User.Identity.Name;
            var currentDate = DateTime.Now;
            try
            {
                foreach (var entry in ChangeTracker.Entries<IAudit>()
               .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
                {
                    if (entry.State == EntityState.Added)
                    {

                        entry.Entity.Created = currentDate;
                        entry.Entity.CreatedBy = currentUser;
                        entry.Entity.Modified = currentDate;
                        entry.Entity.ModifiedBy = currentUser;


                    }
                    if (entry.State == EntityState.Modified)
                    {
                        entry.Entity.Modified = currentDate;
                        entry.Entity.ModifiedBy = currentUser;
                        if (entry.Entity.IsDeleted == true && entry.Entity.Deleted == null)
                        {
                            entry.Entity.Deleted = currentDate;
                            entry.Entity.DeletedBy = currentUser;
                        }
                    }
                }
                saved = base.SaveChanges();
            }
            catch (Exception Ex)
            {
                //Log.Error(Ex, "An error has occured in SaveChanges");
                //Log.Error(Ex.InnerException, "An error has occured in SaveChanges InnerException");
                //Log.Error(Ex.StackTrace, "An error has occured in SaveChanges StackTrace");

            }

            return saved;
        }
    }
}
