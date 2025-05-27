using Core.Services.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
//using System.Web.Mvc;
using WebApi.GlobalException;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    [RoutePrefix("products")]
    [GenericExceptionHandler]
    public class ProductController : BaseApiController
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        [Route("list")]
        public async Task<HttpResponseMessage> GetProducts(string name = null, string category = null, bool? isActive = null)
        {
            var products = await _productService.GetProducts(name, category, isActive);
            var productModels = products.Select(q => new ProductModel(q)).ToList();
            return Found(productModels);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<HttpResponseMessage> GetProduct(int id)
        {
            var product = await _productService.GetProductById(id);
            if (product == null)
                return DoesNotExist();
            return Found(new ProductModel(product));
        }

        [HttpPost]
        [Route("create")]
        public async Task<HttpResponseMessage> CreateProduct([FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var product = await _productService.CreateProduct(model.ToEntity());
            return Found(new ProductModel(product));
        }

        [HttpPut]
        [Route("{id:int}")]
        public async Task<HttpResponseMessage> UpdateProduct(int id, [FromBody] ProductModel model)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            var updated = await _productService.UpdateProduct(id, model.ToEntity());
            if (updated == null)
                return DoesNotExist();
            return Found(new ProductModel(updated));
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<HttpResponseMessage> DeleteProduct(int id)
        {
            var deleted = await _productService.DeleteProduct(id);
            if (!deleted)
                return DoesNotExist();
            return Found();
        }
    }
}