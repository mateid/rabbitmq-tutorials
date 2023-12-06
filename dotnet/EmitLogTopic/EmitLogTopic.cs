using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "topic_logs", type: ExchangeType.Topic, durable: true);

// this would make the messages persistent across restarts of the broker
var properties = channel.CreateBasicProperties();
properties.Persistent = true;

var routingKey = (args.Length > 0) ? args[0] : "anonymous.info";
var message = (args.Length > 1)
                    ? string.Join(" ", args.Skip(1).ToArray())
                    : "Hello World!";
var body = Encoding.UTF8.GetBytes(message);
channel.BasicPublish(exchange: "topic_logs",
                     routingKey: routingKey,
                     basicProperties: properties, // use properties here to make the message persistent
                     body: body);
Console.WriteLine($" [x] Sent '{routingKey}':'{message}'");