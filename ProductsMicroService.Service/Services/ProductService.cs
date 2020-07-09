using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using ProductsMicroService.Data.Models;
using ProductsMicroService.Repository.Repository;
using ProductsMicroService.Service.Factories;
using ProductsMicroService.Service.Utilities;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ProductsMicroService.Service.Services
{
    public class ProductService : GenericService<Product, Product, Product, Product>, IProductService
    {
        private IHostingEnvironment host;
        private IProductRepository productRepository;
        public ProductService(GenericRepository<Product> repository, IGenericObjectFactory<Product, Product> factory, IGenericObjectFactory<Product, Product> factoryForUpdate, IProductRepository productRepository, IHostingEnvironment host)
            : base(repository, factory, factoryForUpdate)
        {
            this.productRepository = productRepository;
            this.host = host;
        }
        public void test()
        {

        }
        public async Task<RequestActionResult<Product>> AddWithFile(string name, string description, decimal price, int quantity, string size, IFormFile file)
        {
            int added = 0;
            var product = new Product();

            var productResult = new Product();
            try
            {
                Log.Information("Inside AddWithFile method");

                Log.Information(" name = {@name}, description = {@description}, price = {@price}, quantity = {@quantity}, size = {@size} ",
                  name, description, price, quantity, size);
                var uploadsFolder = "Uploads/Products";
                var uploadsFolderPath = Path.Combine(host.WebRootPath, uploadsFolder);

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }


                product.Name = name;
                product.Description = description;
                product.Price = price;
                product.Quantity = quantity;
                product.Size = size;

                await productRepository.Add(product);
                added = await productRepository.SaveAsync();
                if (added > 0)
                {
                    var fileExtension = Path.GetExtension(file.FileName);
                    var fileName = "product-" + product.Id + Path.GetExtension(file.FileName);
                    var filePath = Path.Combine(uploadsFolderPath, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite))
                    {

                        file.CopyTo(stream);

                        product.URL = uploadsFolder + "/" + fileName;
                        productRepository.Update(product);
                        int updated = await productRepository.SaveAsync();
                        if (updated > 0)
                        {

                            return new RequestActionResult<Product>(product, ActionStatus.Created);
                        }


                    }
                }

            }
            catch (Exception Ex)
            {
                added = 0;
                Log.Error(Ex, "An error has occured in AddWithFile");
                return new RequestActionResult<Product>(product, ActionStatus.Error, Ex);

            }
            Log.Information("ProductResult from AddWithFile method is {@ProductResult}", productResult);

            return new RequestActionResult<Product>(product, ActionStatus.NothingModified);

        }

    }
}
