using System;
using AutoMapper;
using Sarona_Solution_Softwares.Model.Domain;
using Sarona_Solution_Softwares.Model.DomainDTOs;

namespace Sarona_Solution_Softwares
{
	public class AutoMapperProfiles : Profile
	{
		public AutoMapperProfiles()
		{
			CreateMap<TestRequest, TestRequestDto>().ReverseMap();

        }

    }
}

