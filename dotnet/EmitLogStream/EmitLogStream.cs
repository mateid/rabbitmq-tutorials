using System.Net;
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

await streamSystem.CreateStream(new StreamSpec(stream));

var producer = await Producer.Create(
    new ProducerConfig(
        streamSystem,
        stream)
).ConfigureAwait(false);

await producer.Send(new Message("Hello world!"u8.ToArray()));

await producer.Close().ConfigureAwait(false);
await streamSystem.Close().ConfigureAwait(false);