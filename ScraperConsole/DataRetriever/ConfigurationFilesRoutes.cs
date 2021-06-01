using System;

namespace DataRetriever
{
    public class ConfigurationFilesRoutes
    {
        public static string GetQueueConfigRoute()
        {
            return String.Format(@"{0}ConfigurationFiles\QueueConfig.txt",
                AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf("bin")));
        }
        public static string GetConsumerConfigRoute()
        {
            return String.Format(@"{0}ConfigurationFiles\ConsumerConfig.txt",
                AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf("bin")));
        }
        public static string GetPublisherConfigRoute()
        {
            return String.Format(@"{0}ConfigurationFiles\PublisherConfig.txt",
                AppDomain.CurrentDomain.BaseDirectory.Substring(0, AppDomain.CurrentDomain.BaseDirectory.LastIndexOf("bin")));
        }
    }
}
