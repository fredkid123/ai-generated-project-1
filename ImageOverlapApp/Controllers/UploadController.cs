using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("upload")]
public class UploadController : ControllerBase
{
	private readonly IWebHostEnvironment _env;

	public UploadController(IWebHostEnvironment env)
	{
		_env = env;
	}

	[HttpPost("groupA")]
	[HttpPost("groupB")]
	public async Task<IActionResult> UploadImages([FromRoute] string group, [FromForm] IFormFileCollection files)
	{
		var targetFolder = Path.Combine(_env.WebRootPath ?? "wwwroot", group);
		Directory.CreateDirectory(targetFolder);

		foreach (var file in files)
		{
			var filePath = Path.Combine(targetFolder, file.FileName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				await file.CopyToAsync(stream);
			}
		}

		return Ok(new { uploaded = files.Count });
	}
}