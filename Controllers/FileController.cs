﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
		[HttpGet]
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

		[HttpPost]
		public ActionResult Upload([FromForm] IFormFile file)
		{
			if (file != null && file.Length > 0)
			{
				var rootPath = Directory.GetCurrentDirectory();
				var fileName = file.FileName;
				var fullPath = $"{rootPath}/PrivateFiles/{fileName}";

				using(var stream = new FileStream(fullPath,FileMode.Create))
				{
					file.CopyTo(stream);
				}

				return Ok();
			}

			return BadRequest();

		}

	}
}
