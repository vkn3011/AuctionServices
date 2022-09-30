using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using EAuction.Domain.Seller;
using EAuction.Service.BidsService;
using EAuction.Service.Model;
using EAuction.Service.ProductService;
using EAuction.Service.SellerService;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EAuction.API.Controllers
{

    [Route("api/[controller]")]
    public class SellerController : Controller
    {
        private readonly SellerService _sellerService;        
       // private readonly BidService _bidService;

        public SellerController(SellerService sellerService)
        {
            _sellerService = sellerService;            
            //_bidService = bidService;
        }

        /// <summary>
        /// Get All seller information
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("/getAllSellers")]
        public async Task<IActionResult> GetAllSeller()
        {
            return Ok(await _sellerService.GetAllSeller());
        }

        /// <summary>
        /// Add seller.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/add-seller")]
        public async Task<IActionResult> AddSeller([FromBody] string value)
        {
            SellerInfo seller = JsonConvert.DeserializeObject<SellerInfo>(value);

            var result = await _sellerService.AddSeller(seller);

            return Created("/api/DataEventRecord", result);
        }            
  

    }
}
