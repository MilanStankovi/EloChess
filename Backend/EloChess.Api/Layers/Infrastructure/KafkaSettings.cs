namespace EloChess.Api.Infrastructure.Kafka;

public class KafkaSettings
{
    public string BootstrapServers { get; set; } = string.Empty;
    public string Topic { get; set; } = string.Empty;
    public string SslCaLocation { get; set; } = string.Empty;
    public string SslCertificateLocation { get; set; } = string.Empty;
    public string SslKeyLocation { get; set; } = string.Empty;
}