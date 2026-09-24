using HotChocolate.Authorization;
using HotChocolate.Subscriptions;
using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoard.API.Mutation
{
	public class Mutation
	{
		// Add Job
		//[Authorize]
		public async Task<Job> AddJob(AddJobRequest addJobRequest, 
			[Service] IJobRepositoryMutation jobRepositoryMutation,
			[Service] ITopicEventSender topicEventSender)
		{
			if (string.IsNullOrWhiteSpace(addJobRequest.Title))
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Title is required.")
					.SetCode("VALIDATION_ERROR")
					.Build());

			if (string.IsNullOrWhiteSpace(addJobRequest.Location))
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Location is required.")
					.SetCode("VALIDATION_ERROR")
					.Build());

			if (string.IsNullOrWhiteSpace(addJobRequest.JobType))
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Job type is required.")
					.SetCode("VALIDATION_ERROR")
					.Build());

			if (addJobRequest.CompanyId<=0)
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Company is required.")
					.SetCode("VALIDATION_ERROR")
					.Build());

			if (addJobRequest.Salary <= 0)
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Salary is required.")
					.SetCode("VALIDATION_ERROR")
					.Build());

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
			await topicEventSender.SendAsync("JobPosted", job);
			return job;
		}

		// Apply for an existing job by the user 
		[Authorize]
		public async Task<JobApplication> AppyForJob(AddJobApplication addJobApplicationRequest, [Service] IJobRepositoryMutation jobRepositoryMutation)
		{
			if (addJobApplicationRequest.JobId<=0 || addJobApplicationRequest.UserId<=0)
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Job id and user id are required.")
					.SetCode("VALIDATION_ERROR")
					.Build()); 

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
		//[Authorize]
		public async Task<JobApplication> UpdateApplicationStatus(int applicationId, string status, 
			[Service] IJobRepositoryMutation jobRepositoryMutation,
			[Service] ITopicEventSender topicEventSender) 
		{
			if (applicationId <= 0)
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Invalid application id.")
					.SetCode("VALIDATION_ERROR")
					.Build());

			if (string.IsNullOrEmpty(status))
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Status is required.")
					.SetCode("VALIDATION_ERROR")
					.Build());

			try
			{
				var application = await jobRepositoryMutation.UpdateApplicationStatus(applicationId, status);
				await topicEventSender.SendAsync($"JobApplicationStatusChanged_{application.UserId}", application);

				return application;
			}
			catch (Exception ex)
			{
				throw new GraphQLException(ex.Message);
			}
		}

		public async Task<LoginResponse> Login(string username, string password, 
			[Service]IJobRepository jobRepository, 
			[Service]IJobRepositoryMutation jobRepositoryMutation, 
			[Service] IConfiguration configuration)
		{
			var user = await jobRepository.GetUserByEmail(username);
			if (user is null)
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage($"User not found. User {username}")
					.SetCode("VALIDATION_ERROR")
					.Build()); 

			bool isValidPassword = BCrypt.Net.BCrypt.Verify(password, user.Password);
			if (!isValidPassword)
				throw new GraphQLException(ErrorBuilder.New()
					.SetMessage("Invalid password entered.")
					.SetCode("VALIDATION_ERROR")
					.Build());

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

			// generate refresh token and update user db
			var refreshToken = Guid.NewGuid().ToString();
			await jobRepositoryMutation.UpdateRefreshToken(user.Id, refreshToken, DateTime.UtcNow.AddDays(2));

			return new LoginResponse
			{
				RefreshToken=refreshToken, 
				Token=jwtToken
			};
		}

		public async Task<string> RefreshToken(string refreshToken, [Service] IJobRepository jobRepository, [Service] IConfiguration configuration)
		{
			var user = await jobRepository.GetUsersRefreshToken(refreshToken);
			if (user is null)
				throw new GraphQLException("Invalid refresh token");

			if (user.RefreshTokenExpiry < DateTime.UtcNow)
				throw new GraphQLException("Refresh token expired");

			// generate Jwt token
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
