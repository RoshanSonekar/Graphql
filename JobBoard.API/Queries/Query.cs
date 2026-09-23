using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;

namespace JobBoard.API.Queries
{
	public class Query
	{
		[UsePaging]
		[UseProjection]
		[UseFiltering]
		[UseSorting]
		public IQueryable<Job> GetJobs([Service] IJobRepository jobRepository)
		{
			return jobRepository.GetAllJobs();
		}

		public async Task<Job?> GetJob(int jobId, [Service] IJobRepository jobRepository)
		{
			return await jobRepository.GetJobById(jobId);
		}

		[UseFiltering]
		[UseSorting]
		public IQueryable<Job> GetJobByCompanyId(int companyId, [Service] IJobRepository jobRepository)
		{
			return jobRepository.GetJobByCompanyId(companyId);
		}

		[UseFiltering]
		[UseSorting]
		public IQueryable<JobApplication> GetApplicationByJobId(int jobId, [Service] IJobRepository jobRepository)
		{
			return jobRepository.GetApplicationsByJob(jobId);
		}
	}
}
