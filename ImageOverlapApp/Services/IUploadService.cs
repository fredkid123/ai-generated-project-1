using Microsoft.AspNetCore.Http;

namespace ImageOverlapApp.Services
{
	public interface IUploadService
	{
		void UploadFiles(string group, string instanceId, IFormFile[] files);
	}
}