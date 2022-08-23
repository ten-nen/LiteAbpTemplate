using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using LiteAbp.Application.Dtos;
using Volo.Abp.Authorization.Permissions;

namespace LiteAbp.Application
{
    public class LiteAbpApplicationModuleAutoMapperProfile : Profile
    {
        public LiteAbpApplicationModuleAutoMapperProfile()
        {
            CreateMap<IdentityUser, UserDto>()
                .MapExtraProperties();

            CreateMap<UserCreateDto,IdentityUser>()
                .MapExtraProperties();

            CreateMap<UserUpdateDto,IdentityUser>()
                .MapExtraProperties();

            CreateMap<IdentityRole, RoleDto>()
                .MapExtraProperties();

            CreateMap<PermissionGrant, PermissionInfoDto>();
        }
    }
}
