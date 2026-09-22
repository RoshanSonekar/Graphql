using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;

namespace JobBoard.API.Queries
{
	public class Query
	{
		public IQueryable<Job> GetJobs([Service] IJobRepository jobRepository)
		{
			return jobRepository.GetAllJobs();
		}
	}
}
