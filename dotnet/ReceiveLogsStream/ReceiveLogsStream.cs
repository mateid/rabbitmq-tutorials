using System.Net;
using System.Text;
using RabbitMQ.Stream.Client;
using RabbitMQ.Stream.Client.Reliable;

const string stream = "event-stream";

var streamSystem = await StreamSystem.Create(
    new StreamSystemConfig
    {
        UserName = "guest",
        Password = "guest",
        Endpoints = new List<EndPoint>
        {
            new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5552)
        }
    }
).ConfigureAwait(false);

var consumer = await Consumer.Create(
    new ConsumerConfig(
        streamSystem,
        stream)
    {
        
        OffsetSpec = new OffsetTypeTimestamp(),
        MessageHandler = async (stream, consumer, context, message) =>
        {
            Console.WriteLine($"Received message: {Encoding.UTF8.GetString(message.Data.Contents)}");
            await Task.CompletedTask.ConfigureAwait(false);
        }
    }
).ConfigureAwait(false);

Console.ReadLine();

await consumer.Close().ConfigureAwait(false);
await streamSystem.Close().ConfigureAwait(false);