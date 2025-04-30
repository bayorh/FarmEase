using Domain.Dtos;
using Domain.Shared;

namespace Application.Features.Queries.Users.GetMultipleUsers;

public record GetMultipleQueryResponse : BaseResponse
{
    public PagedList<UserDto> Users { get; set; }
}

