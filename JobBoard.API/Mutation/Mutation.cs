using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;

namespace JobBoard.API.Mutation
{
	public record AddJobRequest
		(
	string Title,
	string Description,
	decimal Salary,
	string Location,
	string JobType,
	int CompanyId
		);
	public class Mutation
	{
		
		public async Task<Job> AddJob(AddJobRequest addJobRequest, [Service] IJobRepositoryMutation jobRepositoryMutation)
		{
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
	}
}
