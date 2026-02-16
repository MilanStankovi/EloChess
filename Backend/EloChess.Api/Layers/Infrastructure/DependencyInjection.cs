using EloChess.Api.Infrastructure.Kafka; // Osiguraj da je ovo tu
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EloChess.Api.Infrastructure.Kafka; // Isti namespace kao i klase iznad

public static class KafkaServiceExtensions
{
    public static IServiceCollection AddKafkaInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Ova linija mapira sekciju iz appsettings.json ili environment varijable
        services.Configure<KafkaSettings>(configuration.GetSection("Kafka"));
        
        services.AddSingleton<KafkaProducer>();
        services.AddHostedService<KafkaConsumerWorker>();
        
        return services;
    }
}