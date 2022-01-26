using LiteAbp.Application.Dtos.Identity;
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
using UserLoginInfo = LiteAbp.API.Controllers.Models.UserLoginInfo;

namespace LiteAbp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : AbpControllerBase
    {
        protected AppServices AppServices { get; }
        protected IConfiguration Configuration { get; }
        public AuthController(AppServices appServices, IConfiguration configuration)
        {
            AppServices = appServices;
            Configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<object> LoginAsync([FromBody] UserLoginInfo user)
        {
            (IdentityUserDto user, string token) r = await AppServices.UserService.LoginAsync(user.UserName, user.Password, user.RememberMe);
            return new { r.user, r.token };
        }

        [HttpDelete]
        public async Task LogoutAsync()
        {
            await AppServices.UserService.LogoutAsync(CurrentUser.UserName);
        }
    }
}
