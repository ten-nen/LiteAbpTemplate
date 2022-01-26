using LiteAbp.Shared.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LiteAbp.API.Controllers.Forestage
{
    [Route("api/forestage/[controller]")]
    [Authorize(ApiPermissions.Forestage)]
    public class ForestageControllerBase : ControllerBase
    {
    }
}
