using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EAuction.Domain.Seller;
using EAuction.Service.BidsService;
using EAuction.Service.Model;
using EAuction.Service.ProductService;
using EAuction.Service.SellerService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EAuction.API.Controllers
{
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly ProductService _productService;        
        public ProductController(ProductService productService)
        {            
            _productService = productService;            
        }

        /// <summary>
        /// Get list of all product
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/getAllproducts")]
        public async Task<IActionResult> GetAllProducts()
        {
            return Ok(await _productService.GetAllProducts());
        }

        /// <summary>
        /// Add Product
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/add-products")]  //Direct use
        public async Task<IActionResult> AddProducts([FromBody] string value)
        {
            ProductInfo product = JsonConvert.DeserializeObject<ProductInfo>(value);

            var result = await _productService.AddProduct(product);

            return Created("/api/DataEventRecord", result);
        }
      
        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("/delete/{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Conflict)]
        public async Task<IActionResult> Delete(long id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            if (!await _productService.ExistsProducts(id))
            {
                return NotFound($"Product with Id {id} does not exist");
            }

            await _productService.DeleteProduct(id);

            return Ok();
        }
       


    }
}
