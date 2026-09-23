using JobBoard.Domain.Entities;

namespace JobBoard.Application.Interfaces;

public interface IJobRepositoryMutation
{
	Task<Job> AddJob(Job job);
}
