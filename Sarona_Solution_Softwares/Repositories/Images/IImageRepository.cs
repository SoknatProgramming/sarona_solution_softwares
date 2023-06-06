using System;
using Sarona_Solution_Softwares.Model.Domain;

namespace Sarona_Solution_Softwares.Repositories.Images
{
	public interface IImageRepository
	{
		Task<Image> Upload(Image image);
	}
}

