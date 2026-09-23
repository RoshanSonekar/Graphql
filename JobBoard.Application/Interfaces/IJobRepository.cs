using JobBoard.Domain.Entities;

namespace JobBoard.Application.Interfaces;

public interface IJobRepository
{
	IQueryable<Job> GetAllJobs();
	Task<Job?> GetJobById(int id);
	IQueryable<Job> GetJobByCompanyId(int companyId);
	IQueryable<JobApplication> GetApplicationsByJob(int jobId);
	Task<User?> GetUserByEmail(string email);
}
