using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Rabbit.Models;
using Rabbit.Models.Entities;

var factory = new ConnectionFactory() {
    HostName = "localhost",
    UserName= "admin",
    Password= "123456",
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "rabbitMensagemQueue",
                     durable: false,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);


var consumer = new EventingBasicConsumer(channel);
consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var json = Encoding.UTF8.GetString(body);

    RabbitMensagem mensagem = JsonSerializer.Deserialize<RabbitMensagem>(json);

    Console.WriteLine($"Titulo: {mensagem.Nome}");
    Console.WriteLine($"Texto: {mensagem.Texto}");
};
channel.BasicConsume(queue: "rabbitMensagemQueue",
                     autoAck: true,
                     consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();
