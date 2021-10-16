using Newtonsoft.Json;
using poc_csharp_console_app_socket_io.Dto;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace poc_csharp_console_app_socket_io
{
    class Program
    {
        public static void execPublish(MessageInputDto message)
        {
            ConnectionFactory _factory = new ConnectionFactory {
                HostName = "localhost"
            };

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "Fila_Console_APP",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var stringfiedMessage = JsonConvert.SerializeObject(message);
                    var byteMessage = Encoding.UTF8.GetBytes(stringfiedMessage);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: "Fila_Console_APP",
                        basicProperties: null,
                        body: byteMessage
                    );
                }
            }
            Console.WriteLine(" [x] Sent {0}", message.Content);
        }

        public static void execReceiver()
        {
            ConnectionFactory _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: "Fila_Console_APP",
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body.ToArray();
                        var stringfiedMessage = Encoding.UTF8.GetString(body);
                        var message = JsonConvert.DeserializeObject<MessageInputDto>(stringfiedMessage);
                        Console.WriteLine(" [x] Received {0}", message.Content);
                    };
                    channel.BasicConsume(queue: "Fila_Console_APP",
                                         autoAck: true,
                                         consumer: consumer);

                    Console.WriteLine(" Press [enter] to exit.");
                    Console.ReadLine();
                }
            }
        }

        static void Main(string[] args)
        {
            var message = new MessageInputDto
            {
                Content = "Conteudo da mensagem",
            };

			execPublish(message);

            execReceiver();

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}
