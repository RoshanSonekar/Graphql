using JobBoard.Domain.Entities;

namespace JobBoard.Application.Interfaces;

public interface IJobRepositoryMutation
{
	Task<Job> AddJob(Job job);

	Task<JobApplication> AddApplication(JobApplication jobApplication);

	Task<JobApplication> UpdateApplicationStatus(int applicationId, string status);
}
