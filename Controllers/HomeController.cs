using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BuildTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace BuildTracker.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly BuildTracker.Data.BuildTrackerContext _context;

    public HomeController(ILogger<HomeController> logger, BuildTracker.Data.BuildTrackerContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        var data = _context.Builds
            .Include(b => b.Application)
            .GroupBy(b => b.Application.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .ToList();

        var analytics = data.Select(d => new BuildAnalyticsViewModel
        {
            ApplicationName = d.Name,
            Count = d.Count
        }).ToList();

        return View(analytics);
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
