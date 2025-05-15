using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("compare")]
public class CompareController : ControllerBase
{
	private readonly IWebHostEnvironment _env;
	private readonly IImageComparisonService _comparisonService;

	public CompareController(IWebHostEnvironment env, IImageComparisonService comparisonService)
	{
		_env = env;
		_comparisonService = comparisonService;
	}

	[HttpPost]
	public IActionResult Compare()
	{
		var groupAPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "groupA");
		var groupBPath = Path.Combine(_env.WebRootPath ?? "wwwroot", "groupB");

		var results = _comparisonService.Compare(groupAPath, groupBPath);
		return Ok(results);
	}
}