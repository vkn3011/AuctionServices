using EAuction.Domain.Buyer;
using EAuction.Domain.Product;
using EAuction.Domain.Seller;
using EAuction.Service;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EAuction.DataAccessProvider
{
    public class MongoDataAccessProvider:IDataAccessProvider
    {
        private readonly DataContext _context;
        private readonly ILogger _logger;



        public MongoDataAccessProvider(IConfiguration configRoot, ILoggerFactory loggerFactory)
        {
            _context = new DataContext(configRoot);
            _logger = loggerFactory.CreateLogger("DataAccessMongoProvider");
        }



        //E-Auction Methods Start here

        public async Task<Seller> AddSeller(Seller sellerRecord)
        {
            await _context.SellerInfo.InsertOneAsync(sellerRecord);
            return sellerRecord;
        }

        public async Task<List<Seller>> GetAllSeller()
        {
            return await _context.SellerInfo.Find(_ => true).ToListAsync();
        }

        public async Task<Product> AddProduct(Product productRecord)
        {
            await _context.ProductInfo.InsertOneAsync(productRecord);
            return productRecord;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.ProductInfo.Find(a => a.IsDeleted == false).ToListAsync();
        }

        public async Task<Buyer> AddBuyer(Buyer buyerRecord)
        {
            await _context.BuyerInfo.InsertOneAsync(buyerRecord);
            return buyerRecord;
        }

        public async Task<List<Buyer>> GetAllBuyer()
        {

            return await _context.BuyerInfo.Find(_ => true).ToListAsync();
        }

        public async Task UpdateBid(int productId, string buyerEmailId, double newBidAmt)
        {
            var updateRecored = _context.BuyerInfo.Find(a => a.ProductId == productId && a.Email == buyerEmailId).SingleOrDefault();
            updateRecored.BidAmount = newBidAmt;
            await _context.BuyerInfo.UpdateOneAsync(Builders<Buyer>.Filter.Where(a => a.ProductId == productId && a.Email == buyerEmailId), Builders<Buyer>.Update.Set(x => x.BidAmount, newBidAmt));

        }

        public async Task<Product> GetProductById(int productId)
        {
            return await _context.ProductInfo
                               .Find(t => t.ProductId == productId && t.IsDeleted == false).FirstOrDefaultAsync();
        }

        public async Task<List<Buyer>> GetAllBidsByProductId(int productId)
        {
            return await _context.BuyerInfo.Find(a => a.ProductId == productId)
                              .ToListAsync();
        }

        public async Task<bool> ExistsProducts(long id)
        {
            var filteredDataEventRecords = _context.ProductInfo
                .Find(item => item.ProductId == id);

            return await filteredDataEventRecords.AnyAsync();
        }

        public async Task DeleteProduct(long productId)
        {
            var entity = _context.ProductInfo.Find(t => t.ProductId == productId).FirstOrDefault();
            await _context.ProductInfo.DeleteOneAsync(Builders<Product>.Filter.Eq("ProductId", productId));
        }

        private ObjectId GetInternalId(string id)
        {
            if (!ObjectId.TryParse(id, out var internalId))
            {
                internalId = ObjectId.Empty;
            }

            return internalId;
        }
    }
}
