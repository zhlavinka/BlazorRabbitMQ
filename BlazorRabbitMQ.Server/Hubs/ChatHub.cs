using BlazorRabbitMQ.Shared;
using BlazorRabbitMQ.Shared.Models;
using Microsoft.AspNetCore.SignalR;

namespace BlazorRabbitMQ.Server.Hubs;

public class ChatHub : Hub
{
	public async Task SendMessage(ChatMessage msg)
	{
		await Clients.All.SendAsync(Constants.ChatHubMethods.ReceiveMessage, msg);
	}
}
