using Microsoft.AspNetCore.Mvc;

namespace ImageOverlapApp.Controllers
{
	[ApiController]
	public class HealthCheckController : ControllerBase
	{
		[HttpGet("/")]
		public IActionResult Index()
		{
			return Ok("ImageOverlapApp está rodando.");
		}
	}
}