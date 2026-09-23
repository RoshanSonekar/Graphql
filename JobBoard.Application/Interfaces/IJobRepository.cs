using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
	public interface IJobRepository
	{
		IQueryable<Job> GetAllJobs();
		Task<Job?> GetJobById(int id);
		IQueryable<Job> GetJobByCompanyId(int companyId);
		IQueryable<JobApplication> GetApplicationsByJob(int jobId);
	}
}
