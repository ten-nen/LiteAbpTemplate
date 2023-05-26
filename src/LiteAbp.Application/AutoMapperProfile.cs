using AutoMapper;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using LiteAbp.Application.Dtos;

namespace LiteAbp.Application
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
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
