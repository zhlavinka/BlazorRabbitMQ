using BlazorRabbitConst = BlazorRabbitMQ.Shared.Constants;
using BlazorRabbitMQ.Server.Components;
using BlazorRabbitMQ.Server.Hubs;
using BlazorRabbitMQ.Server.Services;
using RabbitMQ.Client;

namespace BlazorRabbitMQ.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorComponents()
            .AddInteractiveWebAssemblyComponents();

        builder.Services.AddControllers();

        builder.Services.AddSingleton<IConnection>(sp => {
			var factory = new ConnectionFactory {
				HostName = BlazorRabbitConst.RabbitMQ.HostName,
				Port = BlazorRabbitConst.RabbitMQ.Port,
				UserName = BlazorRabbitConst.RabbitMQ.Username,
				Password = BlazorRabbitConst.RabbitMQ.Password
			};
			return factory.CreateConnectionAsync().GetAwaiter().GetResult();
		});

        builder.Services.AddSignalR();
        builder.Services.AddHostedService<RabbitMqListener>();

        var app = builder.Build();

        app.MapHub<ChatHub>(BlazorRabbitConst.RabbitMQ.RoutePattern);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseWebAssemblyDebugging();
        }
        else
        {
            app.UseExceptionHandler("/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        app.UseHttpsRedirection();

        app.UseAntiforgery();

        app.MapStaticAssets();

        app.MapRazorComponents<App>()
            .AddInteractiveWebAssemblyRenderMode()
            .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

        app.MapControllers();

        app.Run();
    }
}
