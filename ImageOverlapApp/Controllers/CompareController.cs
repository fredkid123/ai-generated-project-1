using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImageOverlapApp.Services;

namespace ImageOverlapApp.Controllers
{
	[ApiController]
	[Route("compare")]
	public class CompareController : ControllerBase
	{
		private ILogger<CompareController> Logger { get; set; }
		private IImageComparisonService ImageComparisonService { get; set; }

		public CompareController(
			ILogger<CompareController> logger,
			IImageComparisonService imageComparisonService)
		{
			Logger = logger;
			ImageComparisonService = imageComparisonService;
		}

		[HttpPost("{instanceId}")]
		public IActionResult Compare(string instanceId)
		{
			var results = ImageComparisonService.CompareImages("groupA", "groupB", instanceId);
			return Ok(results);
		}
	}
}