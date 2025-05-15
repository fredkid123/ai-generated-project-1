using Microsoft.AspNetCore.Mvc;

namespace ImageOverlapApp.Controllers
{
	[ApiController]
	public class UploadController : ControllerBase
	{
		private readonly IWebHostEnvironment _env;
		private readonly ILogger<UploadController> _logger;

		public UploadController(IWebHostEnvironment env, ILogger<UploadController> logger)
		{
			_env = env;
			_logger = logger;
		}

		[HttpPost("upload/{group}")]
		public IActionResult Upload(string group, [FromForm] IFormFile[] files)
		{
			if (files == null || files.Length == 0)
				return BadRequest("Nenhum arquivo enviado.");

			var uploadPath = Path.Combine(_env.WebRootPath ?? "wwwroot", group);
			if (!Directory.Exists(uploadPath))
				Directory.CreateDirectory(uploadPath);

			foreach (var file in files)
			{
				var filePath = Path.Combine(uploadPath, file.FileName);
				using var stream = new FileStream(filePath, FileMode.Create);
				file.CopyTo(stream);
				_logger.LogInformation("Arquivo salvo: {filePath}", filePath);
			}

			return Ok();
		}
	}
}