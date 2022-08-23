using LiteAbp.Application;
using LiteAbp.Application.Dtos;
using LiteAbp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteAbp.Api.Controllers.Backstage
{
    public class PermissionController : BackstageControllerBase
    {
        protected IPermissionService PermissionService { get; }

        public PermissionController(IPermissionService  permissionService)
        {
            PermissionService = permissionService;
        }

        [HttpGet]
        public List<PermissionDto> GetAll()
        {
            return PermissionService.GetAll();
        }
    }
}
