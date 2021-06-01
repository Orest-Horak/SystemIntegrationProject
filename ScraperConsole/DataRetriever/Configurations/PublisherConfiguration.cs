using Newtonsoft.Json.Linq;

namespace DataRetriever
{
    public class PublisherConfiguration : Configuration
    {
        public PublisherConfiguration() : base()
        {

        }

        public PublisherConfiguration(JObject config) : base(config)
        {
        }
    }

}
