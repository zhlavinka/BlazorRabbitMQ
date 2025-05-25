using BlazorRabbitMQ.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorRabbitMQ.Client.Pages;

public partial class Home : ComponentBase
{
	private HubConnection? _hubConnection;
	private List<string> messages = new();

	[Inject]
	NavigationManager Navigation { get; set; }

	protected override async Task OnInitializedAsync()
	{
		_hubConnection = new HubConnectionBuilder()
			.WithUrl(Navigation.ToAbsoluteUri("/chathub"))
			.WithAutomaticReconnect()
			.Build();

		_hubConnection.On<string>(Constants.ChatHubMethods.ReceiveMessage, message =>
		{
			messages.Add(message);
			InvokeAsync(StateHasChanged);
		});

		await _hubConnection.StartAsync();
	}
}
