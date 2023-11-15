using System.Text;
using DAL.Entities;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace BLL
{
    public class MessageService
    {
        private readonly string _hostName = "localhost";
        private readonly string _exchange = "catalog";
        private readonly ConnectionFactory factory;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly ICartService _cartService;
        private readonly string queueName;

        private readonly TaskCompletionSource<bool> _messageReceived = new TaskCompletionSource<bool>();

        public MessageService(ICartService cartservice)
        {
            _cartService = cartservice;
            factory = new ConnectionFactory() { HostName = _hostName };
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.ExchangeDeclare(exchange: _exchange, type: ExchangeType.Fanout);

            // declare a server-named queue
            queueName = channel.QueueDeclare().QueueName;
            channel.QueueBind(queue: queueName,
                              exchange: _exchange,
                              routingKey: string.Empty);
        }
        public string Receive()
        {
            string result = string.Empty;
            Console.WriteLine(" [*] Waiting for logs.");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                byte[] body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var receivedItem = JsonConvert.DeserializeObject<MessageItem>(message);
                string response = upsertReceivedItem(receivedItem) == 1 ? "Success" : "Fail" ;
                Console.WriteLine($" Message received: {message}");
                Console.WriteLine($" Update response: {response}");
                result = response;

                // Signal that a message has been received
                _messageReceived.TrySetResult(true);
            };
            channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer);
            return result;
        }

        public Task WaitForMessage()
        {
            return _messageReceived.Task;
        }

        private int upsertReceivedItem(MessageItem messageItem)
        {
            Item item = new Item(messageItem);
            return _cartService.Insert(null, item);
        }
    }
}
