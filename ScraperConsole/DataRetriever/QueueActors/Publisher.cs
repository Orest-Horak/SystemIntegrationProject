using DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace DataRetriever
{
    public class Publisher
    {
        private QueueConfiguration queueConfiguration;
        private PublisherConfiguration publisherConfiguration;

        public QueueConfiguration QueueConfiguration { get => queueConfiguration; set => queueConfiguration = value; }
        public PublisherConfiguration PublisherConfiguration { get => publisherConfiguration; set => publisherConfiguration = value; }

        public void Publish(object objectToPublish)
        {
            // встановлення зв"язку з брокером на локальній машині (налаштування сокетів, встановлення протоколу, аутентифікація)
            var factory = new ConnectionFactory()
            {
                HostName = (string)queueConfiguration.Config["factory"]["HostName"]
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // вся комунікація відбувається через чергу
                // створення черги, якщо такої ще немає, або використання існуючої
                channel.QueueDeclare(queue: (string)queueConfiguration.Config["queue"]["queue"],
                                     durable: (bool)queueConfiguration.Config["queue"]["durable"],
                                     exclusive: (bool)queueConfiguration.Config["queue"]["exclusive"],
                                     autoDelete: (bool)queueConfiguration.Config["queue"]["autoDelete"],
                                     arguments: null);

                //Example #1
                //string message = "Hello World!";
                //var body = Encoding.UTF8.GetBytes(message);

                //// публікація повідомлення у відповідну чергу
                //channel.BasicPublish(exchange: "",
                //                     routingKey: "hello",
                //                     basicProperties: null,
                //                     body: body);
                //Console.WriteLine(" [x] Sent {0}", message);

                ////Example #2
                //var n = new NewsDTO
                //{
                //    Author = "Test Name",
                //    Title = "Some Title",
                //    Description = "Very long description",
                //    Url = "http://localhost/test",
                //    DateOfPublication = DateTime.Now.Subtract(new TimeSpan(1))
                //};
                ////length = 360
                //channel.BasicPublish(exchange: "",
                //                     routingKey: "hello",
                //                     basicProperties: null,
                //                     body: BinaryConverter.ObjectToByteArray(n));
                //Console.WriteLine(" [x] Sent \n {0}", n);

                ////Example #3
                //var n = new NewsDTO
                //{
                //    Author = "Test Name",
                //    Title = "Some Title",
                //    Description = "Very long description",
                //    Url = "http://localhost/test",
                //    DateOfPublication = DateTime.Now.Subtract(new TimeSpan(1))
                //};

                //message body
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(objectToPublish));

                var properties = channel.CreateBasicProperties();
                properties.Persistent = (bool)publisherConfiguration.Config["publisher"]["Persistent"];

                channel.BasicPublish(exchange: "",
                                     routingKey: (string)queueConfiguration.Config["queue"]["queue"],
                                     basicProperties: properties,
                                     body: body);
                Console.WriteLine(" [x] Sent \n {0}", objectToPublish);
            }

        }
    }
}
