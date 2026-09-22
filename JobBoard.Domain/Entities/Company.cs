namespace JobBoard.Domain.Entities
{
	public class Company
	{
		public int Id { get; set; }
		public string Name { get; set; } = string.Empty;
		public string Email { get; set; } = string.Empty;
		public string Location { get; set; } = string.Empty;

		public ICollection<Job> Jobs { get; } = new List<Job>();
	}
}
