using Microsoft.AspNetCore.SignalR;

namespace TrackMS.WebAPI.Features.Tracking;

public class TrackingHub : Hub
{
    public async Task Send(string message)
    {
        await Clients.All.SendAsync("Receive", message);
    }
}
