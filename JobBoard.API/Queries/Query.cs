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
			string str = BCrypt.Net.BCrypt.HashPassword("Roshan@#$12345");
			str = BCrypt.Net.BCrypt.HashPassword("Adira@#$12345");
			str = BCrypt.Net.BCrypt.HashPassword("Pooja@#$12345");
			str = BCrypt.Net.BCrypt.HashPassword("Rohit@#$12345"); 
			return jobRepository.GetAllJobs();
		}

		public async Task<Job> GetJob(int jobId, [Service] IJobRepository jobRepository)
		{
			var job=  await jobRepository.GetJobById(jobId);
			if (job is null)
				throw new GraphQLException($"Job for given id {jobId} not found.");
			
			return job;
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
