using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Features.Queries.Users.GetMultipleUsers;

public class GetMultipleQuery : IRequest<GetMultipleQueryResponse>
{
    public RequestParameters? RequestParameters { get; set; }
}
