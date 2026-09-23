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
}
