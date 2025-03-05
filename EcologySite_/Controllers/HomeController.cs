using System.Diagnostics;
using Ecology.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using EcologySite.Models;
using EcologySite.Models.Home;
using EcologySite.Services;
using EcologySite.Services.Apis;

namespace EcologySite.Controllers;

public class HomeController : Controller
{
    //private readonly ILogger<HomeController> _logger;
    private AuthService _authService;
    private IUserRepositryReal _userRepositryReal;
    private HttpNumberApi _httpNumberApi;
    private HttpWoofApi _httpWoofApi;

    public HomeController(AuthService authService, 
        IUserRepositryReal userRepositryReal,
        HttpNumberApi httpNumberApi,
        HttpWoofApi httpWoofApi)
    {
        _authService = authService;
        _userRepositryReal = userRepositryReal;
        _httpNumberApi = httpNumberApi;
        _httpWoofApi = httpWoofApi;
    }

    public async Task<IActionResult> Index()
    {
        var viewModel = new IndexViewModel();

        var userName = _authService.GetName();
        var userId = _authService.GetUserId();
        
        viewModel.UserName = userName;
        viewModel.UserId = userId ?? -1;
        
        viewModel.TheNumber = DateTime.Now.Second;
            
        var taskforNumber = _httpNumberApi.GetFactAsync(viewModel.TheNumber);
        var taskforDog = _httpWoofApi.GetRandomDogImage();

        await Task.WhenAll(taskforNumber, taskforDog);

        viewModel.FactAboutNumber = taskforDog.Result;
        viewModel.DogImageSrc = taskforDog.Result;
        
        return View(viewModel);
    }

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