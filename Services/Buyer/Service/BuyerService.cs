
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EAuction.Domain.Buyer;
using EAuction.Service.BuyerModels;

namespace EAuction.Service.BuyerService
{
	public class BuyerService
	{
		private readonly IDataAccessProvider _dataAccessProvider;

		public BuyerService(IDataAccessProvider dataAccessProvider)
		{
			_dataAccessProvider = dataAccessProvider;
		}

        public async Task<BuyerInfo> AddBuyer(BuyerInfo value)
        {
            var buyerRecord = new Buyer
            {
                BuyerId=value.BuyerId,
                Address = value.Address,
                BidAmount = value.BidAmount,
                City = value.City,
                CreatedDate = System.DateTime.Now,
                Email = value.Email,
                FirstName = value.FirstName,
                LastName = value.LastName,
                Phone = value.Phone,
                PinCode = value.PinCode,
                ProductId = value.ProductId,
                State = value.State,
            };



            var der = await _dataAccessProvider.AddBuyer(buyerRecord);

            var result = new BuyerInfo
            {
                Address = der.Address,
                BidAmount = der.BidAmount,
                City = der.City,
                CreatedDate = System.DateTime.Now,
                Email = der.Email,
                FirstName = der.FirstName,
                LastName = der.LastName,
                Phone = der.Phone,
                PinCode = der.PinCode,
                ProductId = der.ProductId,
                State = der.State,
                BuyerId = der.BuyerId
            };

            return result;
        }

        public async Task<IEnumerable<BuyerInfo>> GetAllBuyer()
        {
            var data = await _dataAccessProvider.GetAllBuyer();

            var results = data.Select(der => new BuyerInfo
            {
                Address = der.Address,
                BidAmount = der.BidAmount,
                City = der.City,
                CreatedDate = System.DateTime.Now,
                Email = der.Email,
                FirstName = der.FirstName,
                LastName = der.LastName,
                Phone = der.Phone,
                PinCode = der.PinCode,
                ProductId = der.ProductId,
                State = der.State,
                BuyerId = der.BuyerId
            });

            return results;
        }
    }
}
