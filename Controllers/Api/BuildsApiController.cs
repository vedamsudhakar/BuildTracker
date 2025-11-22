using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BuildTracker.Data;
using BuildTracker.Models;
using System.ComponentModel.DataAnnotations;

namespace BuildTracker.Controllers.Api
{
    [Route("api/builds")]
    [ApiController]
    public class BuildsApiController : ControllerBase
    {
        private readonly BuildTrackerContext _context;

        public BuildsApiController(BuildTrackerContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Create a new build record
        /// </summary>
        /// <param name="request">Build information</param>
        /// <returns>Created build record</returns>
        [HttpPost]
        public async Task<ActionResult<BuildInfo>> CreateBuild([FromBody] CreateBuildRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var build = new BuildInfo
            {
                BuildType = request.BuildType,
                BuildPath = request.BuildPath,
                ReleaseNotes = request.ReleaseNotes,
                Date = request.Date ?? DateTime.Now,
                Version = request.Version,
                FtpServerId = request.FtpServerId
            };

            _context.Builds.Add(build);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBuild), new { id = build.Id }, build);
        }

        /// <summary>
        /// Get a specific build by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<BuildInfo>> GetBuild(int id)
        {
            var build = await _context.Builds
                .Include(b => b.FtpServer)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (build == null)
            {
                return NotFound();
            }

            return build;
        }

        /// <summary>
        /// Get all builds
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BuildInfo>>> GetBuilds()
        {
            return await _context.Builds
                .Include(b => b.FtpServer)
                .OrderByDescending(b => b.Date)
                .ToListAsync();
        }
    }

    public class CreateBuildRequest
    {
        [Required]
        public BuildType BuildType { get; set; }

        [Required]
        public string BuildPath { get; set; } = string.Empty;

        public string? ReleaseNotes { get; set; }

        public DateTime? Date { get; set; }

        [Required]
        public string Version { get; set; } = string.Empty;

        public int? FtpServerId { get; set; }
    }
}
