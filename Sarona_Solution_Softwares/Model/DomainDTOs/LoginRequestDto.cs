using System;
using System.ComponentModel.DataAnnotations;

namespace Sarona_Solution_Softwares.Model.DomainDTOs
{
	public class LoginRequestDto
	{
        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage ="Email address is invalid, PLease help verify your email address")]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password, ErrorMessage ="Please use strong password")]
        public string Password { get; set; }
    }
}

