using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProductsMicroService.Data.Models;
using ProductsMicroService.Service.Services;
using ProductsMicroService.Service.Utilities;
using Serilog;

namespace ProductsMicroService.API.Controllers
{
   //[Produces("application/json")]
    // [Route("api/Products")]
   // [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductService ProductService;
        private readonly FileUploadSettings fileUploadSettings;

        public ProductsController(IProductService ProductService,
            IOptionsSnapshot<FileUploadSettings> fileUploadSettings)
        {
            this.ProductService = ProductService;
            this.fileUploadSettings = fileUploadSettings.Value;
        }
        [Route("GetAll")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await ProductService.GetAll();
                Log.Information("result from GetAll method is @result", result);
                if (result.Status == ActionStatus.NothingModified)
                {
                    return Ok(result.Entity);
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in GetAll");

            }
            return NoContent();
        }
         [HttpGet("{id}")]
        //  [HttpGet]
       // [Route("Get")]
      // [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var result = await ProductService.GetByID(id);
                Log.Information("result from Get method is @result", result);

                if (result.Status == ActionStatus.NothingModified)
                {
                    return Ok(result.Entity);
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in Get");

            }
            return NoContent();
        }

        [HttpGet("GetByIDAndIncludeRelatedEntities/{id}")]
       // [HttpGet]
       // [Route("GetByIDAndIncludeRelatedEntities")]
       // [HttpGet("{id}", Name = "GetByIDAndIncludeRelatedEntities")]
        public async Task<IActionResult> GetByIDAndIncludeRelatedEntities(int id)
        {
            try
            {
                Log.Information("Inside GetByIDAndIncludeRelatedEntities method...");
                List<string> entitiesToInclude = new List<string>();
                var result = await ProductService.GetByIDAndIncludeRelatedEntities(id, entitiesToInclude);

                Log.Information("result from GetByIDAndIncludeRelatedEntities method is @result", result);

                if (result.Status == ActionStatus.NothingModified)
                {
                    Log.Information("result from GetByIDAndIncludeRelatedEntities method is @result", result);

                    return Ok(result.Entity);
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in GetByIDAndIncludeRelatedEntities");
            }
            return NoContent();
        }

        [HttpGet]
        [Route("GetAllAndIncludeRelatedEntities")]
        public async Task<IActionResult> GetAllAndIncludeRelatedEntities()
        {
            try
            {
                List<string> entitiesToInclude = new List<string>();
                var result = await ProductService.GetAllAndIncludeRelatedEntities(entitiesToInclude);
                Log.Information("result from GetAllAndIncludeRelatedEntities method is @result", result);

                if (result.Status == ActionStatus.NothingModified)
                {
                    return Ok(result.Entity);
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in GetAllPublicHolidayForDisplay");

            }
            return NoContent();
        }

        [HttpPost]
        [Route("AddWithFile")]
        public async Task<IActionResult> AddWithFile(string name, string description, decimal price, int quantity, string size, IFormFile file)
        {

            try
            {

                if (file == null)
                {
                    Log.Information("Uploaded file is null");
                    return BadRequest("Uploaded File Is null");
                }
                if (fileUploadSettings.IsFileEmpty(file.Length))
                {
                    return BadRequest("Uploaded File is Empty");
                }
                if (fileUploadSettings.IsMaxBytesExceeded(file.Length))
                {
                    return BadRequest("Maximum file size of " + (fileUploadSettings.MaxBytes / (1024 * 1024)) + "MB exceeded.");
                }
                if (!fileUploadSettings.AcceptedFileTypes.Any(s => s == Path.GetExtension(file.FileName).ToLower()))
                {
                    return BadRequest("invalid file extension");
                }
                var result = await ProductService.AddWithFile(name, description, price, quantity, size, file);
                Log.Information("result from Add method is @result", result);

                if (result.Status == ActionStatus.Created)
                {
                    // return Created(new Uri())
                    return Created(Request.GetDisplayUrl() + "/" + result.Entity.Id, result.Entity);

                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in ProjectSupportDocument");

            }
            return BadRequest();
        }
        [HttpPost]
        [Route("Add")]
        public async Task<IActionResult> Add([FromBody]Product Product)
        {

            try
            {

                if (Product == null || !ModelState.IsValid)
                {
                    return BadRequest("Specified object is null or invalid");
                }
                var result = await ProductService.Add(Product);
                Log.Information("result from Add method is @result", result);

                if (result.Status == ActionStatus.Created)
                {
                    // return Created(new Uri())
                    return Created(Request.GetDisplayUrl() + "/" + result.Entity.Id, result.Entity);

                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in Product");

            }
            return BadRequest();
        }
        [Route("AddRange")]
        [HttpPost]
        public async Task<IActionResult> AddRange([FromBody]List<Product> ProductList)
        {

            try
            {
                if (ProductList == null || ProductList.Count == 0 || !ModelState.IsValid)
                {
                    return BadRequest("Specified object is null or invalid");
                }
                var result = await ProductService.AddRange(ProductList);
                if (result.Status == ActionStatus.Created)
                {
                    // return Created(new Uri())
                    return Created(Request.GetDisplayUrl() + "/", result.Entity);

                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in ProductList");

            }
            return BadRequest();
        }

        [Route("Update")]
        [HttpPut]
        // public async Task<IActionResult> Update([FromBody]Product Product)
        public async Task<IActionResult> Update([FromBody]Product Product)

        {

            try
            {

                if (Product == null || !ModelState.IsValid)
                {
                    return BadRequest("Specified object is null or invalid");
                }
                var result = await ProductService.Update(Product);
                Log.Information("result from Update method is @result", result);

                if (result.Status == ActionStatus.Updated)
                {
                    return Ok(result.Entity);
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in Update");

            }
            return BadRequest();
        }

        [Route("UpdateRange")]
        [HttpPut]
        //public async Task<IActionResult> UpdateRange([FromBody]List<Product> ProductList)
        public async Task<IActionResult> UpdateRange([FromBody]List<Product> ProductList)

        {

            try
            {
                if (ProductList == null || ProductList.Count == 0 || !ModelState.IsValid)
                {
                    return BadRequest("Specified object is null or invalid");
                }
                var result = await ProductService.UpdateRange(ProductList);
                if (result.Status == ActionStatus.Updated)
                {
                    // return Created(new Uri())
                    return Ok(result.Entity);

                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in Update Range");
            }
            return BadRequest();
        }

        [Route("MarkAsDeleted")]
        [HttpDelete]
        public async Task<IActionResult> MarkAsDeleted(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Specified id is null or invalid");
                }
                var result = await ProductService.MarkAsDeleted(id);
                Log.Information("result from MarkAsDeleted method is @result", result);

                if (result.Status == ActionStatus.Deleted)
                {
                    return NoContent();
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in MarkAsDeleted");

            }
            return BadRequest("Object not marked as deleted.");
        }

        [Route("MarkRangeAsDeleted")]
        [HttpDelete]
        public async Task<IActionResult> MarkRangeAsDeleted([FromBody]List<int> ids)
        {
            try
            {
                if (ids == null || ids.Count == 0)
                {
                    return BadRequest("Specified ids are null or invalid");
                }
                var result = await ProductService.MarkRangeAsDeleted(ids);
                if (result.Status == ActionStatus.Deleted)
                {
                    return NoContent();
                }
            }
            catch (Exception Ex)
            {
                Log.Error(Ex, "An error has occured in MarkRangeAsDeleted");

            }
            return BadRequest("Objects not marked as deleted.");
        }
    }

}