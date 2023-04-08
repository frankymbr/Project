using System.Net;

namespace Project_API.Models
{
	public class APIResponse
	{
		public HttpStatusCode StatusCode { get; set; }
		public bool IsExistoso { get; set; } = true;
		public List<string> ErrorMessages { get; set; }
		public object Resultado { get; set; }
	}
}
