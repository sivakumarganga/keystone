using Asp.Versioning;
using FluentResults;
using FluentValidation;
using KeyStone.Identity.Contracts;
using KeyStone.Shared.API.RequestModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KeyStone.API.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("api/v{version:apiVersion}/Connect")]
    public class ConnectController : BaseController
    {
        private readonly ILogger<ConnectController> _logger;
        private readonly IAccountContract _accountService;
        private readonly IValidator<SignUpRequest> _signUpRequestValidator;
        private readonly IValidator<LoginRequest> _loginRequestValidator;

        public ConnectController(ILogger<ConnectController> logger, IAccountContract accountService, IValidator<SignUpRequest> signUpRequestValidator, IValidator<LoginRequest> loginRequestValidator)
        {
            _logger = logger;
            _accountService = accountService;
            _signUpRequestValidator = signUpRequestValidator;
            _loginRequestValidator = loginRequestValidator;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(SignUpRequest request)
        {
            var validationResult = _signUpRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ResultResponse(validationResult.Errors);
            }
            
            var serviceResult = await _accountService.SignUpAsync(request);
            return ResultResponse(serviceResult);
        }

        [HttpPost("Token")]
        public async Task<IActionResult> Token(LoginRequest request)
        {
            var validationResult = _loginRequestValidator.Validate(request);
            if (!validationResult.IsValid)
            {
                return ResultResponse(validationResult.Errors);
            }
            
            var serviceResult = await _accountService.GetTokenAsync(request);
            return ResultResponse(serviceResult);
        }

        [Authorize]
        [HttpGet("UserInfo")]
        public async Task<IActionResult> UserInfo()
        {
            var serviceResult = await _accountService.GetUserInfoAsync();
            return ResultResponse(serviceResult);
        }

        [HttpPost("Logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var serviceResult = await _accountService.RequestLogoutAsync();
            return ResultResponse(serviceResult);
        }

        [HttpPost("RefreshSignIn")]
        public async Task<IActionResult> RefreshUserToken(Guid refreshToken)
        {
            var checkCurrentAccessTokenValidity =
                await HttpContext.AuthenticateAsync(JwtBearerDefaults.AuthenticationScheme);

            if (checkCurrentAccessTokenValidity.Succeeded)
                return BadRequest("Current access token is valid. No need to refresh");

            var newTokenResult = await _accountService.RefreshUserTokenAsync(refreshToken);

            return ResultResponse(newTokenResult);
        }
    }
}