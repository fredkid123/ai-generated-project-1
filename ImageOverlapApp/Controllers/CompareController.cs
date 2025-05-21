using Microsoft.AspNetCore.Mvc;
using ImageOverlapApp.Services;

namespace ImageOverlapApp.Controllers
{
	[ApiController]
	[Route("compare")]
	public class CompareController : ControllerBase
	{
		private IImageComparisonService ImageComparisonService { get; set; }
		private ILogger<CompareController> Logger { get; set; }

		public CompareController(IImageComparisonService imageComparisonService, ILogger<CompareController> logger)
		{
			ImageComparisonService = imageComparisonService;
			Logger = logger;
		}

		[HttpPost("{instanceId}")]
		public IActionResult Compare([FromRoute] string instanceId)
		{
			Logger.LogInformation("Iniciando comparacao para instanceId: {InstanceId}", instanceId);

			string groupA = Path.Combine("wwwroot", instanceId, "groupA");
			string groupB = Path.Combine("wwwroot", instanceId, "groupB");

			if (!Directory.Exists(groupA) || !Directory.Exists(groupB))
			{
				Logger.LogWarning("Um dos grupos de imagem nao foi encontrado.");
				return BadRequest("Um dos grupos de imagem nao foi encontrado.");
			}

			var result = ImageComparisonService.CompareImages(groupA, groupB);
			return Ok(result);
		}
	}
}
