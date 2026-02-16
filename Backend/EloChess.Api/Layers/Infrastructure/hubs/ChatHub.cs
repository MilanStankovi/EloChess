using Microsoft.AspNetCore.SignalR;

namespace EloChess.Api.Hubs;

public class ChatHub : Hub
{
    // Ova metoda omogućava klijentima da se pridruže grupi (npr. određenoj partiji šaha)
    public async Task JoinRoom(string roomId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, roomId);
    }
}