using LiteAbp.Application.Dtos.Identity;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using LiteAbp.Application.Dtos.Permission;
using Volo.Abp.Authorization.Permissions;

namespace LiteAbp.Application
{
    public class LiteAbpApplicationModuleAutoMapperProfile : Profile
    {
        public LiteAbpApplicationModuleAutoMapperProfile()
        {
            CreateMap<IdentityUser, IdentityUserDto>()
                .MapExtraProperties();

            CreateMap<IdentityUserCreateDto,IdentityUser>()
                .MapExtraProperties();

            CreateMap<IdentityUserUpdateDto,IdentityUser>()
                .MapExtraProperties();

            CreateMap<IdentityRole, IdentityRoleDto>()
                .MapExtraProperties();

            CreateMap<PermissionGrant, PermissionGrantInfoDto>();
        }
    }
}
