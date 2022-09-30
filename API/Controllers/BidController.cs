using EAuction.Domain.Buyer;
using EAuction.Service.Bids.Model;
using EAuction.Service.BidsService;
using EAuction.Service.BuyerModels;
using EAuction.Service.BuyerService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace EAuction.API.Controllers
{
    [Route("e-auction/api/v1/bid/[controller]")]
    public class BidController : Controller
    {
        
        private readonly BidService _bidService;

        public BidController(BuyerService buyerService, BidService bidService)
        {
            
            _bidService = bidService;   
        }



        /// <summary>
        /// Update Existing Bid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [HttpPut("/update-bid")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> UpdateBid([FromBody] string value)
        {
            try
            {
                BidInfo buyerInfo = JsonConvert.DeserializeObject<BidInfo>(value);
                if (buyerInfo.ProductId == 0)
                {
                    return BadRequest();
                }
                await _bidService.UpdateBid(buyerInfo.ProductId, buyerInfo.Email, buyerInfo.BidAmount);
                return Ok();
            }
            catch (Exception ex)
            {

                return NotFound(ex.Message);
            }           
        }

        /// <summary>
        /// Show all bid
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("/show-bids/{productId}")]
        public async Task<IActionResult> ShowAllBids(int productId)
        {
            if (productId == 0)
            {
                return BadRequest();
            }
            return Ok(await _bidService.ShowAllBids(productId));
        }       
    }
}
