using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.Configuration
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
    }
}
