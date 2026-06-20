
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Modules.Identities.Core.Features.Commands.Users.LoginUser;
using Modules.Identities.Core.Features.Commands.Users.Register;
using Modules.Identities.Core.Features.Commands.Users.ResetPassword;
using Shared.Core.Dtos;
using Shared.Dispatcher;

namespace Modules.Identities.Controllers;

[Route("api/auth")]
[ApiController]
[ApiExplorerSettings(GroupName = "v1")]
[Authorize]
public class AuthController(ISender sender): ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    [ProducesResponseType(typeof(Result<LoginUserResponseDto>),StatusCodes.Status200OK)]

    public async Task<IActionResult> Login([FromBody]LoginUserCommand request)
    {
        var response = await sender.Send(request);
        return Ok(response);
    }

    [AllowAnonymous]
    [HttpPost("register")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]

    public async Task<IActionResult> Register([FromBody]RegisterUserCommand request)
    {
        var response = await sender.Send(request);
        return Ok(response);
    }
    // [AllowAnonymous]
    // [HttpPost("SendResetLinkToMail")]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // public async Task<ActionResult<SendResetPasswordlinkToMailResponse>> GetResetPasswordLink([FromBody] SendResetPasswordlinkToMailRequest query)
    // {
    //     var response = await sender.Send(query);
    //     return Ok(response);

    // }
    [AllowAnonymous]
    [HttpPost("ResetPassword")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command)
    {
        var response = await sender.Send(command);
        return Ok(response);

    }

    // [HttpGet]
    // [ProducesResponseType(StatusCodes.Status200OK)]
    // [Authorize(Roles ="Admin")]
    // public async Task<ActionResult<GetMultipleQueryResponse>> GetMultipleUser([FromQuery] GetMultipleQuery query)
    // {
    //     var response = await sender.Send(query);
    //     Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(response.Users.MetaData));
    //     return Ok(response);
    // }
}
