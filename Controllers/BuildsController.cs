using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildTracker.Data;
using BuildTracker.Models;
using Microsoft.AspNetCore.Authorization;

namespace BuildTracker.Controllers
{
    [Authorize]
    public class BuildsController : Controller
    {
        private readonly BuildTrackerContext _context;

        public BuildsController(BuildTrackerContext context)
        {
            _context = context;
        }

        // GET: Builds
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            IQueryable<BuildInfo> builds = _context.Builds.Include(b => b.Application);

            if (!String.IsNullOrEmpty(searchString))
            {
                builds = builds.Where(s => s.BuildPath.Contains(searchString)
                                       || (s.ReleaseNotes != null && s.ReleaseNotes.Contains(searchString))
                                       || s.Version.Contains(searchString));
            }

            return View(await builds.ToListAsync());
        }

        // GET: Builds/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildInfo = await _context.Builds
                .Include(b => b.Application)
                .Include(b => b.Bugs)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildInfo == null)
            {
                return NotFound();
            }

            return View(buildInfo);
        }

        // GET: Builds/Create
        public IActionResult Create()
        {
            ViewData["FtpServerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.FtpServers.Where(f => f.IsActive), "Id", "Name");
            ViewData["ApplicationId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Applications, "Id", "Name");
            return View();
        }

        // POST: Builds/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ApplicationId,BuildPath,ReleaseNotes,Date,Version,FtpServerId")] BuildInfo buildInfo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(buildInfo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FtpServerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.FtpServers.Where(f => f.IsActive), "Id", "Name", buildInfo.FtpServerId);
            ViewData["ApplicationId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Applications, "Id", "Name", buildInfo.ApplicationId);
            return View(buildInfo);
        }

        // GET: Builds/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildInfo = await _context.Builds.FindAsync(id);
            if (buildInfo == null)
            {
                return NotFound();
            }
            ViewData["FtpServerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.FtpServers.Where(f => f.IsActive), "Id", "Name", buildInfo.FtpServerId);
            ViewData["ApplicationId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Applications, "Id", "Name", buildInfo.ApplicationId);
            return View(buildInfo);
        }

        // POST: Builds/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ApplicationId,BuildPath,ReleaseNotes,Date,Version,FtpServerId")] BuildInfo buildInfo)
        {
            if (id != buildInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(buildInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BuildInfoExists(buildInfo.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["FtpServerId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.FtpServers.Where(f => f.IsActive), "Id", "Name", buildInfo.FtpServerId);
            ViewData["ApplicationId"] = new Microsoft.AspNetCore.Mvc.Rendering.SelectList(_context.Applications, "Id", "Name", buildInfo.ApplicationId);
            return View(buildInfo);
        }

        // GET: Builds/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var buildInfo = await _context.Builds
                .Include(b => b.FtpServer)
                .Include(b => b.Application)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (buildInfo == null)
            {
                return NotFound();
            }

            return View(buildInfo);
        }

        // POST: Builds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var buildInfo = await _context.Builds.FindAsync(id);
            if (buildInfo != null)
            {
                _context.Builds.Remove(buildInfo);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    #pragma warning disable SYSLIB0014
        public async Task<IActionResult> Download(int id)
        {
            var build = await _context.Builds.Include(b => b.FtpServer).FirstOrDefaultAsync(b => b.Id == id);
            if (build == null || build.FtpServer == null)
            {
                return NotFound("Build or FTP Server not found.");
            }

            try
            {
                var request = (System.Net.FtpWebRequest)System.Net.WebRequest.Create(new Uri($"ftp://{build.FtpServer.Host}:{build.FtpServer.Port}/{build.BuildPath.TrimStart('/')}"));
                request.Method = System.Net.WebRequestMethods.Ftp.DownloadFile;
                request.Credentials = new System.Net.NetworkCredential(build.FtpServer.Username, build.FtpServer.Password);
                request.UseBinary = true;

                var response = (System.Net.FtpWebResponse)await request.GetResponseAsync();
                var stream = response.GetResponseStream();
                
                return File(stream, "application/octet-stream", System.IO.Path.GetFileName(build.BuildPath));
            }
            catch (Exception ex)
            {
                return BadRequest($"Error downloading file: {ex.Message}");
            }
        }
        #pragma warning restore SYSLIB0014

    private bool BuildInfoExists(int id)
    {
        return _context.Builds.Any(e => e.Id == id);
    }
}
}
