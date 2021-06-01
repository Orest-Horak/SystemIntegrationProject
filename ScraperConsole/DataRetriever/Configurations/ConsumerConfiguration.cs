using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataRetriever
{

    public class ConsumerConfiguration : Configuration
    {
        public ConsumerConfiguration() : base()
        {

        }

        public ConsumerConfiguration(JObject config) : base(config)
        {
        }
    }

}
