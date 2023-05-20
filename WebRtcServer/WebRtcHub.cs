using Microsoft.AspNetCore.SignalR;

namespace WebRtcServer;

public class WebRtcHub : Hub
{


    private readonly UsersStorage _connectionUserStorage;

    public WebRtcHub(UsersStorage connectionUserStorage)
    {
        _connectionUserStorage = connectionUserStorage;
    }

    public override async Task OnConnectedAsync()
    {
    }

    public override async Task OnDisconnectedAsync(Exception? exception) {
        var user = _connectionUserStorage.FirstOrDefault(x=>x.ConnectionId == Context.ConnectionId);
        if(user != null) {
            _connectionUserStorage.Remove(user);
        }
        await Clients.Group("eshgh").SendAsync("UsersInRoomUpdated", _connectionUserStorage);
    }

    [HubMethodName("room:join")]
    public async Task JoinRoom(string name, string password)
    {
        Console.WriteLine($"[{Context.ConnectionId}][room:join] : {name}");
        if (password != "1")
        {
            await Clients.Caller.SendAsync("room:join:failed");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, "eshgh");

        _connectionUserStorage.Add(new Models.HubUser {ConnectionId=Context.ConnectionId,Name= name});

        await Clients.Group("eshgh").SendAsync("room:users:update", _connectionUserStorage);
        await Clients.Caller.SendAsync("room:join:success", _connectionUserStorage);

    }

    [HubMethodName("call:make")]
    public async Task MakeCall(string to, object offer)
    {
        Console.WriteLine($"[{Context.ConnectionId}][call:make] : {to}");
        await Clients.Client(to).SendAsync("call:received",Context.ConnectionId, offer);
    }

    [HubMethodName("call:accept")]
    public async Task CallAccepted(string to, object answer)
    {
        Console.WriteLine($"[{Context.ConnectionId}][call:accept] : {to}");
        await Clients.Client(to).SendAsync("call:accepted",Context.ConnectionId, answer);
        
    }
    
    [HubMethodName("call:nego:needed")]
    public async Task CallNegoNeeded(string to, object offer)
    {
        Console.WriteLine($"[{Context.ConnectionId}][call:nego:needed] : {to}");
        await Clients.Client(to).SendAsync("call:nego:needed:incoming",Context.ConnectionId, offer);
    }

    [HubMethodName("call:nego:done")]
    public async Task CallNegoDone(string to, object answer)
    {
        Console.WriteLine($"[{Context.ConnectionId}][call:nego:done] : {to}");
        await Clients.Client(to).SendAsync("call:nego:finished",Context.ConnectionId, answer);
        
    }
}
