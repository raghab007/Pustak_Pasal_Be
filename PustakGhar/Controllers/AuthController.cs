using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AlishPustakGhar.Dtos;
using AlishPustakGhar.Model;
using AlishPustakGhar.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AlishPustakGhar.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    
    private JwtHelper _jwtHelper;

    public AuthController(UserManager<User> userManager, RoleManager<IdentityRole<Guid>> roleManager, JwtHelper jwtHelper)
    {
        _userManager= userManager;
        _roleManager = roleManager;
        _jwtHelper = jwtHelper;
    }


    [Authorize]
    [HttpGet("me")]
    public   IActionResult Get()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var user =  _userManager.Users.FirstOrDefault(x => x.Id.ToString() == userId);
        return Ok(user);
    }

    [HttpPost("signup")]
    public async Task<ActionResult<User>> SignUp([FromBody] UserRegisterDto userRegisterDto )
    {
        
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        
        var oldUser = await _userManager.FindByEmailAsync(userRegisterDto.Email);
        if (oldUser != null)
        {
            return BadRequest("User already exists");
        }

        var user = new User
        {
            UserName = userRegisterDto.UserName,
            Email = userRegisterDto.Email,
            FirstName = userRegisterDto.FirstName,
            LastName = userRegisterDto.LastName,
            Age = userRegisterDto.Age,
            Address = userRegisterDto.Address,
        };
    
        var result = await _userManager.CreateAsync(user, userRegisterDto.Password);
    
        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        // Check if role exists before assigning
        if (await _roleManager.RoleExistsAsync("User"))
        {
            await _userManager.AddToRoleAsync(user, "User");
        }
        else
        {
            // Optionally create the role if it doesn't exist
            await _roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            await _userManager.AddToRoleAsync(user, "User");
        }

        return Ok();
    }



    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var user = await _userManager.FindByEmailAsync(userLoginDto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userLoginDto.Password))
        {
            return Unauthorized();
        }

        var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(ClaimTypes.Email, user.Email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
        };

        var token = _jwtHelper.GeToken(authClaims);


        String returnToken = new JwtSecurityTokenHandler().WriteToken(token);
        return   Ok(new
        {
            token = new JwtSecurityTokenHandler().WriteToken(token),
            expiration = token.ValidTo
        });
    ;
    }


}