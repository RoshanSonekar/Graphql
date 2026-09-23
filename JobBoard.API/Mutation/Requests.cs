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

	public record AddJobApplication
		(
	int JobId,
	int UserId 
		);
}
