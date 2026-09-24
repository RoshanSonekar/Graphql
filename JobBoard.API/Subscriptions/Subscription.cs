using JobBoard.Domain.Entities;

namespace JobBoard.API.Subscriptions
{
	public class Subscription
	{
		[Subscribe]
		[Topic("JobPosted")]
		public Job OnJobPosted([EventMessage] Job job)
		{
			return job;
		}

		[Subscribe]
		[Topic("JobApplicationStatusChanged_{userId}")]
		public JobApplication OnJobApplicationStatusChanged(int userId, [EventMessage] JobApplication jobApplication)
		{
			return jobApplication;
		}
	}
}
