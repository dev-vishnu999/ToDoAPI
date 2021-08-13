using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Repositories;
using Services;
using Services.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TODOApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<ToDoUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;
        private readonly IToDoItemService _toDoitemService;
        public LoginController(UserManager<ToDoUser> userManager, IOptionsSnapshot<JwtConfig> jwtConfig,
            RoleManager<Role> roleManager, IMapper mapper, IToDoItemService toDoitemService)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig.Value;
            _roleManager = roleManager;
            _mapper = mapper;
            this._toDoitemService = toDoitemService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<dynamic> login(ToDoUserModel model)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Email == model.Email);
                if (user is null)
                {
                    return new
                    {
                        success = false,
                        message = "User not found.",
                    };
                }
                var userSigninResult = await _userManager.CheckPasswordAsync(user, model.Password);
                if (userSigninResult)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    string Token = GenerateJwt(user, roles);
                    bool IsSuperAdmin = (roles.Count > 0) ? await _userManager.IsInRoleAsync(user, "SuperAdmin") : false;
                    return new
                    {
                        success = true,
                        message = "Login Successfully",
                        roles = roles,
                        token = Token,
                        userDetail = user,
                        isSuperAdmin = IsSuperAdmin,
                    };
                }
                return new
                {
                    success = false,
                    message = "Email or password incorrect.",
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message,
                };
            }
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<dynamic> Register(ToDoUserModel model)
        {
            try
            {
                var user = _mapper.Map<ToDoUserModel, ToDoUser>(model);
                var userDetail = _userManager.Users.SingleOrDefault(u => u.Email == model.Email);
                if (userDetail is null)
                {
                    var userCreateResult = await _userManager.CreateAsync(user, model.Password);

                    if (userCreateResult.Succeeded)
                    {
                        var createdUser = _userManager.Users.SingleOrDefault(u => u.UserName == model.Email);
                        return new
                        {
                            success = true,
                            message = "User created successfully",
                            data = createdUser
                        };
                    }
                    else
                    {
                        return new
                        {
                            success = false,
                            message = userCreateResult.Errors.First().Description,
                        };
                    }
                }
                else
                {
                    return new
                    {
                        success = false,
                        message = "User Already Exist",
                    };
                }
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message,
                };
            }

        }

        [Authorize("SuperAdmin")]
        [HttpPost("assignRoleToUser")]
        public async Task<dynamic> AssignRoleToUser([FromBody] ToDoUserRoleModel model)
        {
            try
            {
                var user = _userManager.Users.SingleOrDefault(u => u.Id.ToString() == model.UserId);
                string[] roles = new string[3] { "SuperAdmin", "Admin", "User" };
                foreach (string role in roles)
                {
                    await _userManager.RemoveFromRoleAsync(user, role);
                };
                var result = await _userManager.AddToRoleAsync(user, model.RoleName);

                if (result.Succeeded)
                {
                    return new
                    {
                        success = true,
                        message = "Role assigned successfully",
                    };
                    //return Ok(new Response(true, "Role assigned successfully", null));
                }
                return new
                {
                    success = false,
                    message = result.Errors.First().Description,
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message,
                };
            }
        }

        [Authorize("Admin")]
        [HttpGet("getRoles")]
        public async Task<dynamic> GetRoles()
        {
            var roles = _roleManager.Roles;
            if (roles != null)
            {
                return new
                {
                    success = true,
                    data = roles,
                };
            }
            return new
            {
                success = false,
                data = "",
                message = "No role found"
            };
        }

        [Authorize("Admin")]
        [HttpGet("getUsers")]
        public async Task<dynamic> GetUsers()
        {
            var users = _userManager.Users;
            List<UserModel> userModel = new List<UserModel>();
            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userModel.Add(new UserModel(user.Id, user.Email, user.FirstName, user.LastName, roles.Count >= 1 ? roles[0] : ""));
            }
            if (users != null)
            {
                return new
                {
                    success = true,
                    data = userModel,
                };
            }
            return new
            {
                success = false,
                data = "",
                message = "No user found"
            };
        }

        private string GenerateJwt(ToDoUser user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var roleClaims = roles.Select(r => new Claim(ClaimTypes.Role, r));
            claims.AddRange(roleClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(Convert.ToDouble(_jwtConfig.ExpirationInDays));

            var token = new JwtSecurityToken(
                issuer: _jwtConfig.Issuer,
                audience: _jwtConfig.Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
