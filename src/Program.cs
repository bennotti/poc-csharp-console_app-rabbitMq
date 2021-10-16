using Newtonsoft.Json;
using poc_csharp_console_app_socket_io.Dto;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace poc_csharp_console_app_socket_io
{
    class Program
    {
        public static void exec(MessageInputDto message)
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
        }

        static void Main(string[] args)
        {
            var message = new MessageInputDto
            {
                Content = "Conteudo da mensagem",
            };

			exec(message);

            Console.WriteLine("Hello World!");
            Console.ReadLine();
        }
    }
}
