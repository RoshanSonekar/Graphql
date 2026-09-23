using JobBoard.API.Mutation;
using JobBoard.API.Queries;
using JobBoard.Application.Interfaces;
using JobBoard.Infrastructure.Data;
using JobBoard.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

// Add JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["JWTSetting:Issuer"],
			ValidAudience = builder.Configuration["JWTSetting:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTSetting:Key"]!))
		};
	});
builder.Services.AddAuthorization();

// Add services to the container.
builder.Services.AddGraphQLServer()
	.AddQueryType<Query>()
	.AddMutationType<Mutation>()
	.AddFiltering()
	.AddSorting()
	.AddProjections()
	.AddAuthorization();

// Add db context
builder.Services.AddDbContext<AppDbContext>(options =>
		options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add service
builder.Services.AddScoped<IJobRepository, JobRepository>();
builder.Services.AddScoped<IJobRepositoryMutation,  JobRepositoryMutation>();



var app = builder.Build();

// Seeding
using (var scope = app.Services.CreateScope())
{
	var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
	await AppDbContextSeeder.SeedAsync(context);
}


// Configure the HTTP request pipeline.
app.UseAuthentication();
app.UseAuthorization();
//app.UseCors("AllowGraphQLUI");
app.MapGraphQL();

app.Run();
