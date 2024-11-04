using Microsoft.EntityFrameworkCore;
using NorthwindApi.DbContexts;
using System.Text.Json.Serialization;

namespace NorthwindApi;

public static class Program
{
	public static void Main(string[] args)
	{
		var builder = WebApplication.CreateSlimBuilder(args);

		builder.Services.AddControllers().AddJsonOptions(opts =>
		{
			opts.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		});

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

		app.Services.CreateScope().ServiceProvider.GetRequiredService<NorthwindContext>()
			.Database.ExecuteSqlRaw("""
				CREATE OR REPLACE FUNCTION GetSuppByParams(minId int, maxId int) 
				RETURNS TABLE (s_id INT2, s_name VARCHAR(30)) AS $$
				BEGIN
					RETURN QUERY SELECT supplier_id, contact_name FROM suppliers WHERE supplier_id BETWEEN minId AND maxId;
				END;
				$$ LANGUAGE plpgsql
			""");

		app.Run();
	}
}