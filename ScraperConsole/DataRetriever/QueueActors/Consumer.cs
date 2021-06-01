using DTO;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace DataRetriever
{
    public class Consumer
    {
        private QueueConfiguration queueConfiguration;
        private ConsumerConfiguration consumerConfiguration;
        private MongoCRUD mongoDb;

        public QueueConfiguration QueueConfiguration { get => queueConfiguration; set => queueConfiguration = value; }
        public ConsumerConfiguration ConsumerConfiguration { get => consumerConfiguration; set => consumerConfiguration = value; }
        public MongoCRUD MongoDb { get => mongoDb; set => mongoDb = value; }

        public void Consume()
        {
            var factory = new ConnectionFactory()
            {
                HostName = (string)queueConfiguration.Config["factory"]["HostName"]
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: (string)queueConfiguration.Config["queue"]["queue"],
                                     durable: (bool)queueConfiguration.Config["queue"]["durable"],
                                     exclusive: (bool)queueConfiguration.Config["queue"]["exclusive"],
                                     autoDelete: (bool)queueConfiguration.Config["queue"]["autoDelete"],
                                     arguments: null);

                var consumer = new EventingBasicConsumer(channel);

                //Example #1
                //коллбек для асинхронної нотифікації про отримання повідомлень
                //consumer.Received += (model, ea) =>
                //{
                //    var body = ea.Body;
                //    var message = Encoding.UTF8.GetString(body);
                //    Console.WriteLine(" [x] Received {0}", message);
                //};

                ////Example #2
                //consumer.Received += (model, ea) =>
                //{
                //    var body = ea.Body;
                //    NewsDTO n = BinaryConverter.ByteArrayToObject(body) as NewsDTO;
                //    Console.WriteLine(" [x] Received \n {0}", n);
                //};

                //Example #3
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    NewsDTO n = JsonConvert.DeserializeObject<NewsDTO>(Encoding.UTF8.GetString(body.ToArray()));
                    mongoDb.InsertRecord("News", n);
                    Console.WriteLine(" [x] Received \n {0}", n);
                };

                //// споживач працює неперервно
                channel.BasicConsume(queue: (string)queueConfiguration.Config["queue"]["queue"],
                                     autoAck: (bool)consumerConfiguration.Config["consumer"]["autoAck"],
                                     consumer: consumer);
            }
        }
    }
}
