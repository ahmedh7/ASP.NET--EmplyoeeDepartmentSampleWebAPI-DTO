using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Identity;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using EmployeeDep.BL.Dtos.User;
using System.Security.Claims;
using System.Net;
using EmployeeDep.DAL.Data.Models;

namespace EmployeeDep.API_Day2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IConfiguration _configuration;
        UserManager<Employee> _userManager;
        public UsersController(IConfiguration configuration, UserManager<Employee> userManager) { 
            _configuration= configuration;
        }
        [HttpPost]
        [Route("staticlogin")]
        public ActionResult<TokenDto> StaticLogin(LoginDto credentials)
        {


            // Static credentials
            if (credentials.Username != "admin" || credentials.Password != "password")
            {
                return BadRequest();
            }

            // Static ClaimList
            var claimsList = new List<Claim>
            {
                 new Claim(ClaimTypes.NameIdentifier, "Any_radom_Id"),
                 new Claim(ClaimTypes.Role, "Manager"),
                 new Claim("Nationality","Egyptian"),
             };

            // Convert Secret Hashing Key To ByteArray and encrypting it as SymmetricSecurityKey
            string keyString = _configuration.GetValue<string>("SecretKey")!;
            byte[] keyInByteArray = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyInByteArray);

            //Combine Secret Key with Hashing Algorithm
            var signingCredentials = new SigningCredentials(key ,
                SecurityAlgorithms.HmacSha256Signature);

            // Preparing JWT
            var expiry = DateTime.UtcNow.AddMinutes(5);
            var jwt = new JwtSecurityToken(
                expires:expiry,
                claims:claimsList,
                signingCredentials:signingCredentials
                );

            // Hand token to user (C# object to string)
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(jwt);

            return new TokenDto
            {
                Token = tokenString,
            };


        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<TokenDto>> Login(LoginDto loginDto)
        {
            Employee? user = await _userManager.FindByNameAsync(loginDto.Username);
            if(user == null) { return BadRequest(); }


            // Here you can check is user is locked due to too many failed attempts
            // var isLocked = _userManager.IsLockedOutAsync(user); 
            // if (isLocked) { return BadRequest();}

            bool isAuthenticated = await _userManager.CheckPasswordAsync(user, loginDto.Password);
            if(!isAuthenticated) 
            { 
                // Here you can increase Failed Attempt count by using 
                // awiat _userManager.AccessFailedAsync(user);
                return BadRequest();
            }

            var claimsList = await _userManager.GetClaimsAsync(user);
            // Convert Secret Hashing Key To ByteArray and encrypting it as SymmetricSecurityKey
            string keyString = _configuration.GetValue<string>("SecretKey")!;
            byte[] keyInByteArray = Encoding.ASCII.GetBytes(keyString);
            var key = new SymmetricSecurityKey(keyInByteArray);

            //Combine Secret Key with Hashing Algorithm
            var signingCredentials = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256Signature);

            // Preparing JWT
            var expiry = DateTime.UtcNow.AddMinutes(5);
            var jwt = new JwtSecurityToken(
                expires: expiry,
                claims: claimsList,
                signingCredentials: signingCredentials
                );

            // Hand token to user (C# object to string)
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(jwt);

            return new TokenDto
            {
                Token = tokenString,
            };

        }



        #region register 
        [HttpPost]
        [Route("registeremployee")]
        // All methods within user manager are asynchronous for better performance
        public async Task<ActionResult> RegisterEmployee(RegisterDto registerDto)
        {
            var newEmployee = new Employee
            {
                UserName = registerDto.Username,
                DepartmentId = registerDto.DepartmentId,
                Email = registerDto.Email
            };

            // UserManager will try creating a user in the DB with its own ID
            var result = await _userManager.CreateAsync(newEmployee, registerDto.Password);   
            if(!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.Role, "Employee"),
                // User will have Id after creation
                new Claim (ClaimTypes.NameIdentifier, newEmployee.Id.ToString()),
            };
            await _userManager.AddClaimsAsync(newEmployee, claims);
            return NoContent();
        }


        [HttpPost]
        [Route("registermanager")]
        // All methods within user manager are asynchronous for better performance
        public async Task<ActionResult> RegisterManager(RegisterDto registerDto)
        {
            var newEmployee = new Employee
            {
                UserName = registerDto.Username,
                DepartmentId = registerDto.DepartmentId,
                Email = registerDto.Email
            };

            // UserManager will try creating a user in the DB with its own ID
            var result = await _userManager.CreateAsync(newEmployee, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            var claims = new List<Claim>
            {
                new Claim (ClaimTypes.Role, "Manager"),
                // User will have Id after creation
                new Claim (ClaimTypes.NameIdentifier, newEmployee.Id.ToString()),
            };
            await _userManager.AddClaimsAsync(newEmployee, claims);
            return NoContent();
        }
        #endregion

    }
}

//var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
//Employee? userFromDb = await _userManager.FindByIdAsync(userId);
// The above 2 lines are identical to the following line and it retrieves all user information
// from the db supposing you already added the NameIdentifier claim for the user 
//Employee? userFromDb = await _userManager.GetUserAsync(User);