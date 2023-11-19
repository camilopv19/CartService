using System.Text;
using System.Threading.Channels;
using DAL.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BLL
{
    public class RabbitMqConsumer:IMessageConsumer
    {
        private readonly ICartService _cartService;
        public RabbitMqConsumer(ICartService cartservice)
        {
            _cartService = cartservice;
        }
        public void StartConsuming()
        {
            string _hostName = "localhost";
            string _exchange = "catalog";
            var factory = new ConnectionFactory() { HostName = _hostName };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

            // declare a server-named queue
            var queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: _exchange,
                              routingKey: string.Empty);
            string result = string.Empty;
            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var receivedItem = JsonConvert.DeserializeObject<MessageItem>(message);
                string response = upsertReceivedItem(receivedItem) == 1 ? "Success" : "Fail";
                Console.WriteLine($" Message received: {message}");
                Console.WriteLine($" Update response: {response}");
                //result = response;
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
        }

        private int upsertReceivedItem(MessageItem messageItem)
        {
            Item item = new Item(messageItem);
            return _cartService.Insert(item.CartId, item);
        }
    }
}
