using WebRtcServer.Models;

namespace WebRtcServer;

using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class VoiceChatHub : Hub
{
    public async Task Join(string remoteConnectionId, object answer)
    {
        // PeerConnection peerConnection = new PeerConnection();
        // PeerConnectionConfiguration config = new PeerConnectionConfiguration();
        // config.IceServers.Add(new IceServer { Urls = { "stun:localhost:3478" } });
        // peerConnection.Initialize(config);
        // PeerConnection.RtcOfferOptions offerOptions = new PeerConnection.RtcOfferOptions();
        // SdpCreateOfferResult offerResult = await peerConnection.CreateOffer(offerOptions);
        // if (offerResult.Offer != null)
        // {
        //     peerConnection.SetLocalDescription(offerResult.Offer);
        // }
        await Clients.Client(remoteConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
    }
    
    public async Task SendAnswer(string remoteConnectionId, object answer)
    {
        await Clients.Client(remoteConnectionId).SendAsync("ReceiveAnswer", Context.ConnectionId, answer);
    }

    public async Task SendIceCandidate(string remoteConnectionId, object iceCandidate)
    {
        await Clients.Client(remoteConnectionId).SendAsync("ReceiveIceCandidate", Context.ConnectionId, iceCandidate);
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        // var roomId = GetRoomIdForConnection(Context.ConnectionId);
        // await Groups.RemoveFromGroupAsync(Context.ConnectionId, roomId);

        // Notify other clients in the room about the user leaving
        // await Clients.Group(roomId).SendAsync("UserLeft", Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }

    
}
