
using EAuction.Domain.Buyer;
using EAuction.Service.Bids.Model;
using EAuction.Service.BuyerModels;
using EAuction.Service.BuyerService;
using EAuction.Service.Model;
using EAuction.Service.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EAuction.Service.BidsService
{
	public class BidService
	{

		private readonly IDataAccessProvider _dataAccessProvider;
        private readonly BuyerService.BuyerService _buyerService;
        private readonly ProductService.ProductService _productService;

        public BidService(IDataAccessProvider dataAccessProvider, BuyerService.BuyerService buyerService, ProductService.ProductService productService)
		{
			_dataAccessProvider = dataAccessProvider;
            _buyerService = buyerService;
            _productService = productService;

        }

        public async Task<BidsInfo> AddBid(BuyerInfo buyerInfo,int productId)
        {
            BidsInfo result = new BidsInfo();
            ProductInfo productInfo = new ProductInfo();
            var productInfoData = await _dataAccessProvider.GetProductById(productId);

            var buyerRecord = new Buyer
            {
                BuyerId = buyerInfo.BuyerId,
                Address = buyerInfo.Address,
                BidAmount = buyerInfo.BidAmount,
                City = buyerInfo.City,
                CreatedDate = System.DateTime.Now,
                Email = buyerInfo.Email,
                FirstName = buyerInfo.FirstName,
                LastName = buyerInfo.LastName,
                Phone = buyerInfo.Phone,
                PinCode = buyerInfo.PinCode,
                ProductId = buyerInfo.ProductId,
                State = buyerInfo.State
            };
            var der = await _dataAccessProvider.AddBuyer(buyerRecord);
            if (productInfoData != null)
            {
                productInfo.BidEndDate = productInfoData.BidEndDate;
                productInfo.Category = productInfoData.Category;
                productInfo.CreatedDate = productInfoData.CreatedDate;
                productInfo.DetailedDescription = productInfoData.DetailedDescription;
                productInfo.IsDeleted = productInfoData.IsDeleted;
                productInfo.ProductId = productInfoData.ProductId;
                productInfo.ProductName = productInfoData.ProductName;
                productInfo.SellerId = productInfoData.SellerId;
                productInfo.ShortDescription = productInfoData.ShortDescription;
                productInfo.StartingPrice = productInfoData.StartingPrice;

            }
            result.buyerInfo.Add(buyerInfo);
            result.productInfo = productInfo;
            return result;
        }

        public async Task UpdateBid(int productId, string buyerEmailId, double newBidAmt)
        {
            var buyerExists = _buyerService.GetAllBuyer().Result.Where(a => a.ProductId == productId && a.Email == buyerEmailId).SingleOrDefault();
            var productInfo = _productService.GetAllProducts().Result.Where(a => a.ProductId == productId).SingleOrDefault();
            if (productInfo == null)
            {
                throw new Exception("Product is not exists.");
            }
            if (buyerExists == null)
            {
                throw new Exception("This buyer is not exists.");
            }
            if (productInfo != null && productInfo.StartingPrice > newBidAmt)
            {
                throw new Exception("Bid Amount is not less then the product starting price.");
            }
            if (productInfo != null && productInfo.BidEndDate < System.DateTime.Now)
            {
                throw new Exception("Bid end date is expired.");
            }


            if (buyerExists != null)
            {
                await _dataAccessProvider.UpdateBid(productId, buyerEmailId, newBidAmt);

            }

        }

        public async Task<BidsInfo> ShowAllBids(int productId)
        {
            BidsInfo result = new BidsInfo();
            ProductInfo productInfo = new ProductInfo();
            List<BuyerInfo> buyerInfo = new List<BuyerInfo>();
            var productInfoData = await _dataAccessProvider.GetProductById(productId);
            var buyerInfoData = await _dataAccessProvider.GetAllBidsByProductId(productId);

            if (productInfoData != null)
            {
                productInfo.BidEndDate = productInfoData.BidEndDate;
                productInfo.Category = productInfoData.Category;
                productInfo.CreatedDate = productInfoData.CreatedDate;
                productInfo.DetailedDescription = productInfoData.DetailedDescription;
                productInfo.IsDeleted = productInfoData.IsDeleted;
                productInfo.ProductId = productInfoData.ProductId;
                productInfo.ProductName = productInfoData.ProductName;
                productInfo.SellerId = productInfoData.SellerId;
                productInfo.ShortDescription = productInfoData.ShortDescription;
                productInfo.StartingPrice = productInfoData.StartingPrice;

            }

            if (buyerInfoData != null && buyerInfoData.Count > 0)
            {
                foreach (var item in buyerInfoData)
                {
                    BuyerInfo buyer = new BuyerInfo();
                    buyer.Address = item.Address;
                    buyer.BidAmount = item.BidAmount;
                    buyer.BuyerId = item.BuyerId;
                    buyer.City = item.City;
                    buyer.CreatedDate = item.CreatedDate;
                    buyer.Email = item.Email;
                    buyer.FirstName = item.FirstName;
                    buyer.LastName = item.LastName;
                    buyer.Phone = item.Phone;
                    buyer.PinCode = item.PinCode;
                    buyer.ProductId = item.ProductId;
                    buyer.State = item.State;
                    buyerInfo.Add(buyer);
                }
            }

            result.buyerInfo = buyerInfo;
            result.productInfo = productInfo;
            return result;
        }
    }
}
