using ApplicationCore.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MovieShopMVC.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IUserService _userService;

        public UserController(ICurrentUserService currentUserService, IUserService userService)
        {
            _currentUserService = currentUserService;
            _userService = userService;
        }

        public async Task<IActionResult> GetAllPurchases()
        {
            var userId = _currentUserService.UserId;
            var movieCards = await _userService.GetPurchaseMovies(userId);
            return View(movieCards);
        }
        public async Task<IActionResult> GetFavorites()
        {
            var userId = _currentUserService.UserId;
            var movieCards = await _userService.GetFavoriteMovies(userId);
            return View(movieCards);
        }
    }
}