namespace JobBoard.Domain.Entities;

public class User
{
	public int Id { get; set; }
	public string Name { get; set; } = string.Empty;
	public string Email { get; set; }= string.Empty;
	public string Password { get; set; }=	string.Empty;
	public string? RefreshToken { get; set; }= string.Empty;
	public DateTime? RefreshTokenExpiry { get; set; }
	public ICollection<JobApplication> Applicationss { get; set; } = new List<JobApplication>();
}
