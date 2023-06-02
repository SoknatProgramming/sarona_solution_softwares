using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sarona_Solution_Softwares;
using Sarona_Solution_Softwares.Model.DomainDTOs;
using Sarona_Solution_Softwares.Repositories;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApiAsp.net7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly ITokenRepository tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
        }


        //POST : /api/auth/register
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            HandleStatus handleStatus = new HandleStatus();
            handleStatus.ErrCode = 1;
            try
            {
                var identityUser = new IdentityUser
                {
                    UserName = registerRequestDto.Username,
                    Email = registerRequestDto.Username

                };
                var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

                if (identityResult.Succeeded)
                {
                    // add roles to this user

                    if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                    {
                        identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                        if (identityResult.Succeeded)
                        {
                            handleStatus.ErrCode = 0;
                            handleStatus.ErrMsg = "User was registered! PLease Login";

                            return Ok(handleStatus);
                        }
                    }
                }
            }catch(Exception ex)
            {
                handleStatus.ErrCode = ex.HResult;
                handleStatus.ErrMsg = ex.Message;
            }
            
            return BadRequest(handleStatus);
        }

        [HttpPost]
        [Route("Login")]

        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto )
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (user != null)
            {
                var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
                var checkLoked = await userManager.IsLockedOutAsync(user);

                if (checkPasswordResult && checkLoked == false)
                {
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        // Create Token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };

                        Response.Cookies.Append("JwtToken", jwtToken, new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.None,
                            Expires = DateTimeOffset.UtcNow.AddHours(12)
                        });

                        // Reset AccessFailedCount upon successful login
                        await userManager.ResetAccessFailedCountAsync(user);

                        return Ok(response);
                    }
                }
                else
                {
                    // Increment AccessFailedCount and lock account if exceeded maximum attempts
                    await userManager.AccessFailedAsync(user);

                    if (await userManager.GetAccessFailedCountAsync(user) >= 3)
                    {
                   
                        await userManager.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.AddMinutes(30));
                        return BadRequest($"To many atempt!");

                    }
                    
                }
                //check isLockedOut user waiting minutes counted
                if (checkLoked)
                {
                    var lockoutEnd = await userManager.GetLockoutEndDateAsync(user);
                    var remainingTimeSpan = lockoutEnd - DateTimeOffset.UtcNow;
                    var remainingMinutes = FormatTime(remainingTimeSpan.ToString());
                    return BadRequest($"Account is locked. Please try again in {remainingMinutes} minutes.");
                }

                return BadRequest("Your password incorrect");

            }

            return BadRequest("This Email isn't Registered yet, PLease Register First");
        }
        //format time
        private static string FormatTime(string inputTime)
        {
            TimeSpan timeSpan = TimeSpan.Parse(inputTime);
            string formattedTime = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            return formattedTime;
        }
    }
}

