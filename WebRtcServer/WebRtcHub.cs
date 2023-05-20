using Microsoft.AspNetCore.SignalR;

namespace WebRtcServer
{

    public class WebRtcHub : Hub
    {

        public async Task SendSignal(object signal)
        {
            await Clients.Others.SendAsync("receiveSignal", signal);
        }
        
        public async Task JoinChatRoom(string roomName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task LeaveChatRoom(string roomName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomName);
        }

        public async Task SendOffer(string roomName, string targetConnectionId, string offer)
        {
            await Clients.Group(roomName).SendAsync("ReceiveOffer", Context.ConnectionId, targetConnectionId, offer);
        }

        public async Task SendAnswer(string roomName, string targetConnectionId, string answer)
        {
            await Clients.Group(roomName).SendAsync("ReceiveAnswer", Context.ConnectionId, targetConnectionId, answer);
        }

        public async Task SendIceCandidate(string roomName, string targetConnectionId, string candidate)
        {
            await Clients.Group(roomName).SendAsync("ReceiveIceCandidate", Context.ConnectionId, targetConnectionId, candidate);
        }
    }

}
