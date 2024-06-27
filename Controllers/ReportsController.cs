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
                .Where(m => m.Season.IsCurrent)
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
            var matchResultsReport = await _context.MatchResultsReports
                .Where(m => m.Season.IsCurrent)
                .Include(m => m.Season)
                .FirstOrDefaultAsync();

            if (matchResultsReport == null)
            {
                // Handle scenario where no match results report is found
                return NotFound();
            }

            var overallMatchResultsCount = await GetOverallMatchResultsCountAsync();

            if (overallMatchResultsCount > 0)
            {
                matchResultsReport.ExpectedResultsCount = (overallMatchResultsCount * overallMatchResultsCount) - overallMatchResultsCount;
            }
            else
            {
                matchResultsReport.ExpectedResultsCount = 0;
            }

            matchResultsReport.UnreleasedResultsCount = matchResultsReport.ExpectedResultsCount - matchResultsReport.ReleasedResultsCount;

            decimal releasedResultsRate = 0;
            decimal unreleasedResultsRate = 0;

            if (matchResultsReport.ExpectedResultsCount > 0)
            {
                releasedResultsRate = ((decimal)matchResultsReport.ReleasedResultsCount / matchResultsReport.ExpectedResultsCount) * 100;
                unreleasedResultsRate = ((decimal)matchResultsReport.UnreleasedResultsCount / matchResultsReport.ExpectedResultsCount) * 100;
            }

            decimal resultsRate = releasedResultsRate / (releasedResultsRate + unreleasedResultsRate) * 100;

            matchResultsReport.ResultsRate = resultsRate;

            var matchResultsReports = await _context.MatchResultsReports
                .Where(m => m.Season.IsCurrent)
                .Include(m => m.Season)
                .ToListAsync();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();

            ViewBag.CurrentSeason = currentSeason?.LeagueYears;

            return View(matchResultsReports);
        }


        public async Task<int> GetOverallMatchResultsCountAsync()
        {
            return await _context.Club.CountAsync();
        }

        public async Task<IActionResult> TransfersReports()
        {
            var transfersReports = await _context.TransfersReports
                 .Where(m => m.Season.IsCurrent)
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
