using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;

namespace MyField.Controllers
{
    public class ReportsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;

        public ReportsController(Ksans_SportsDbContext context,
            FileUploadService fileUploadService,
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
            _activityLogger = activityLogger;
        }
        public async Task<IActionResult> MatchReports ()
        {
            var matchReports = await _context.MatchReports
                .Include( m => m.Season)
                .ToListAsync();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(matchReports);
        }

        public async Task<IActionResult> MatchResultsReports()
        {
            var matchResultsReports = await _context.MatchResultsReports
                 .Include(m => m.Season)
                 .ToListAsync();

            var currentSeason = await _context.League
                  .Where(c => c.IsCurrent)
                  .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(matchResultsReports);
        }

        public async Task<IActionResult> TransfersReports()
        {
            var transfersReports = await _context.TransfersReports
                 .Include(m => m.Season)
                 .Include(m => m.TransferPeriod)
                 .ToListAsync();

            var currentSeason = await _context.League
                  .Where(c => c.IsCurrent)
                  .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(transfersReports);
        }
    }
}
