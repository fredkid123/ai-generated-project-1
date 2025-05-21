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
			var results = ImageComparisonService.CompareImages(instanceId);
			if (results == null)
			{
				Logger.LogWarning("Um dos grupos de imagem nao foi encontrado.");
				return BadRequest("Um dos grupos de imagem nao foi encontrado.");
			}
			return Ok(results);
		}
	}
}