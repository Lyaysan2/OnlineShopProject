using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Dto.UserDTO;
using OnlineShopProject.Models;
using OnlineShopProject.Service;

namespace OnlineShopProject.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;
        public UserController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, 
            IMapper mapper, ILogger<UserController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var appUser = _mapper.Map<AppUser>(signUpDto);

            var createdUser = await _userManager.CreateAsync(appUser, signUpDto.Password);
            if (!createdUser.Succeeded) return BadRequest(createdUser.Errors);
                
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);

            var userDto = _mapper.Map<UserDto>(appUser);

            _logger.LogInformation($"User with email - {userDto.Email}, username - {userDto.UserName}, id - {userDto.Id} created");
            return Ok(userDto);
        }

        [HttpPost("sign-in")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto singInDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == singInDto.UserName);

            if (user == null) return Unauthorized("Invalid username!");

            var result = await _signInManager.CheckPasswordSignInAsync(user, singInDto.Password, false);

            if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

            var (token, expires) = await _tokenService.CreateToken(user);

            var userTokenDto = _mapper.Map<UserTokenDto>((user, token, expires));

            _logger.LogInformation($"User with username - {singInDto.UserName} logged in");
            return Ok(userTokenDto);
        }
    }
}

