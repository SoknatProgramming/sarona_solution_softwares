using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sarona_Solution_Softwares;
using Sarona_Solution_Softwares.Model.Domain;
using Sarona_Solution_Softwares.Model.DomainDTOs;
using Sarona_Solution_Softwares.Model.DomainDTOs.CustomActionFilters;
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
        private readonly ITestRequest testRequest;
        private readonly IMapper mapper;

        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository, ITestRequest testRequest, IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.testRequest = testRequest;
            this.mapper = mapper;
        }


        //POST : /api/auth/register
        [HttpPost]
        [Route("Register")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            // Perform email validation
            if (!IsValidEmail(registerRequestDto.Username))
            {
                ModelState.AddModelError("errMgs", "Invalid email format, Example: userexample@gmail.com");
                return BadRequest(ModelState);
            }

            // Perform password validation
            else if (!IsValidPassword(registerRequestDto.Password))
            {
                ModelState.AddModelError("errMgs", "Password must have a minimum length of 8 characters with uppercase lowercase letter,digit and Symbol");
                return BadRequest(ModelState);
            }
            else
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
                            return Ok(identityResult);
                        }
                    }
                }
            }
            
            return Ok();
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPassword(string password)
        {
            // Password must have a minimum length of 8 characters
            if (password.Length < 8)
            {
                return false;
            }

            // Password must contain at least one uppercase letter, one lowercase letter, one digit and one Symbol
            if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit) || !password.Any(char.IsSymbol))
            {
                return false;
            }

            // Additional password complexity rules can be added here

            return true;
        }

        [HttpPost]
        [Route("Login")]
        [ValidateModel]
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
                        var testData = testRequest.GetAllAsync();
                        response.TestData = mapper.Map<List<TestRequestDto>>(testData);

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

