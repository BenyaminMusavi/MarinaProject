using Marina.UI.Models;
using Marina.UI.Providers.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using X.PagedList;

namespace Marina.UI.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IImportRepository _importRepository;
    public HomeController(ILogger<HomeController> logger, IImportRepository importRepository)
    {
        _logger = logger;
        _importRepository = importRepository;
    }

    [Authorize]
    public async Task<IActionResult> Index(int? page, int pageSize = 25)
    {
        var model = await _importRepository.GetAll();
        List<DataRow> rows = model.AsEnumerable().ToList();

        var pageNumber = page ?? 1;

        var result = rows.ToPagedList(pageNumber, pageSize);
        return View(result);
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