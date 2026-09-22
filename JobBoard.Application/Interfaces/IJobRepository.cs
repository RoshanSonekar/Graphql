using JobBoard.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobBoard.Application.Interfaces
{
	public interface IJobRepository
	{
		IQueryable<Job> GetAllJobs();
	}
}
