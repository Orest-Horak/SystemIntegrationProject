using Newtonsoft.Json.Linq;

namespace DataRetriever
{
    public class Configuration
    {
        protected JObject config;
        public JObject Config { get => config; set => config = value; }

        public Configuration()
        {
            config = null;
        }

        public Configuration(JObject config)
        {
            this.config = config;
        }

        public JObject GetConfig()
        {
            return config;
        }
    }

}
