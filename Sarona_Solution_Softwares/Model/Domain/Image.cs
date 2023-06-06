using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sarona_Solution_Softwares.Model.Domain
{
	public class Image
	{
		public Guid Id { set; get; }
		[NotMapped]
		public IFormFile File { get; set; }
		public string FileName { get; set; }
		public string? FileDescription { get; set; }
		public string FileExtension { get; set; }
		public long FileSizeInBytes { get; set; }
		public string FilePath { get; set; }
	}
}

