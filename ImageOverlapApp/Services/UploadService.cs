using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ImageOverlapApp.Services
{
	public class UploadService : IUploadService
	{
		private IWebHostEnvironment Env { get; set; }
		private ILogger<UploadService> Logger { get; set; }

		public UploadService(IWebHostEnvironment env, ILogger<UploadService> logger)
		{
			Env = env;
			Logger = logger;
		}

		public void UploadFiles(string group, IFormFile[] files)
		{
			string uploadPath = Path.Combine(Env.WebRootPath ?? "wwwroot", group);

			if (!Directory.Exists(uploadPath))
			{
				Directory.CreateDirectory(uploadPath);
			}

			foreach (var file in files)
			{
				string filePath = Path.Combine(uploadPath, file.FileName);
				using var stream = new FileStream(filePath, FileMode.Create);
				file.CopyTo(stream);
				Logger.LogInformation("Arquivo salvo: {filePath}", filePath);
			}
		}
	}
}