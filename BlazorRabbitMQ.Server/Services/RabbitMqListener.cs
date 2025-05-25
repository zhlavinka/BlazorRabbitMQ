using Microsoft.AspNetCore.SignalR;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using BlazorRabbitMQ.Server.Hubs;
using BlazorRabbitConst = BlazorRabbitMQ.Shared.Constants;

namespace BlazorRabbitMQ.Server.Services;

public class RabbitMqListener : BackgroundService
{
	private readonly IHubContext<ChatHub> _hubContext;
	private IChannel? _channel;
	private IConnection? _connection;

	public RabbitMqListener(
		IHubContext<ChatHub> hubContext,
		IConnection connection)
	{
		_hubContext = hubContext;
		_connection = connection;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		_channel = await _connection.CreateChannelAsync();

		await _channel.QueueDeclareAsync(
			queue: BlazorRabbitConst.RabbitMQ.Queue,
			durable: false,
			exclusive: false,
			autoDelete: false,
			arguments: null);

		var consumer = new AsyncEventingBasicConsumer(_channel);
		consumer.ReceivedAsync += async (sender, eventArgs) =>
		{
			var body  = eventArgs.Body.ToArray();
			var message = Encoding.UTF8.GetString(body);

			// send to SignalR clients
			await _hubContext.Clients.All.SendAsync(BlazorRabbitConst.ChatHubMethods.ReceiveMessage, message, cancellationToken: stoppingToken);
		};

		await _channel.BasicConsumeAsync(queue: BlazorRabbitConst.RabbitMQ.Queue, autoAck: true, consumer: consumer);
	}

	public override void Dispose()
	{
		_channel?.Dispose();
		_connection?.Dispose();
		base.Dispose();
	}
}
