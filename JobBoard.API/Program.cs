using JobBoard.API.Queries;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Add CORS services
//builder.Services.AddCors(options =>
//{
//	options.AddPolicy("AllowGraphQLUI", policy =>
//	{
//		policy.WithOrigins("https://localhost:7232", "http://localhost:5125", "https://cdn.bananacakepop.com") // Match your local ports
//					.AllowAnyHeader()
//					.AllowAnyMethod()
//					.AllowCredentials();
//	});
//});

// Add services to the container.
builder.Services.AddGraphQLServer().AddQueryType<Query>();
builder.Services.AddDbContext<AppDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline.
//app.UseCors("AllowGraphQLUI");
app.MapGraphQL();

app.Run();
