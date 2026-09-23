using JobBoard.Application.Interfaces;
using JobBoard.Domain.Entities;

namespace JobBoard.API.Mutation
{
	public class Mutation
	{
		// Add Job
		public async Task<Job> AddJob(AddJobRequest addJobRequest, [Service] IJobRepositoryMutation jobRepositoryMutation)
		{
			if (string.IsNullOrWhiteSpace(addJobRequest.Title))
				throw new GraphQLException("Title is required.");
			if (string.IsNullOrWhiteSpace(addJobRequest.Location))
				throw new GraphQLException("Location is required.");
			if (string.IsNullOrWhiteSpace(addJobRequest.JobType))
				throw new GraphQLException("Job type is required");
			if (addJobRequest.Salary <= 0)
				throw new GraphQLException("Salary is required.");

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
			if (addJobApplicationRequest.JobId<=0 || addJobApplicationRequest.UserId<=0)
				throw new GraphQLException("Job id and user id are required."); 

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

		// Update job application status
		public async Task<JobApplication> UpdateApplicationStatus(int applicationId, string status, [Service] IJobRepositoryMutation jobRepositoryMutation) 
		{
			if (string.IsNullOrEmpty(status))
				throw new GraphQLException("Status is required.");
			return await jobRepositoryMutation.UpdateApplicationStatus(applicationId, status);
		}
	}
}
