namespace JobBoard.Domain.Entities;
public class Job
{
	public int Id { get; set; }
	public string Title { get; set; } = string.Empty;
	public string Description { get; set; } = string.Empty;
	public decimal Salary { get; set; }
	public string Location { get; set; } = string.Empty;
	public string JobType { get; set; } = string.Empty;
	public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
	public int CompanyId { get; set; }
	public Company Company { get; set; } = null!;
	public ICollection<JobApplication> jobApplications { get; set; } = new List<JobApplication>();

}
