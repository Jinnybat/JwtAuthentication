using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuthentication.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    // [Authorize]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }

    [HttpPost("token")]
    public IActionResult Token(string userId, string password)
    {
        if (userId == "deepak" && password == "deepak")
        {
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this@ismy@secret#keytoEncrypt"));

            var claims = new List<Claim>();
            claims.Add(new Claim("userId", userId));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, "Deepak Bisht"));
            claims.Add(new Claim(ClaimTypes.Role, "Admin"));

            var securityToken = new JwtSecurityToken
            (
                issuer: "me",
                audience: "you",
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature),
                claims: claims
            );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            return Ok(token);
        }

        return Unauthorized();
    }
}
