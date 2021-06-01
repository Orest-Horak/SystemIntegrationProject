using System;
using DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json.Linq;

namespace DataRetriever
{
    public class Program
    {
        static void Main(string[] args)
        {
            ////configuration files creation
            //JObject jObject = JObject.FromObject(new
            //{
            //    consumer = new
            //    {
            //        autoAck = true
            //    }
            //});


            //JsonFunctions.WriteTo(ConfigurationFilesRoutes.GetQueueConfigRoute(), jObject);
            //JsonFunctions.WriteTo(ConfigurationFilesRoutes.GetPublisherConfigRoute(), jObject);
            //JsonFunctions.WriteTo(ConfigurationFilesRoutes.GetConsumerConfigRoute(), jObject);




            //queue configuration
            var queueConfigurationJson = JsonFunctions.ReadFrom(ConfigurationFilesRoutes.GetQueueConfigRoute());
            var queueConfig = new QueueConfiguration(queueConfigurationJson);

            ////publisher
            //var publisherConfigurationJson = JsonFunctions.ReadFrom(ConfigurationFilesRoutes.GetPublisherConfigRoute());
            //var publisherConfig = new PublisherConfiguration(publisherConfigurationJson);

            //var publisher = new Publisher();
            //publisher.QueueConfiguration = queueConfig;
            //publisher.PublisherConfiguration = publisherConfig;

            ////crawl and publish
            //ICrawl crawler = new Nature_Crawler();
            //var list = crawler.Crawl();

            //foreach (var title in list)
            //{
            //    Console.WriteLine("Title: " + title.Title + Environment.NewLine
            //        + "Url: " + title.Url + Environment.NewLine
            //        + "Author: " + title.Author + Environment.NewLine
            //        + "Date of publication: " + title.DateOfPublication + Environment.NewLine);

            //    publisher.Publish(title);
            //}


            //consumer
            var consumerConfigurationJson = JsonFunctions.ReadFrom(ConfigurationFilesRoutes.GetConsumerConfigRoute());
            var consumerConfig = new ConsumerConfiguration(consumerConfigurationJson);

            var consumer = new Consumer();
            consumer.QueueConfiguration = queueConfig;
            consumer.ConsumerConfiguration = consumerConfig;
            consumer.MongoDb = new MongoCRUD("NewsCrawl");
            consumer.Consume();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
