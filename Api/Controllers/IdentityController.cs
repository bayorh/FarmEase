using Application.Features.Commands.Users.LoginUser;
using Application.Features.Commands.Users.RegisterUser;
using Application.Features.Commands.Users.ResetPassword;
using Application.Features.Commands.Users.ResetPasswordlink;
using Application.Features.Queries.Users.GetMultipleUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Api.Controllers;

[ApiVersion("1.0")]
[Route("api/Users")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Authorize]
public class AuthController: ControllerBase
{
    private readonly IMediator _mediator;
    
     
    public AuthController(IMediator medietor)
    {
        _mediator = medietor;
    }
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]

    public async Task<IActionResult> Login([FromBody]LoginUserCommand request)
    {
        var response = await _mediator.Send(request);
        if (response.Success) return Ok(response);
        return BadRequest(response);
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]

    public async Task<IActionResult> Register([FromBody]RegisterUserCommand request)
    {
        var response = await _mediator.Send(request);
        if (response.Success) return Ok(response);
        return BadRequest(response);
    }
    [AllowAnonymous]
    [HttpPost("SendResetLinkToMail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<SendResetPasswordlinkToMailResponse>> GetResetPasswordLink([FromBody] SendResetPasswordlinkToMailRequest query)
    {
        var response = await _mediator.Send(query);
        if (response.Success) return Ok(response);
        return BadRequest(response);

    }
    [AllowAnonymous]
    [HttpPost("ResetPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<ResetPasswordCommandResponse>> ResetPassword([FromBody] ResetPasswordCommand query)
    {
        var response = await _mediator.Send(query);
        if (response.Success) return Ok(response);
        return BadRequest(response);

    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [Authorize(Roles ="Admin")]
    public async Task<ActionResult<GetMultipleQueryResponse>> GetMultipleUser([FromQuery] GetMultipleQuery query)
    {
        var response = await _mediator.Send(query);
        Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response.Users.MetaData));
        if (response.Success) return Ok(response);
        return BadRequest(response);
    }
}
