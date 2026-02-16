using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace EloChess.Api.Infrastructure.Kafka;

public class KafkaProducer
{
    private readonly ProducerConfig _config;
    private readonly string _topic;

    public KafkaProducer(IOptions<KafkaSettings> settings)
    {
        _topic = settings.Value.Topic;
        _config = new ProducerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers,
            SecurityProtocol = SecurityProtocol.Ssl,
            SslCaLocation = settings.Value.SslCaLocation,
            SslCertificateLocation = settings.Value.SslCertificateLocation,
            SslKeyLocation = settings.Value.SslKeyLocation
        };
    }

    public async Task ProduceAsync(string key, string message)
    {
        using var producer = new ProducerBuilder<string, string>(_config).Build();
        await producer.ProduceAsync(_topic, new Message<string, string> { Key = key, Value = message });
    }
}