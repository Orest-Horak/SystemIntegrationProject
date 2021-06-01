using System;
using System.Collections.Generic;
using System.Text;
using DAL.Configuration;
using DAL.Entities;
using MongoDB.Driver;

namespace DAL
{
    public class DBContext
    {
        private readonly IMongoDatabase _database = null;

        public DBContext(MongoDbConfig config)
        {
            var client = new MongoClient(config.ConnectionString);
            if(client != null)
            {
                _database = client.GetDatabase(config.Database);
            }
        }

        public IMongoCollection<NewsEntity> News
        {
            get
            {
                return _database.GetCollection<NewsEntity>("News");
            }
        }
    }
}
