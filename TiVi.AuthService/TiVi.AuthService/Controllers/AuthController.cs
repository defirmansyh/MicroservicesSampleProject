using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TiVi.AuthService.Models;
using TiVi.AuthService.Models.Base;
using TiVi.AuthService.Models.Request;
using TiVi.AuthService.Models.Response;
using TiVi.AuthService.Models.ViewModel;
using TiVi.AuthService.Services.Interfaces;
using TiVi.AuthService.Utilities;

namespace TiVi.AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserAccountService _userAccountService;
        private readonly TokenManagement _tokenManagement;
        public AuthController(IOptions<TokenManagement> tokenManagement, IUserAccountService userAccountService)
        {
            _tokenManagement = tokenManagement.Value;
            _userAccountService = userAccountService;
        }

        [HttpPost]
        [Route("GetAuthentication")]
        public BaseJsonResponse<AuthenticationResponse> GetAuthentication([FromBody]AuthenticationRequest authenticationRequest)
        {
            string token = "";
            var result = new BaseJsonResponse<AuthenticationResponse>();
            var isValidAccount = _userAccountService.IsUserValid(authenticationRequest.Username, authenticationRequest.Password);
            if (isValidAccount)
            {
                var user = _userAccountService.GetUserAccount(authenticationRequest.Username);
                IsAuthenticated(user, out token);

                result.is_success = true;
                result.data = new AuthenticationResponse { Token = token };
            }
            else
            {
                result.is_success=false;
                result.errors.Add(new BaseJsonResponseError("Username or Password wrong", "Username or Password wrong"));
            }
            return result;
        }

        [HttpPost]
        [Route("Register")]
        public BaseJsonResponse<RegisterResponse> Register([FromBody]RegisterRequest registerRequest)
        {
            var result = new BaseJsonResponse<RegisterResponse>();

            _userAccountService.Register(registerRequest);

            result.is_success = true;
            result.data = new RegisterResponse {
                Username = registerRequest.Username,
                Password = registerRequest.Password,
                Email = registerRequest.Email
            };
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("ChangePassword")]
        public BaseJsonResponse<string> ChangePassword([FromBody]ChangePasswordRequest changePasswordRequest)
        {

            var result = new BaseJsonResponse<string>();

            //validate
            var userAuthenticated = _userAccountService.IsUserValid(ClaimIdentity.CurrentUserName, changePasswordRequest.password);
            if (!userAuthenticated)
            {
                result.is_success = false;
                result.data = "Password is wrong";
                return result;
            }

            if (changePasswordRequest.new_password != changePasswordRequest.retype_password)
            {
                result.is_success = false;
                result.data = "New password is not match";
                return result;
            }
            if (changePasswordRequest.password == changePasswordRequest.new_password)
            {
                result.is_success = false;
                result.data = "New password and old pass must different";
                return result;
            }

            _userAccountService.ChangePassword(changePasswordRequest);

            result.is_success = true;
            result.data = "Success";
            return result;
        }

        [Authorize]
        [HttpPost]
        [Route("DeactivateAccount")]
        public BaseJsonResponse<string> DeactivateAccount([FromBody]DeactivateAccountRequest accountRequest)
        {
            
            _userAccountService.DeleteUser(accountRequest);
            return new BaseJsonResponse<string>()
            {
                is_success = true,
                data = "Success"
            };
        }

        private bool IsAuthenticated(UserAccountVModel user, out string token)
        {

            token = string.Empty;

            var claim = new[]
            {
                //new Claim(ClaimTypes.Name, request.UserName),
                new Claim("CurrentUserID", user.UserAccountId.ToString()),
                new Claim("CurrentUserName", user.Username),
                new Claim("CurrentEmail" , user.Email??""),
                new Claim("IsAdmin" , user.IsAdmin.ToString()??""),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                _tokenManagement.Issuer,
                _tokenManagement.Audience,
                claim,
                expires: DataHelper.GetLocalTimes().AddHours(_tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );
            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;

        }
    }
}
