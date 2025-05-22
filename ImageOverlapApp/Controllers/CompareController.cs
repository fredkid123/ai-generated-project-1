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
		private IPathService PathService { get; set; }

		public CompareController(
			ILogger<CompareController> logger,
			IImageComparisonService imageComparisonService,
			IPathService pathService)
		{
			Logger = logger;
			ImageComparisonService = imageComparisonService;
			PathService = pathService;
		}

		[HttpPost("{instanceId}")]
		public IActionResult Compare(string instanceId)
		{
			var groupA = PathService.GetGroupPath("groupA", instanceId);
			var groupB = PathService.GetGroupPath("groupB", instanceId);

			if (!Directory.Exists(groupA) || !Directory.Exists(groupB))
			{
				Logger.LogWarning("Um dos grupos de imagem nao foi encontrado.");
				return BadRequest("Um dos grupos de imagem nao foi encontrado.");
			}

			var results = ImageComparisonService.CompareImages(groupA, groupB);
			return Ok(results);
		}
	}
}