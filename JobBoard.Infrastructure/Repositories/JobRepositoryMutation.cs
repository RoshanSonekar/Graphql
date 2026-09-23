using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Data;

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
}
