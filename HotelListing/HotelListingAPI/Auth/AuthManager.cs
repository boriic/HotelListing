using AutoMapper;
using HotelListingAPI.Auth.Common;
using HotelListingAPI.Auth.Models;
using HotelListingAPI.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListingAPI.Auth
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private User _user;

        private const string _loginProvider = "HotelListingApi";
        private const string _refreshToken = "RefreshToken";

        public AuthManager(IMapper mapper, UserManager<User> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Method creates refresh token for a user
        /// </summary>
        /// <returns>Newly created refresh token</returns>
        public async Task<string> CreateRefreshToken()
        {
            await _userManager.RemoveAuthenticationTokenAsync(_user, "HotelListingApi", "RefreshToken");

            var newRefreshToken = await _userManager.GenerateUserTokenAsync(_user, _loginProvider, _refreshToken);

            await _userManager.SetAuthenticationTokenAsync(_user, _loginProvider, _refreshToken, newRefreshToken);

            return newRefreshToken;

        }

        /// <summary>
        /// Method verifies the refresh token if it is valid and creates new token
        /// </summary>
        /// <param name="request">AuthResponseDto object</param>
        /// <returns>AuthResponse object</returns>
        public async Task<AuthResponseDto> VerifyRefreshToken(AuthResponseDto request)
        {

            /* You can do it this way also to read from the token find the email claim and try to find the user by email,
             * but since we get user id from the request we can directly try to find user by id
             * 
                var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

                var tokenContent = jwtSecurityTokenHandler.ReadJwtToken(request.Token);

                var username = tokenContent.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;

                _user = await _userManager.FindByNameAsync(username);
             */

            _user = await _userManager.FindByIdAsync(request.UserId);

            if (_user == null || _user.Id != request.UserId)
                return null;

            var isValidRefreshToken = await _userManager.VerifyUserTokenAsync(_user, _loginProvider, _refreshToken, request.RefreshToken);

            if (isValidRefreshToken)
            {
                var response = await GenerateToken();
                return response;
            }

            await _userManager.UpdateSecurityStampAsync(_user);

            return null;
        }

        /// <summary>
        /// Method verifies if the password matches the user and returns auth response with token and refresh token
        /// </summary>
        /// <param name="loginDto">LoginDto object</param>
        /// <returns>AuthResponse object</returns>
        public async Task<AuthResponseDto> Login(LoginDto loginDto)
        {
            _user = await _userManager.FindByEmailAsync(loginDto.Email);

            bool isValidUser = await _userManager.CheckPasswordAsync(_user, loginDto.Password);

            if (_user == null || isValidUser == false)
            {
                return null;
            }

            var response = await GenerateToken();

            return response;
        }

        /// <summary>
        /// Method registers new user and adds user role
        /// </summary>
        /// <param name="userDto">UserDto object</param>
        /// <returns>Error if any</returns>
        public async Task<IEnumerable<IdentityError>> Register(UserDto userDto)
        {
            _user = _mapper.Map<User>(userDto);

            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "User");
            }

            return result.Errors;
        }

        /// <summary>
        /// Method generates new token for a user if the login is successful
        /// </summary>
        /// <returns>AuthResponse object</returns>
        private async Task<AuthResponseDto> GenerateToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(_user);

            var roleClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                new Claim("uid", _user.Id)
            }.Union(roleClaims);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
                );

            var response = new AuthResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                UserId = _user.Id,
                RefreshToken = await CreateRefreshToken()
            };
            return response;
        }
    }
}
