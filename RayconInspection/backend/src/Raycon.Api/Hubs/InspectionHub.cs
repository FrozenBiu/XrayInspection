using Microsoft.AspNetCore.SignalR;

namespace Raycon.Api.Hubs;

public class InspectionHub : Hub
{
    public async Task JoinMachineGroup(string machineId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, machineId);
    }
}

