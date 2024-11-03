using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbContexts;

namespace NorthwindApi;

public static class Program
{
	public static void Main(string[] args)
	{

		var builder = WebApplication.CreateBuilder(args);

		builder.Services.AddControllers();

		// Add services to the container.
		// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
		builder.Services.AddOpenApi();

		builder.Services.AddDbContext<NorthwindContext>(
			opts =>
			{
				var s1 = builder.Configuration["ConnectionStrings:DatabaseConnection"];
				var s2 = builder.Configuration["ConnectionStrings:LocalhostConnection"];
				opts.UseNpgsql(s1 ?? s2);
			});

		var app = builder.Build();

		// Configure the HTTP request pipeline.
		if(app.Environment.IsDevelopment())
		{
			app.MapOpenApi();
		}

		app.MapControllers();

		app.Run();
	}
}