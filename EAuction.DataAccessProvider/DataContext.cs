using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using EAuction.Domain.Seller;
using EAuction.Domain.Buyer;
using EAuction.Domain.Product;

namespace EAuction.DataAccessProvider
{
    public class DataContext
    {
        private readonly IMongoDatabase _database;
        public DataContext(IConfiguration configRoot)
        {
            var client = new MongoClient(configRoot.GetSection("MongoConnection").GetValue<string>("ConnectionString"));
            _database = client.GetDatabase(configRoot.GetSection("MongoConnection").GetValue<string>("DatabaseName"));
        }

        public IMongoCollection<Seller> SellerInfo => _database.GetCollection<Seller>("Coln_Seller");
        public IMongoCollection<Buyer> BuyerInfo => _database.GetCollection<Buyer>("Coln_Buyer");
        public IMongoCollection<Product> ProductInfo => _database.GetCollection<Product>("Coln_Product");
    }
}
