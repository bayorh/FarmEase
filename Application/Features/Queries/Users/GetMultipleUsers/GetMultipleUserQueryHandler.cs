using AutoMapper;
using Domain.Contracts.Repositories;
using Domain.Dtos;
using Domain.Entities;
using Domain.Shared;
using MediatR;

namespace Application.Features.Queries.Users.GetMultipleUsers;

public class GetMultipleUserQueryHandler : IRequestHandler<GetMultipleQuery, GetMultipleQueryResponse>
{
    private readonly IAsyncRepository<User> _userRepository;
    private readonly IMapper _mapper;
   
    public GetMultipleUserQueryHandler(IAsyncRepository<User> userRepository,IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<GetMultipleQueryResponse> Handle(GetMultipleQuery request, CancellationToken cancellationToken)
    {
      
        var response = new GetMultipleQueryResponse();
        try
        {
            
            var _user = await  _userRepository.GetAllAsync(request.RequestParameters);
            var result = _mapper.Map<PagedList<UserDto>>(_user);
            response.Users = result; 
            response.Success = true;
            response.Message = "fetching users.";     
        }
        catch (Exception ex)
        {
            response.Success = false;
            response.Message = "error while fetching users:" + ex.Message;
        }
        return response;
    }
}
