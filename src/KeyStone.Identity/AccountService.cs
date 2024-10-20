using AutoMapper;
using FluentResults;
using KeyStone.Concerns.Domain;
using KeyStone.Core.Context;
using KeyStone.Data.Models.Identity;
using KeyStone.Identity.Contracts;
using KeyStone.Shared.API.RequestModels;
using KeyStone.Shared.Models;
using KeyStone.Shared.Models.Identity;

namespace KeyStone.Identity
{
    public class AccountService : IAccountContract
    {
        private readonly IAppUserManager _userManager;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;
        private readonly IRequestContext _requestContext;

        public AccountService(IAppUserManager userManager, IMapper mapper, IJwtService jwtService,
            IRequestContext requestContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _jwtService = jwtService;
            _requestContext = requestContext;
        }

        public async Task<Result<bool>> SignUpAsync(SignUpRequest request)
        {
            bool phoneExists = await _userManager.IsExistUser(request.PhoneNumber);
            if (phoneExists)
            {
                return Result.Fail<bool>("User with this phone number already exists");
            }

            bool userNameExists = await _userManager.IsExistUserName(request.UserName);
            if (userNameExists)
            {
                return Result.Fail<bool>("Username already taken");
            }

            var user = _mapper.Map<User>(request);
            var createResult = await _userManager.CreateUserWithPasswordAsync(user, request.Password);
            if (!createResult.Succeeded)
            {
                return Result.Fail<bool>(string.Join(",",
                    createResult.Errors.Select(c => c.Description)));
            }

            return true;
        }

        public async Task<Result<AccessTokenResponse>> GetTokenAsync(LoginRequest request)
        {
            var user = await _userManager.GetByUserName(request.Username);
            if (user is null)
            {
                return Result.Fail<AccessTokenResponse>("Invalid username or password");
            }

            var isLockedOut = await _userManager.IsUserLockedOutAsync(user);
            if (isLockedOut)
            {
                if (user.LockoutEnd is not null)
                {
                    return Result.Fail<AccessTokenResponse>(
                        $"User account is locked out. Try in {(user.LockoutEnd - DateTimeOffset.Now).Value.Minutes} Minutes");
                }
            }

            var passwordCheck = await _userManager.AdminLogin(user, request.Password);
            if (!passwordCheck.Succeeded)
            {
                return Result.Fail<AccessTokenResponse>("Invalid username or password");
            }

            var accessToken = await _jwtService.GenerateAsync(user);
            return Result.Ok(accessToken);
        }

        public async Task<Result<UserInfo>> GetUserInfoAsync()
        {
            var user = await _userManager.GetByUserName(_requestContext.UserName);
            if (user is null)
            {
                return Result.Fail<UserInfo>("Invalid username or password");
            }

            return Result.Ok(new UserInfo
            {
                UserId = user.Id,
                UserKey = user.UserCode,
                Email = user.Email!,
                DisplayName = $"{user.Name} {user.FamilyName}",
                Roles = user.UserRoles.Select(item => item.Role.Name).ToList()!
            });
        }

        public async Task<Result<bool>> RequestLogoutAsync()
        {
            var user = await _userManager.GetByUserName(_requestContext.UserName);
            if (user is null)
            {
                return Result.Fail<bool>("Invalid user");
            }

            await _userManager.UpdateSecurityStampAsync(user);
            return true;
        }

        public async Task<Result<AccessTokenResponse>> RefreshUserTokenAsync(Guid refreshToken)
        {
            var newToken = await _jwtService.RefreshToken(refreshToken);
            return Result.Ok(newToken);
        }
    }
}