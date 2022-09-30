using EAuction.Domain.Buyer;
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
    [Route("e-auction/api/v1/buyer/[controller]")]
    public class BuyerController : Controller
    {
        private readonly BuyerService _buyerService;
        private readonly BidService _bidService;

        public BuyerController(BuyerService buyerService, BidService bidService)
        {
            _buyerService = buyerService;
            _bidService = bidService;   
        }

        /// <summary>
        /// Returns list of all Buyer
        /// </summary>
        /// <returns></returns>
        [ProducesResponseType(typeof(IEnumerable<BuyerInfo>),200)]
        [HttpGet]
        [Route("/getAllBuyer")]
        public async Task<IActionResult> GetAllBuyer()
        {
            return Ok(await _buyerService.GetAllBuyer());
        }

        /// <summary>
        /// Place Bid
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [HttpPost]
        [Route("/buyer")]
        public async Task<IActionResult> PlaceBid([FromBody] string value)
        {
            BuyerInfo buyerInfo = JsonConvert.DeserializeObject<BuyerInfo>(value);

            var result = await _buyerService.AddBuyer(buyerInfo);

            return Created("/api/DataEventRecord", result);
        }
       
    }
}
