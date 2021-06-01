using Newtonsoft.Json.Linq;

namespace DataRetriever
{
    public class QueueConfiguration : Configuration
    {
        public QueueConfiguration(): base()
        {

        }

        public QueueConfiguration(JObject config): base(config)
        {
        }
    }

}
