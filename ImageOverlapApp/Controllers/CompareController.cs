using Microsoft.AspNetCore.Mvc;
using ImageOverlapApp.Services;

namespace ImageOverlapApp.Controllers
{
	[ApiController]
	public class CompareController : ControllerBase
	{
		private IImageComparisonService ComparisonService { get; set; }

		public CompareController(IImageComparisonService comparisonService)
		{
			ComparisonService = comparisonService;
		}

		[HttpPost("compare")]
		public IActionResult Compare()
		{
			var result = ComparisonService.CompareGroups("groupA", "groupB");

			if (result == null)
			{
				return StatusCode(500, "Erro ao realizar comparação.");
			}

			return Ok(result);
		}
	}
}