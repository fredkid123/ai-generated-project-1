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

		[HttpPost("upload/{group}/{instanceId}")]
		public IActionResult Upload(string group, string instanceId, [FromForm] IFormFile[] files)
		{
			if (files == null || files.Length == 0)
			{
				return BadRequest("Nenhum arquivo enviado.");
			}

			UploadService.UploadFiles(group, instanceId, files);
			return Ok();
		}
	}
}