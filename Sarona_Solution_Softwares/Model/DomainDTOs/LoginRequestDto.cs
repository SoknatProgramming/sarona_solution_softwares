using System;
using System.ComponentModel.DataAnnotations;

namespace Sarona_Solution_Softwares.Model.DomainDTOs
{
	public class LoginRequestDto
	{
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}

