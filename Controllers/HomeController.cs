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
        var buildData = _context.Builds
            .Include(b => b.Application)
            .GroupBy(b => b.Application.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .ToList();

        var bugData = _context.Bugs
            .Include(b => b.Application)
            .Where(b => !b.IsDeleted && b.Status == BugStatus.Open)
            .GroupBy(b => b.Application.Name)
            .Select(g => new { Name = g.Key, Count = g.Count() })
            .ToList();

        // Merge data
        var appNames = buildData.Select(d => d.Name).Union(bugData.Select(d => d.Name)).Distinct();

        var analytics = appNames.Select(name => new BuildAnalyticsViewModel
        {
            ApplicationName = name,
            Count = buildData.FirstOrDefault(d => d.Name == name)?.Count ?? 0,
            BugCount = bugData.FirstOrDefault(d => d.Name == name)?.Count ?? 0
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
