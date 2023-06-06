using System;
using System.ComponentModel.DataAnnotations;

namespace Sarona_Solution_Softwares.Model.DomainDTOs
{
	public class ImageUploadRequestDto
	{
		[Required]
		public IFormFile File { get; set; }
		[Required]
		public string FileName { get; set; }
		public string? FileDescription { get; set; }
	}
}

