using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.IO;

namespace ImageOverlapApp.Services
{
	public class UploadService : IUploadService
	{
		private IPathService PathService { get; set; }
		private ILogger<UploadService> Logger { get; set; }

		public UploadService(IPathService pathService, ILogger<UploadService> logger)
		{
			PathService = pathService;
			Logger = logger;
		}

		public void UploadFiles(string group, string instanceId, IFormFile[] files)
		{
			string uploadPath = PathService.GetGroupPath(group, instanceId);

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

			CleanupOldFolders(Path.GetDirectoryName(uploadPath));
		}

		private void CleanupOldFolders(string basePath)
		{
			var dirs = Directory.GetDirectories(basePath);
			foreach (var dir in dirs)
			{
				var creation = Directory.GetCreationTimeUtc(dir);
				if (creation < DateTime.UtcNow.AddHours(-2))
				{
					try
					{
						Directory.Delete(dir, true);
						Logger.LogInformation("Diretório antigo removido: {dir}", dir);
					}
					catch (Exception ex)
					{
						Logger.LogWarning(ex, "Erro ao remover diretório antigo: {dir}", dir);
					}
				}
			}
		}
	}
}