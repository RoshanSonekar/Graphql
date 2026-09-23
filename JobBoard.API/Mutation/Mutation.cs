using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;

namespace JobBoard.API.Mutation
{
	public class Mutation
	{
		// Add Job
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

		// Apply for an existing job by the user 
		public async Task<JobApplication> AppyForJob(AddJobApplication addJobApplicationRequest, [Service] IJobRepositoryMutation jobRepositoryMutation)
		{
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

		public async Task<JobApplication> UpdateApplicationStatus(int applicationId, string status, [Service] IJobRepositoryMutation jobRepositoryMutation) 
		{
			return await jobRepositoryMutation.UpdateApplicationStatus(applicationId, status);
		}
	}
}
