using HotChocolate.Execution;

namespace JobBoard.API.Exceptions
{
	public class GraphQlErrorFilters: IErrorFilter	
	{
		public IError OnError(IError error)
		{
			if (error.Exception is not null)
				return error.WithMessage(error.Exception.Message);

			return error;
		 }
	}
}
