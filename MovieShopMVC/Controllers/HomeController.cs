using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieShopMVC.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Services;
using ApplicationCore.ServiceInterfaces;

namespace MovieShopMVC.Controllers
{
    public class HomeController : Controller
    {
        private IMovieService _movieService;
        //here call the movie service
        // public HomeController(ILogger<HomeController> logger)
        //this parameter pass in any instance that implementing this interface

        public HomeController(IMovieService movieService)
        {
            //_logger = logger;
            _movieService = movieService;
            //supply actual implementation to this controller with inject
        }

        public async Task<IActionResult> Index()
        {
            var movieCards = await _movieService.GetTopRevenueMovies();
            return View(movieCards);
        }



        // Clean Architecture

        // Interfaces

        // IMovieService

        // class MovieService : SOmeClass, GenreService
        // {
        // }


        // class MovieService2 : IMovieSerivce
        // {
        // }

        // Depedency Injection
        // Application core layer
        // Entities => C# classes that repewsents your domian/database objjects =>
        // Models => 

        // 15 tables
        // 20 columns , Movie Table
        // Entity => Movie 20 properties

        // Models => UI
        // Movie list => MovieCardModel => id, title, posterimage
        // DTO (Data Transfer Objetcs) => API

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
