namespace BlazorRabbitMQ.Shared;

public static class Constants
{
	public static class ChatHubMethods
	{
		public const string ReceiveMessage = "ReceiveMessage";
	}

	public static class RabbitMQ
	{
		public const string HostName = "localhost";
		public const int Port = 5672;
		public const string Username = "guest";
		public const string Password = "guest";

		public const string Queue = "ChatQueue";
		public const string RoutePattern = "/chathub";
	}
}
