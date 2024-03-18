using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShopProject.Dto.UserDTO;
using OnlineShopProject.Mappers;
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
        public UserController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signInManager, IMapper mapper)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _mapper = mapper;
        }

        [HttpPost("sign-up")]
        public async Task<IActionResult> SignUp([FromBody] SignUpDto signUpDto)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //var appUser = signUpDto.ToUserEntity();
            var appUser = _mapper.Map<AppUser>(signUpDto);

            var createdUser = await _userManager.CreateAsync(appUser, signUpDto.Password);
            if (!createdUser.Succeeded) return BadRequest(createdUser.Errors);
                
            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
            if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);
            //appUser.ToUserCreatedDto()
            var userDto = _mapper.Map<UserDto>(appUser);
            return Ok(userDto);
        }

        //[HttpPost("sign-in")]
        //public async Task<IActionResult> SignIn(SignInDto singInDto)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);

        //    var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == singInDto.Username);

        //    if (user == null) return Unauthorized("Invalid username!");

        //    var result = await _signInManager.CheckPasswordSignInAsync(user, singInDto.Password, false);

        //    if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

        //    var (token, expires) = _tokenService.CreateToken(user);

        //    return Ok(user.ToUserTokenDto(token, expires));
        //}

        //[HttpPost("sign-out")]
        //public async new Task<IActionResult> SignOut()
        //{
        //    await _signInManager.SignOutAsync();

        //    return Ok("User logged out successfully");
        //}
    }
}

