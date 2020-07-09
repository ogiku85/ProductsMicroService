using ProductsMicroService.Data.Models;

namespace ProductsMicroService.Repository.Repository
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        void test();
    }
}