using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantAPI.Controllers
{
	[Route("file") ] //ten kontroler nie będzie służył do zwracania zasobów z bazy danych
	[Authorize]
	public class FileController:ControllerBase
	{
		public ActionResult GetFile([FromQuery] string fileName)
		{
			var rootPath = Directory.GetCurrentDirectory();

			var filePath = $"{rootPath}/PrivateFiles/{fileName}";

			var fileExist = System.IO.File.Exists(filePath);

			if (!fileExist)
			{ 
				return NotFound(); 
			}
			var contentProvider= new FileExtensionContentTypeProvider();
			contentProvider.TryGetContentType(filePath,out string contentType);
			var fileContents=System.IO.File.ReadAllBytes(filePath);

			return File(fileContents, contentType, fileName);

		}
	}
}
