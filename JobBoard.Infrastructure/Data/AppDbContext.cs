using Microsoft.EntityFrameworkCore;

namespace JobBoard.Infrastructure.Data
{
	public class AppDbContext :DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}
	}
}
