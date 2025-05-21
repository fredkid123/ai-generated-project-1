using System.IO;

namespace ImageOverlapApp.Services
{
	public class PathService
	{
		private readonly string _webRootPath;

		public PathService(IWebHostEnvironment env)
		{
			_webRootPath = env.WebRootPath ?? "wwwroot";
		}

		public string GetGroupPath(string instanceId, string group)
		{
			return Path.Combine(_webRootPath, instanceId, group);
		}
	}
}
