using System;
using Microsoft.AspNetCore.Identity;

namespace Sarona_Solution_Softwares.Repositories
{
	public interface ITokenRepository
	{
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}

