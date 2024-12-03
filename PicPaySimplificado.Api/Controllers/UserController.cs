using Microsoft.AspNetCore.Mvc;
using PicPaySimplificado.Application.DTOs;
using PicPaySimplificado.Application.Interfaces;

namespace PicPaySimplificado.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDTO userDto)
    {
        var userId = await _userService.RegisterUser(userDto);
        return Ok(new { UserId = userId });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
    {
        var user = await _userService.Authenticate(loginDto.Email, loginDto.Password);
        return Ok(new { UserId = user.Id, FullName = user.Fullname });
    }

    [HttpGet("{userId}/wallet")]
    public async Task<IActionResult> GetWalletBalance(Guid userId)
    {
        var balance = await _userService.GetWalletBalance(userId);
        return Ok(new { WalletBalance = balance });
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(Guid payerId, Guid payeeId, decimal amount)
    {
        await _userService.Transfer(payerId, payeeId, amount);
        return Ok(new { Message = "Transfer successful!" });
    }
}
