using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using TodoApp.Configuration;
using TodoApp.Models;
using TodoApp.Models.DTOs;

namespace TodoApp.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly TodoAppContext _context;


        public AuthController(IOptionsMonitor<JwtConfig> optionsMonitor, TodoAppContext context, UserManager<ApplicationUser> manager)
        {
            _jwtConfig = optionsMonitor.CurrentValue;
            _context = context;
            _userManager = manager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> RegisterSync([FromBody] RegistrationDTO user)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.Users.FirstOrDefault(dbUser => dbUser.Email == user.Email);

                if (existingUser != null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>()
                        {
                            "Email already exists"
                        },
                        Success = false
                    }
                    );
                }

                var newUser = new ApplicationUser()
                {
                    Email = user.Email,
                    UserName = user.Email
                };
                var createdUser = await _userManager.CreateAsync(newUser, user.Password);

                if (createdUser != null)
                {
                    await _context.SaveChangesAsync();

                    return Ok(new AuthResult()
                    {
                        Success = true,

                    });
                }
                else
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = { "Internal Server Errors" },
                        Success = false
                    });
                }

            }

            return BadRequest(new AuthResult()
            {
                Errors = new List<string>()
                {
                    "Invalid Email or Password"
                },
                Success = false
            });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> LoginSync([FromBody] LoginDTO user)
        {
            {
                var existingUser = await _userManager.FindByEmailAsync(user.Email);

                if (existingUser == null)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Invalid Email"
                            },
                        Success = false
                    });
                }

                var isCorrect = await _userManager.CheckPasswordAsync(existingUser, user.Password);

                if (!isCorrect)
                {
                    return BadRequest(new AuthResult()
                    {
                        Errors = new List<string>() {
                                "Invalid Password"
                            },
                        Success = false
                    });
                }

                var jwtToken = GenerateJwtToken(existingUser);

                return Ok(new AuthResult()
                {
                    Success = true,
                    Token = jwtToken
                });
            }
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("Id", user.Id),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(6),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }


    }
}
