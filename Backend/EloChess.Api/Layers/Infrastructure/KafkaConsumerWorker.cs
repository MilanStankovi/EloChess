using Confluent.Kafka;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
using EloChess.Api.Hubs;
namespace EloChess.Api.Infrastructure.Kafka;

public class KafkaConsumerWorker : BackgroundService
{
    private readonly IHubContext<ChatHub> _hubContext;
    private readonly ConsumerConfig _config;
    private readonly string _topic;

    public KafkaConsumerWorker(IOptions<KafkaSettings> settings, IHubContext<ChatHub> hubContext)
    {
        _hubContext = hubContext;
        _topic = settings.Value.Topic; //

        string computerName = Environment.MachineName; 
        string uniqueGroupId = $"elochess-group-{computerName}";

        _config = new ConsumerConfig
        {
            BootstrapServers = settings.Value.BootstrapServers, //
            GroupId = uniqueGroupId, //
            AutoOffsetReset = AutoOffsetReset.Earliest, //
            SecurityProtocol = SecurityProtocol.Ssl, //
            SslCaLocation = settings.Value.SslCaLocation, //
            SslCertificateLocation = settings.Value.SslCertificateLocation, //
            SslKeyLocation = settings.Value.SslKeyLocation //
        };
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Oslobađamo glavnu nit aplikacije tako što pokrećemo Consumer u pozadinskom Tasku
        return Task.Run(() => StartConsumer(stoppingToken), stoppingToken);
    }

    private async Task StartConsumer(CancellationToken stoppingToken)
    {
        using var consumer = new ConsumerBuilder<string, string>(_config).Build();
        consumer.Subscribe(_topic);

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // Ova operacija blokira nit dok se ne pojavi nova poruka
                    var result = consumer.Consume(stoppingToken);
                    
                    // 1. Logovanje u konzolu servera
                    Console.WriteLine($"[KAFKA PRIMLJENO]: {result.Message.Key}: {result.Message.Value}");

                    // 2. Slanje poruke svim povezanim klijentima u realnom vremenu preko SignalR-a
                    await _hubContext.Clients.All.SendAsync("ReceiveMessage", 
                        result.Message.Key ?? "Sistem", // Korisničko ime (Key iz Kafke)
                        result.Message.Value,          // Sadržaj poruke (Value iz Kafke)
                        stoppingToken);
                }
                catch (ConsumeException e)
                {
                    Console.WriteLine($"Kafka Error (Consume): {e.Error.Reason}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Kafka Error (General): {ex.Message}");
                }
            }
        }
        catch (OperationCanceledException)
        {
            // Normalno gašenje kada se aplikacija zaustavi
            Console.WriteLine("Kafka Consumer se gasi...");
        }
        finally
        {
            consumer.Close();
        }
    }
}