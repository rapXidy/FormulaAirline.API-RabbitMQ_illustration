// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, World!, Welcome to the ticketing service!");
var factory = new ConnectionFactory()
{
    HostName = "localhost",
    UserName = "user",
    Password = "mypass",
    VirtualHost="/"
};

var conn = factory.CreateConnection();

using var channel = conn.CreateModel(); //enables us create a new channel

channel.QueueDeclare("bookings", durable: true, exclusive: false);

var consumerr = new EventingBasicConsumer(channel);

consumerr.Received += (model, eventArgs) =>
{
    //getting my byte[]
    var body = eventArgs.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($@"New ticket procceing is initiated for - {message}");
}; // here we'll be having anonymous arguments

channel.BasicConsume("bookings",true,consumerr);

Console.ReadKey();