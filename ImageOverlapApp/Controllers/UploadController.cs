using Microsoft.AspNetCore.Mvc;
using ImageOverlapApp.Services;

namespace ImageOverlapApp.Controllers
{
	[ApiController]
	public class UploadController : ControllerBase
	{
		private IUploadService UploadService { get; set; }

		public UploadController(IUploadService uploadService)
		{
			UploadService = uploadService;
		}

		[HttpPost("upload/{group}")]
		public IActionResult Upload(string group, [FromForm] IFormFile[] files)
		{
			if (files == null || files.Length == 0)
			{
				return BadRequest("Nenhum arquivo enviado.");
			}

			UploadService.UploadFiles(group, files);
			return Ok();
		}
	}
}