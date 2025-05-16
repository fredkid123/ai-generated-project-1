using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ImageOverlapApp.Services;
using System;
using System.IO;

namespace ImageOverlapApp.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class CompareController : ControllerBase
	{
		private IImageComparisonService ComparisonService { get; set; }
		private ILogger<CompareController> Logger { get; set; }

		public CompareController(IImageComparisonService comparisonService, ILogger<CompareController> logger)
		{
			ComparisonService = comparisonService;
			Logger = logger;
		}

		[HttpPost]
		public IActionResult Compare([FromQuery] string instanceId)
		{
			if (string.IsNullOrWhiteSpace(instanceId))
			{
				return BadRequest("InstanceId obrigatório.");
			}

			var root = Path.Combine("wwwroot", instanceId);
			var groupADir = Path.Combine(root, "groupA");
			var groupBDir = Path.Combine(root, "groupB");

			if (!Directory.Exists(groupADir) || !Directory.Exists(groupBDir))
			{
				return NotFound("Um ou ambos os diretórios de imagens não foram encontrados.");
			}

			try
			{
				var result = ComparisonService.CompareImages(groupADir, groupBDir);
				return Ok(result);
			}
			catch (Exception ex)
			{
				Logger.LogError(ex, "Erro ao comparar imagens");
				return StatusCode(500, "Erro interno durante a comparação");
			}
		}
	}
}