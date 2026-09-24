using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Infrastructure.Repositories;

public class JobRepositoryMutation : IJobRepositoryMutation
{
	private readonly AppDbContext appDbContext;
	public JobRepositoryMutation(AppDbContext _appDbContext)
	{
		appDbContext = _appDbContext;
	}

	public async Task<Job> AddJob(Job job)
	{
		appDbContext.Jobs.Add(job);
		await appDbContext.SaveChangesAsync();

		return job;
	}

	public async Task<JobApplication> AddApplication(JobApplication jobApplication)
	{ 
		var application =  await appDbContext.Applications.FirstOrDefaultAsync(x=> x.JobId== jobApplication.JobId && x.UserId==jobApplication.UserId);
		if (application != null)
			throw new Exception($"Application exist provided job {jobApplication.JobId} for user {jobApplication.UserId}");

		appDbContext.Applications.Add(jobApplication);
		await appDbContext.SaveChangesAsync();

		return jobApplication;
	}

	public async Task<JobApplication> UpdateApplicationStatus(int applicationId, string status)
	{ 
		var application = await appDbContext.Applications.FirstOrDefaultAsync(x=> x.Id == applicationId);
		if (application == null)
			throw new Exception($"Application with id {applicationId} not found");

		application.Status = status;
		await appDbContext.SaveChangesAsync();

		return application;
	}

	public async Task UpdateRefreshToken(int userId, string refreshToken, DateTime refreshTokenExpiry)
	{
		var user = await appDbContext.Users.FirstOrDefaultAsync(u=> u.Id == userId);
		if (user is null)
			throw new Exception($"User with id {userId} not exist.");

		user.RefreshToken = refreshToken;
		user.RefreshTokenExpiry = refreshTokenExpiry;
		await appDbContext.SaveChangesAsync();
	}
}	
