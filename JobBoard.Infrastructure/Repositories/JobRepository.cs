using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;
using JobBoard.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace JobBoard.Infrastructure.Repositories
{
	public class JobRepository :IJobRepository
	{
		private readonly AppDbContext appDbContext;
		public JobRepository(AppDbContext _appDbContext) 
		{
			appDbContext = _appDbContext;
		}

		public IQueryable<Job> GetAllJobs()
		{
			return appDbContext.Jobs.Include(e=> e.Company).OrderBy(o=> o.Id);
		}
	}
}
