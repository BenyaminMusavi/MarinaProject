using Marina.UI.Models.ViewModels;
using Marina.UI.Providers;
using Marina.UI.Providers.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Marina.UI.Controllers;

public class AccountController : Controller
{
    private readonly IUserManager _userManager;
    private readonly IUserRepository _userRepository;

    public AccountController(IUserManager userManager, IUserRepository userRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
    }

    [Authorize]
    public async Task<IActionResult> List()
    {
        var res = await _userRepository.GetAll();
        return View(res);
    }

    public IActionResult Login()
    {
        return View();
    }

    [Authorize]
    public IActionResult Profile()
    {
        return View(this.User.Claims.ToDictionary(x => x.Type, x => x.Value));
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _userRepository.Validate(model);

        if (user == null) return View(model);

        await _userManager.SignIn(this.HttpContext, user, false);

        return LocalRedirect("~/Account/list");
    }

    public IActionResult Register()
    {
        ViewBag.IsSuccess = false;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = _userRepository.Register(model);

        if (user)
        {
            ViewBag.IsSuccess = true;
            return View();
        }
        else
        {
            ViewBag.IsSuccess = false;
            return View();
        }

        //await _userManager.SignIn(this.HttpContext, user, false);

        //return LocalRedirect("~/Home/Index");
    }

    public async Task<IActionResult> LogoutAsync()
    {
        await _userManager.SignOut(this.HttpContext);
        return RedirectPermanent("~/Home/Index");
    }
}
