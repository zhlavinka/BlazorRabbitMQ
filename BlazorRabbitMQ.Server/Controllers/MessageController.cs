using BlazorRabbitConst = BlazorRabbitMQ.Shared.Constants;
using Microsoft.AspNetCore.Mvc;
using RabbitMQ.Client;
using System.Text;

namespace BlazorRabbitMQ.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MessagesController : ControllerBase
{
	private readonly IConnection _connection;

	public MessagesController(IConnection connection)
	{
		_connection = connection;
	}

	[HttpPost]
	public async Task<IActionResult> PostMessage(
		[FromBody] string message,
		CancellationToken cancellationToken)
	{
		var channel = await _connection.CreateChannelAsync();

		var body = Encoding.UTF8.GetBytes(message);

		await channel.BasicPublishAsync(
			exchange: "",
			routingKey: BlazorRabbitConst.RabbitMQ.Queue,
			body: body
		);

		return Ok();
	}
}

