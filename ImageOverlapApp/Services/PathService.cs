using Microsoft.AspNetCore.Hosting;

namespace ImageOverlapApp.Services
{
	public class PathService : IPathService
	{
		private IWebHostEnvironment Env { get; set; }

		public PathService(IWebHostEnvironment env)
		{
			Env = env;
		}

		public string GetGroupPath(string group, string instanceId)
		{
			return Path.Combine(Env.WebRootPath ?? "wwwroot", instanceId, group);
		}
	}
}