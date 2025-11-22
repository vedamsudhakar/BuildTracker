using Microsoft.EntityFrameworkCore;
using BuildTracker.Data;
using BuildTracker.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BuildTracker.Controllers
{
    [Authorize]
    public class FtpServersController : Controller
    {
        private readonly BuildTrackerContext _context;

        public FtpServersController(BuildTrackerContext context)
        {
            _context = context;
        }

        // GET: FtpServers
        public async Task<IActionResult> Index()
        {
            return View(await _context.FtpServers.ToListAsync());
        }

        // GET: FtpServers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ftpServer = await _context.FtpServers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ftpServer == null)
            {
                return NotFound();
            }

            return View(ftpServer);
        }

        // GET: FtpServers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FtpServers/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Host,Port,Username,Password,IsActive")] FtpServer ftpServer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ftpServer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ftpServer);
        }

        // GET: FtpServers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ftpServer = await _context.FtpServers.FindAsync(id);
            if (ftpServer == null)
            {
                return NotFound();
            }
            return View(ftpServer);
        }

        // POST: FtpServers/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Host,Port,Username,Password,IsActive")] FtpServer ftpServer)
        {
            if (id != ftpServer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ftpServer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FtpServerExists(ftpServer.Id))
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
            return View(ftpServer);
        }

        // GET: FtpServers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ftpServer = await _context.FtpServers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ftpServer == null)
            {
                return NotFound();
            }

            return View(ftpServer);
        }

        // POST: FtpServers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ftpServer = await _context.FtpServers.FindAsync(id);
            if (ftpServer != null)
            {
                _context.FtpServers.Remove(ftpServer);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FtpServerExists(int id)
        {
            return _context.FtpServers.Any(e => e.Id == id);
        }
    }
}
