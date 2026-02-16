using EloChess.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using EloChess.Api.Repository;
using EloChess.Api.Infrastructure.Kafka;
using EloChess.Api.Hubs; // Dodaj ovo

var builder = WebApplication.CreateBuilder(args);

// 1. Osnovni servisi
builder.Services.AddControllers(); // Pozovi samo jednom
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "EloChess API", Version = "v1" });
});

// 2. Baza i Repozitorijumi
builder.Services.AddDbContext<EloChessDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IPlayerRepository, PlayerRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IMoveRepository, MoveRepository>();
builder.Services.AddScoped<IPlayerMatchRepository, PlayerMatchRepository>();

// 3. Infrastruktura (Kafka + SignalR)
builder.Services.AddKafkaInfrastructure(builder.Configuration);
builder.Services.AddSignalR();

var app = builder.Build();

// 4. Middleware (Redosled je bitan!)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "EloChess API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// 5. Mapiranje ruta (Sve mape idu ovde na kraj)
app.MapControllers();
app.MapHub<ChatHub>("/chathub"); // Ovde mu je pravo mesto

app.Run();