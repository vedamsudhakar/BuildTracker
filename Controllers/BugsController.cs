using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BuildTracker.Data;
using BuildTracker.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BuildTracker.Controllers
{
    [Authorize]
    public class BugsController : Controller
    {
        private readonly BuildTrackerContext _context;

        public BugsController(BuildTrackerContext context)
        {
            _context = context;
        }

        // GET: Bugs
        public async Task<IActionResult> Index(int? applicationId, int? buildId, BugStatus? status, BugSeverity? severity)
        {
            var bugs = _context.Bugs
                .Include(b => b.Application)
                .Include(b => b.Build)
                .Include(b => b.AssignedToUser)
                .Where(b => !b.IsDeleted)
                .AsQueryable();

            if (applicationId.HasValue)
            {
                bugs = bugs.Where(b => b.ApplicationId == applicationId);
            }

            if (buildId.HasValue)
            {
                bugs = bugs.Where(b => b.BuildId == buildId);
            }

            if (status.HasValue)
            {
                bugs = bugs.Where(b => b.Status == status);
            }

            if (severity.HasValue)
            {
                bugs = bugs.Where(b => b.Severity == severity);
            }

            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name", applicationId);
            // Build dropdown should ideally be filtered by application, but for filter it's okay to show all or handle via JS
            ViewData["BuildId"] = new SelectList(_context.Builds, "Id", "Version", buildId); 
            ViewData["Status"] = new SelectList(Enum.GetValues(typeof(BugStatus)), status);
            ViewData["Severity"] = new SelectList(Enum.GetValues(typeof(BugSeverity)), severity);

            return View(await bugs.OrderByDescending(b => b.CreatedDate).ToListAsync());
        }

        // GET: Bugs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _context.Bugs
                .Include(b => b.Application)
                .Include(b => b.Build)
                .Include(b => b.AssignedToUser)
                .Include(b => b.CreatedByUser)
                .Include(b => b.Attachments)
                .Include(b => b.Comments).ThenInclude(c => c.AuthorUser)
                .Include(b => b.History).ThenInclude(h => h.ChangedByUser)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (bug == null)
            {
                return NotFound();
            }

            return View(bug);
        }

        // GET: Bugs/Create
        public IActionResult Create(int? applicationId, int? buildId)
        {
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name", applicationId);
            
            if (applicationId.HasValue)
            {
                 ViewData["BuildId"] = new SelectList(_context.Builds.Where(b => b.ApplicationId == applicationId), "Id", "Version", buildId);
            }
            else
            {
                ViewData["BuildId"] = new SelectList(Enumerable.Empty<SelectListItem>());
            }

            ViewData["AssignedToUserId"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Bugs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Description,ApplicationId,BuildId,Type,Severity,Status,EnvironmentDetails,StepsToReproduce,ExpectedResult,ActualResult,AssignedToUserId")] Bug bug)
        {
            if (ModelState.IsValid)
            {
                bug.CreatedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                bug.CreatedDate = DateTime.Now;
                _context.Add(bug);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name", bug.ApplicationId);
            ViewData["AssignedToUserId"] = new SelectList(_context.Users, "Id", "UserName", bug.AssignedToUserId);
            return View(bug);
        }

        // GET: Bugs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _context.Bugs.FindAsync(id);
            if (bug == null)
            {
                return NotFound();
            }
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name", bug.ApplicationId);
            // Filter builds by application
            var builds = _context.Builds.Where(b => b.ApplicationId == bug.ApplicationId);
            ViewData["BuildId"] = new SelectList(builds, "Id", "Version", bug.BuildId);
            ViewData["AssignedToUserId"] = new SelectList(_context.Users, "Id", "UserName", bug.AssignedToUserId);
            return View(bug);
        }

        // POST: Bugs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,ApplicationId,BuildId,Type,Severity,Status,EnvironmentDetails,StepsToReproduce,ExpectedResult,ActualResult,AssignedToUserId")] Bug bug)
        {
            if (id != bug.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var originalBug = await _context.Bugs.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
                    
                    // History Logging
                    var historyEntries = new List<BugHistory>();
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    if (originalBug.Status != bug.Status)
                    {
                        historyEntries.Add(new BugHistory { BugId = bug.Id, ChangedByUserId = userId, ChangedDate = DateTime.Now, Description = $"Status changed from {originalBug.Status} to {bug.Status}" });
                    }
                    if (originalBug.Severity != bug.Severity)
                    {
                        historyEntries.Add(new BugHistory { BugId = bug.Id, ChangedByUserId = userId, ChangedDate = DateTime.Now, Description = $"Severity changed from {originalBug.Severity} to {bug.Severity}" });
                    }
                    if (originalBug.AssignedToUserId != bug.AssignedToUserId)
                    {
                        var newAssignee = bug.AssignedToUserId != null ? _context.Users.Find(bug.AssignedToUserId)?.UserName : "Unassigned";
                        var oldAssignee = originalBug.AssignedToUserId != null ? _context.Users.Find(originalBug.AssignedToUserId)?.UserName : "Unassigned";
                        historyEntries.Add(new BugHistory { BugId = bug.Id, ChangedByUserId = userId, ChangedDate = DateTime.Now, Description = $"Assigned to changed from {oldAssignee} to {newAssignee}" });
                    }

                    if (historyEntries.Any())
                    {
                        _context.BugHistories.AddRange(historyEntries);
                    }

                    bug.UpdatedByUserId = userId;
                    bug.UpdatedDate = DateTime.Now;
                    bug.CreatedByUserId = originalBug.CreatedByUserId; // Preserve creator
                    bug.CreatedDate = originalBug.CreatedDate;

                    _context.Update(bug);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BugExists(bug.Id))
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
            ViewData["ApplicationId"] = new SelectList(_context.Applications, "Id", "Name", bug.ApplicationId);
            ViewData["AssignedToUserId"] = new SelectList(_context.Users, "Id", "UserName", bug.AssignedToUserId);
            return View(bug);
        }

        // GET: Bugs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var bug = await _context.Bugs
                .Include(b => b.Application)
                .Include(b => b.Build)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bug == null)
            {
                return NotFound();
            }

            return View(bug);
        }

        // POST: Bugs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var bug = await _context.Bugs.FindAsync(id);
            if (bug != null)
            {
                // Soft delete
                bug.IsDeleted = true;
                _context.Update(bug);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool BugExists(int id)
        {
            return _context.Bugs.Any(e => e.Id == id);
        }
        
        // POST: Bugs/UploadAttachment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadAttachment(int bugId, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return RedirectToAction(nameof(Details), new { id = bugId });
            }

            var bug = await _context.Bugs.FindAsync(bugId);
            if (bug == null)
            {
                return NotFound();
            }

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "bugs", bugId.ToString());
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new BugAttachment
            {
                BugId = bugId,
                FileName = file.FileName,
                FilePath = "/uploads/bugs/" + bugId.ToString() + "/" + uniqueFileName,
                UploadedDate = DateTime.Now,
                UploadedByUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.BugAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = bugId });
        }

        // POST: Bugs/AddComment
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddComment(int bugId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return RedirectToAction(nameof(Details), new { id = bugId });
            }

            var bug = await _context.Bugs.FindAsync(bugId);
            if (bug == null)
            {
                return NotFound();
            }

            var comment = new BugComment
            {
                BugId = bugId,
                Content = content,
                CreatedDate = DateTime.Now,
                AuthorUserId = User.FindFirstValue(ClaimTypes.NameIdentifier)
            };

            _context.BugComments.Add(comment);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = bugId });
        }

        // API for cascading dropdown
        [HttpGet]
        public JsonResult GetBuildsByApplication(int applicationId)
        {
            var builds = _context.Builds
                .Where(b => b.ApplicationId == applicationId)
                .OrderByDescending(b => b.Date)
                .Select(b => new { id = b.Id, version = b.Version })
                .ToList();
            return Json(builds);
        }
    }
}
