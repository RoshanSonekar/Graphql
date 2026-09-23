using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Infrastructure.Data
{
	public class AppDbContextSeeder
	{
		public static async Task SeedAsync(AppDbContext context)
		{
			// if record already exist then do not add
			if (context.Companies.Any())
				return;

			// Add Componies
			var companies = new List<Company>
			{
				new Company { Name="MultiChoice", Email="info@multichoice.co.za",Location="Randburg"},
				new Company { Name="Nihilent", Email="info@Nihilent.com",Location="Sandton"},
				new Company { Name="Tata", Email="info@Tata.com",Location="Fourways"},
				new Company { Name="Zensar", Email="info@Zensar.co.za",Location="Capetown"},
				new Company { Name="Microsoft", Email="info@Microsoft.co.za",Location="London"},
				new Company { Name="Google", Email="info@Google.co.za",Location="NYC"}
			};

			await context.Companies.AddRangeAsync(companies);
			await context.SaveChangesAsync();

			// Add Users 
			var users = new List<User>
			{
				new User { Name="Roshan", Email="Roshan@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("Roshan123")},
				new User { Name="Adira", Email="Adira@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("Adira123")},
				new User { Name="Pooja", Email="Pooja@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("Pooja123")},
				new User { Name="Rohit", Email="Rohit@gmail.com", Password=BCrypt.Net.BCrypt.HashPassword("Rohit123")}
			};
			await context.Users.AddRangeAsync(users);
			await context.SaveChangesAsync();

			// Add Jobs
			var job = new List<Job>
			{
				new Job { Title = ".Net Developer", Description = "12 Months Contract.Net C# Developer", Salary = 60000, Location ="Randburg",
					JobType="Contract", CreatedAt=DateTime.UtcNow, CompanyId=companies[0].Id},

				new Job { Title = "PHP Developer", Description = "12 Months Contract PHP Developer", Salary = 55000, Location ="Randburg",
					JobType="Contract", CreatedAt=DateTime.UtcNow, CompanyId=companies[1].Id},

				new Job { Title = "Azure Developer", Description = "Azure Developer", Salary = 75000, Location ="Sandton",
					JobType="Permanent", CreatedAt=DateTime.UtcNow, CompanyId=companies[2].Id},

				new Job { Title = ".Net Developer", Description = "12 Months Contract.Net C# Developer", Salary = 60000, Location ="Santon",
					JobType="Contract", CreatedAt=DateTime.UtcNow, CompanyId=companies[2].Id},

				new Job { Title = "SQL DBA", Description = "12 Months Contract SQL DBA", Salary = 80000, Location ="NYC",
					JobType="Contract", CreatedAt=DateTime.UtcNow, CompanyId=companies[1].Id},

				new Job { Title = "Architect", Description = "Cloud Architect", Salary = 85000, Location ="London",
					JobType="Permanent", CreatedAt=DateTime.UtcNow, CompanyId=companies[3].Id},

				new Job { Title = "Front End Dev", Description = "React, Amber JS, CSS", Salary = 40000, Location ="Fourways",
					JobType="Contract", CreatedAt=DateTime.UtcNow, CompanyId=companies[4].Id}
			};

			await context.Jobs.AddRangeAsync(job);
			await context.SaveChangesAsync();
		}
	}
}
