using HotChocolate.Authorization;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoard.API.Mutation
{
	public class Mutation
	{
		// Add Job
		[Authorize]
		public async Task<Job> AddJob(AddJobRequest addJobRequest, [Service] IJobRepositoryMutation jobRepositoryMutation)
		{
			if (string.IsNullOrWhiteSpace(addJobRequest.Title))
				throw new GraphQLException("Title is required.");
			if (string.IsNullOrWhiteSpace(addJobRequest.Location))
				throw new GraphQLException("Location is required.");
			if (string.IsNullOrWhiteSpace(addJobRequest.JobType))
				throw new GraphQLException("Job type is required");
			if (addJobRequest.Salary <= 0)
				throw new GraphQLException("Salary is required.");

			var job = new Job()
			{
				Title=addJobRequest.Title,
				Description=addJobRequest.Description,
				Salary=addJobRequest.Salary,
				Location=addJobRequest.Location,
				JobType=addJobRequest.JobType,
				CreatedAt = DateTime.UtcNow,
				CompanyId=addJobRequest.CompanyId
			};

			await jobRepositoryMutation.AddJob(job);
			return job;
		}

		// Apply for an existing job by the user 
		[Authorize]
		public async Task<JobApplication> AppyForJob(AddJobApplication addJobApplicationRequest, [Service] IJobRepositoryMutation jobRepositoryMutation)
		{
			if (addJobApplicationRequest.JobId<=0 || addJobApplicationRequest.UserId<=0)
				throw new GraphQLException("Job id and user id are required."); 

			var jobApplication = new JobApplication()
			{
				JobId = addJobApplicationRequest.JobId,
				UserId = addJobApplicationRequest.UserId,
				Status = "Applied",
				AppliedAt = DateTime.UtcNow
			};

			await jobRepositoryMutation.AddApplication(jobApplication);
			return jobApplication;

		}

		// Update job application status
		[Authorize]
		public async Task<JobApplication> UpdateApplicationStatus(int applicationId, string status, [Service] IJobRepositoryMutation jobRepositoryMutation) 
		{
			if (string.IsNullOrEmpty(status))
				throw new GraphQLException("Status is required.");
			return await jobRepositoryMutation.UpdateApplicationStatus(applicationId, status);
		}

		public async Task<string> Login(string username, string password, [Service]IJobRepository jobRepository, [Service] IConfiguration configuration)
		{
			var user = await jobRepository.GetUserByEmail(username);
			if (user is null)
				throw new GraphQLException($"User not found. User {username}");

			bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
			if (!isValidPassword)
				throw new GraphQLException("Invalid password entered.");

			var claims = new[]
			{
				new Claim("UserId", user.Id.ToString()),
				new Claim("Email", user.Email.ToString())
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSetting:Key"]!));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: configuration["JWTSetting:Issuer"],
				audience: configuration["JWTSetting:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(2),
				signingCredentials: credentials
				);

			var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);
			return jwtToken;
		}
	}
}
