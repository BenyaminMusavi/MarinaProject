using Marina.UI.Models.Entities;
using Marina.UI.Models.ViewModels;
using Marina.UI.Providers;
using Marina.UI.Providers.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace Marina.UI.Controllers;

public class AccountController : Controller
{
    private readonly IUserManager _userManager;
    private readonly IUserRepository _userRepository;
    private readonly IRegionRepository _regionRepository;
    private readonly IRSMRepository _rsmRepository;
    private readonly IDistributorRepository _distributorRepository;
    private readonly ILineRepository _lineRepository;
    private readonly IProvinceRepository _provinceRepository;
    public AccountController(IUserManager userManager, IUserRepository userRepository,
        IRegionRepository regionRepository, IRSMRepository rsmRepository,
        IDistributorRepository distributorRepository,
        ILineRepository lineRepository, IProvinceRepository provinceRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _regionRepository = regionRepository;
        _rsmRepository = rsmRepository;
        _distributorRepository = distributorRepository;
        _lineRepository = lineRepository;
        _provinceRepository = provinceRepository;
    }

    [Authorize(Roles = "admin")]
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

        if (user.UserId == 1)
        {
   
            await _userManager.SignIn(this.HttpContext, user, false);
            return LocalRedirect("~/Account/list");
        }
        await _userManager.SignIn(this.HttpContext, user, false);
        return LocalRedirect("~/");

    }

    public async Task<IActionResult> Register()
    {
        ViewBag.IsSuccess = false;

        var regions = await _regionRepository.GetAll();
        ViewBag.RegionList = new SelectList(regions, "Id", "Name");

        var rsms = await _rsmRepository.GetAll();
        ViewBag.RsmList = new SelectList(rsms, "Id", "Name");

        var distributors = await _distributorRepository.GetAll();
        ViewBag.DistributorList = new SelectList(distributors, "Id", "Name");

        var lines = await _lineRepository.GetAll();
        ViewBag.LineList = new SelectList(lines, "Id", "Name");

        var provinces = await _provinceRepository.GetAll();
        ViewBag.ProvinceList = new SelectList(provinces, "Id", "Name");

        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userRepository.Register(model);

        if (user)
        {
            ViewBag.IsSuccess = true;
            return LocalRedirect("~/Account/Login");
        }
        else
        {
            ViewBag.IsSuccess = false;
            var regions = await _regionRepository.GetAll();
            ViewBag.RegionList = new SelectList(regions, "Id", "Name");

            var rsms = await _rsmRepository.GetAll();
            ViewBag.RsmList = new SelectList(rsms, "Id", "Name");

            var distributors = await _distributorRepository.GetAll();
            ViewBag.DistributorList = new SelectList(distributors, "Id", "Name");

            var lines = await _lineRepository.GetAll();
            ViewBag.LineList = new SelectList(lines, "Id", "Name");

            var provinces = await _provinceRepository.GetAll();
            ViewBag.ProvinceList = new SelectList(provinces, "Id", "Name");
            return View();
        }
    }

    public async Task<IActionResult> LogoutAsync()
    {
        await _userManager.SignOut(this.HttpContext);
        return RedirectPermanent("~/Home/Index");
    }

    public async Task<ActionResult> Active(int id)
    {
        var IsSuccess = await _userRepository.SetStatus(id);

        if (IsSuccess)
            ViewBag.IsSuccess = true;
        else
            ViewBag.IsSuccess = false;
        return LocalRedirect("~/Account/list");
    }
    [HttpPost]
    public async Task<ActionResult> Delete(int id)
    {
        var IsSuccess = await _userRepository.Delete(id);

        if (IsSuccess)
            ViewBag.IsSuccess = true;
        else
            ViewBag.IsSuccess = false;
        return LocalRedirect("~/Account/list");
    }
}
