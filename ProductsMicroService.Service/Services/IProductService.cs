using Microsoft.AspNetCore.Http;
using ProductsMicroService.Data.Models;
using ProductsMicroService.Service.Utilities;
using System.Threading.Tasks;

namespace ProductsMicroService.Service.Services
{
    public interface IProductService : IGenericService<Product,Product, Product, Product>
    {
        Task<RequestActionResult<Product>> AddWithFile(string name, string description, decimal price, int quantity, string size, IFormFile file);
        void test();
    }
}