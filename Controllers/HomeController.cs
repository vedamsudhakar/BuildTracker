using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BuildTracker.Models;
using Microsoft.AspNetCore.Authorization;

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
            .GroupBy(b => b.BuildType)
            .Select(g => new { Type = g.Key, Count = g.Count() })
            .ToList();

        var analytics = data.Select(d => new BuildAnalyticsViewModel
        {
            BuildType = GetDisplayName(d.Type),
            Count = d.Count
        }).ToList();

        return View(analytics);
    }

    private string GetDisplayName(Enum value)
    {
        var field = value.GetType().GetField(value.ToString());
        var attribute = field?.GetCustomAttributes(typeof(System.ComponentModel.DataAnnotations.DisplayAttribute), false)
                             .FirstOrDefault() as System.ComponentModel.DataAnnotations.DisplayAttribute;
        return attribute?.Name ?? value.ToString();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [AllowAnonymous]
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
