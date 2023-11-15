using System.Text;
using DAL.Entities;
using DAL.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BLL
{
    public class MessageService
    {
        private readonly string _hostName = "localhost";
        private readonly string _exchange = "catalog";
        private readonly ICartService _cartService;

        public MessageService(ICartService cartservice)
        {
            _cartService = cartservice;
        }
        public void Receive()
        {
            var factory = new ConnectionFactory() { HostName = _hostName };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

            // declare a server-named queue
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: _exchange,
                              routingKey: string.Empty);

            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var receivedItem = JsonConvert.DeserializeObject<MessageItem>(message);
                var response = upsertReceivedItem(receivedItem);
                Console.WriteLine($" [x] Received message: {message}");
                Console.WriteLine($" Update response: {response}");
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private int upsertReceivedItem(MessageItem messageItem)
        {
            Item item = new Item(messageItem);
            return _cartService.Insert(string.Empty, item);
        }
    }
}
