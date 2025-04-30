using Application.Features.Commands.Users.LoginUser;
using Application.Features.Commands.Users.RegisterUser;
using AutoMapper;
using Domain.Dtos;
using Domain.Entities;
using Domain.Shared;

namespace Application.Profiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LoginUserCommand, User>();
        CreateMap<User,UserDto>();
        CreateMap(typeof(PagedList<>), typeof(PagedList<>))
            .ConvertUsing(typeof(PagedListTypeConverter<,>));

    }
}
