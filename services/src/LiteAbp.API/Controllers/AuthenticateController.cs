using LiteAbp.Application.Dtos;
using LiteAbp.Application.Interfaces;
using LiteAbp.Extensions.Abp.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Identity;
using Volo.Abp.Identity.AspNetCore;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace LiteAbp.Api.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : AbpControllerBase
    {
        protected IUserService UserService { get; }
        protected IConfiguration Configuration { get; }
        public AuthController(IUserService  identityUserService, IConfiguration configuration)
        {
            UserService = identityUserService;
            Configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<UserDto> LoginAsync([FromBody] UserLoginDto user)
        {
            return await UserService.LoginAsync(user.UserName, user.Password);
        }
    }
}
