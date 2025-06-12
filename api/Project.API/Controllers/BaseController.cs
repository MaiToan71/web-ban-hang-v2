using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.Caching.Interfaces;
using Project.Services.Users.Interfaces;
using Project.ViewModel.Usermanagers.Users;
using Project.ViewModels;

using System.Security.Claims;

namespace Project.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly IAppUserService _IUserService;
        private readonly ICachingExtension _ICachingExtension;

        public BaseController(
            ICachingExtension cachingExtension,
            IAppUserService userService)
        {
            _ICachingExtension = cachingExtension;
            _IUserService = userService;
        }

        protected ClaimsPrincipal CurrentUser => HttpContext?.User;

        private AuthorizedUserViewModel _AuthorizedUserViewModel;

        protected AuthorizedUserViewModel CurrentUserViewModel
        {
            get
            {
                var claims = CurrentUser.Claims;
                var userIdClaim = (string)HttpContext.Items["UserIdAuthorized"];
                Guid userId = Guid.Parse(userIdClaim);
                string _cacheKey = $"authorizeUser-UserId-{userId}";
                var authorizedUserViewModel = new AuthorizedUserViewModel();
                authorizedUserViewModel.UserId = userId;

                UserViewModel _userAuthor;
                if (_ICachingExtension.TryGetCache(out _userAuthor, _cacheKey))
                {
                    _userAuthor = _userAuthor;
                }
                else
                {
                    var userEntity = _IUserService.GetUserById(userId);

                    _ICachingExtension.SetCache(_cacheKey, userEntity, 60 * 60 * 24); // 1 ngày
                    _userAuthor = userEntity;
                }

                authorizedUserViewModel.FullName = _userAuthor.FullName;
                authorizedUserViewModel.UserId = _userAuthor.Id;
                authorizedUserViewModel.UserName = _userAuthor.UserName;
                authorizedUserViewModel.Permissions = _userAuthor.Permissions;
                //   authorizedUserViewModel.SiteId = _userAuthor.SiteId;
                //    authorizedUserViewModel.SiteMapId = _userAuthor.SiteMapId;

                //  var userEntity = _IUserService.GetById(authorizedUserViewModel.Id);


                //    authorizedUserViewModel.SuperAdmin = userEntity.SuperAdmin;

                return authorizedUserViewModel;


            }

            set
            {
                _AuthorizedUserViewModel = value;
            }
        }

    }
}
