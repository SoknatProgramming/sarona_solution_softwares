﻿using System;
namespace Sarona_Solution_Softwares.Model.DomainDTOs
{
	public class LoginResponseDto
	{
        public string JwtToken { get; set; }
        public List<TestRequestDto> TestData { get; set; }
    }
}

