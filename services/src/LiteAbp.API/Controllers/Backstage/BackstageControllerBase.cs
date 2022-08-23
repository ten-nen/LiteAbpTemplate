using LiteAbp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.AspNetCore.Mvc;

namespace LiteAbp.Api.Controllers.Backstage
{
    [Route("api/backstage/[controller]")]
    [Authorize(ApiPermissions.Backstage)]
    public class BackstageControllerBase : AbpControllerBase
    {
    }
}
