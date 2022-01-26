using LiteAbp.Application;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteAbp.API.Controllers.Backstage
{
    public class PermissionController : BackstageControllerBase
    {
        protected AppServices AppServices { get; }

        public PermissionController(AppServices appServices)
        {
            AppServices = appServices;
        }

        [HttpGet]
        public async Task<object> GetAllListAsync()
        {
            return await AppServices.PermissionService.GetAllListAsync();
        }
    }
}
