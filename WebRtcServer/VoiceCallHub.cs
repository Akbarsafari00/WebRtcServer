using Microsoft.AspNetCore.SignalR;

namespace WebRtcServer;

public class VoiceCallHub : Hub
{


    private readonly UsersStorage _connectionUserStorage;

    public VoiceCallHub(UsersStorage connectionUserStorage)
    {
        _connectionUserStorage = connectionUserStorage;
    }

    public override async Task OnConnectedAsync()
    {
        Console.WriteLine(Context.ConnectionId);
    }

    public override async Task OnDisconnectedAsync(Exception? exception) {
        var user = _connectionUserStorage.FirstOrDefault(x=>x.ConnectionId == Context.ConnectionId);
        if(user != null) {
            _connectionUserStorage.Remove(user);
        }
        

        await Clients.Group("eshgh").SendAsync("UsersInRoomUpdated", _connectionUserStorage);
    }

    public async Task JoinRoom(string name, string password)
    {
        if (password != "1")
        {
            await Clients.Caller.SendAsync("UserJoinedFailed");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, "eshgh");

        _connectionUserStorage.Add(new Models.HubUser {ConnectionId=Context.ConnectionId,Name= name});

        await Clients.Group("eshgh").SendAsync("UsersInRoomUpdated", _connectionUserStorage);
        await Clients.Caller.SendAsync("UserJoined", _connectionUserStorage);

    }

    public async Task Call(string targetId, object offer)
    {
        Console.WriteLine($"Call to {targetId}");
        await Clients.Client(targetId).SendAsync("CallReceived",Context.ConnectionId, offer);
    }

    public async Task CallAccepted(string sourceId, object answer)
    {
        Console.WriteLine($"CallAccepted from {sourceId}");
        await Clients.Client(sourceId).SendAsync("CallAccepted",Context.ConnectionId, answer);
        
    }
}
