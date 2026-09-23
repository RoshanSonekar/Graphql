using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Infrastructure.Repositories;

public class JobRepository : IJobRepository
{
	private readonly AppDbContext appDbContext;
	public JobRepository(AppDbContext _appDbContext)
	{
		appDbContext = _appDbContext;
	}

	public IQueryable<Job> GetAllJobs()
	{
		return appDbContext.Jobs.Include(e => e.Company).OrderBy(o => o.Id);
	}
	public async Task<Job?> GetJobById(int id)
	{
		return await appDbContext.Jobs
			.Include(j => j.Company)
			.FirstOrDefaultAsync(j => j.Id == id);
	}

	public IQueryable<JobApplication> GetApplicationsByJob(int jobId)
	{
		return appDbContext.Applications
			.Where(a => a.JobId == jobId)
			.OrderBy(a => a.Id);
	}

	public IQueryable<Job> GetJobByCompanyId(int companyId)
	{
		return appDbContext.Jobs
			.Include(j => j.Company)
			.Where(j => j.CompanyId == companyId)
			.OrderBy(j => j.Id);
	}

	public async Task<User?> GetUserByEmail(string email)
	{ 
		return await appDbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
	}
}
