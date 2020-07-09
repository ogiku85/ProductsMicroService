using Microsoft.EntityFrameworkCore;
using ProductsMicroService.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsMicroService.Repository.Repository
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(DbContext context) : base(context)
        {
        }
        public void test()
        {

        }
    }
}
