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
        public async Task<IActionResult> MatchReports()
        {
            var matchReport = await _context.MatchReports
                .Where(m => m.Season.IsCurrent)
                .Include(m => m.Season)
                .FirstOrDefaultAsync();

            var overallMatchCount = await GetOverallMatchCountAsync();

            if (overallMatchCount > 0)
            {
                matchReport.MatchesToBePlayedCount = (overallMatchCount * overallMatchCount) - overallMatchCount;
            }
            else
            {
                matchReport.MatchesToBePlayedCount = 0;
            }

            matchReport.UnreleasedFixturesCount = matchReport.MatchesToBePlayedCount - matchReport.FixturedMatchesCount;

            decimal fixturedMatchesRate = 0;
            decimal unfixturedMatchesRate = 0;
            decimal playedMatchesRate = 0;
            decimal postponedMatchesRate = 0;
            decimal interruptedMatchesRate = 0;

            if (matchReport.MatchesToBePlayedCount > 0)
            {
                fixturedMatchesRate = ((decimal)matchReport.FixturedMatchesCount / matchReport.MatchesToBePlayedCount) * 100;
                unfixturedMatchesRate = ((decimal)(matchReport.MatchesToBePlayedCount - matchReport.FixturedMatchesCount) / matchReport.MatchesToBePlayedCount) * 100;
                playedMatchesRate = ((decimal)matchReport.PlayedMatchesCounts / matchReport.MatchesToBePlayedCount) * 100;
                postponedMatchesRate = ((decimal)matchReport.PostponedMatchesCount / matchReport.MatchesToBePlayedCount) * 100;
                interruptedMatchesRate = ((decimal)matchReport.InterruptedMatchesCount / matchReport.MatchesToBePlayedCount) * 100;

                decimal totalPlayRates = playedMatchesRate + postponedMatchesRate + interruptedMatchesRate;

                if (totalPlayRates != 0)
                {
                    decimal adjustmentFactor = 100 / totalPlayRates;
                    playedMatchesRate *= adjustmentFactor;
                    postponedMatchesRate *= adjustmentFactor;
                    interruptedMatchesRate *= adjustmentFactor;
                }
            }

            matchReport.FixturedMatchesRate = fixturedMatchesRate;
            matchReport.UnfixturedMatchesRate = unfixturedMatchesRate;
            matchReport.PlayedMatchesRate = playedMatchesRate;
            matchReport.PostponedMatchesRate = postponedMatchesRate;
            matchReport.InterruptedMatchesRate = interruptedMatchesRate;

            var matchReports = await _context.MatchReports
                .Where(m => m.Season.IsCurrent)
                .Include(m => m.Season)
                .ToListAsync();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();

            ViewBag.CurrentSeason = currentSeason?.LeagueYears; 

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
            decimal winningRate = 0;
            decimal losingRate = 0;
            decimal drawingRate = 0;

            if (matchResultsReport.ExpectedResultsCount > 0)
            {
                releasedResultsRate = ((decimal)matchResultsReport.ReleasedResultsCount / matchResultsReport.ExpectedResultsCount) * 100;
                unreleasedResultsRate = ((decimal)matchResultsReport.UnreleasedResultsCount / matchResultsReport.ExpectedResultsCount) * 100;

                if (matchResultsReport.ReleasedResultsCount > 0)
                {
                    winningRate = ((decimal)matchResultsReport.WinsCount / matchResultsReport.ReleasedResultsCount) * 100;
                    losingRate = ((decimal)matchResultsReport.LosesCount / matchResultsReport.ReleasedResultsCount) * 100;
                    drawingRate = ((decimal)matchResultsReport.DrawsCount / matchResultsReport.ReleasedResultsCount) * 100;

                    decimal totalRate = winningRate + losingRate + drawingRate;
                    if (totalRate != 100)
                    {
                        decimal adjustmentFactor = 100 / totalRate;
                        winningRate *= adjustmentFactor;
                        losingRate *= adjustmentFactor;
                        drawingRate *= adjustmentFactor;
                    }
                }
            }

            matchResultsReport.ReleasedResultsRate = releasedResultsRate;
            matchResultsReport.UnreleasedMatchesRate = unreleasedResultsRate;
            matchResultsReport.WinningRate = winningRate;
            matchResultsReport.LosingRate = losingRate;
            matchResultsReport.DrawingRate = drawingRate;




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
            return await _context.Club
                .Where(c => c.League.IsCurrent)
                .CountAsync();
        }

        public async Task<int> GetOverallMatchCountAsync()
        {
            return await _context.Club
                  .Where(c => c.League.IsCurrent)
                  .CountAsync();
        }

        public async Task<IActionResult> TransfersReports()
        {
            var transferReport = await _context.TransfersReports
                 .Where(m => m.Season.IsCurrent)
                 .Include(m => m.Season)
                 .Include(m => m.TransferPeriod)
                 .FirstOrDefaultAsync();

            decimal purchasedPercentage = ((decimal)transferReport.PurchasedPlayersCount / transferReport.TransferMarketCount) * 100;
            decimal declinedPercentage = ((decimal)transferReport.DeclinedTransfersCount / transferReport.TransferMarketCount) * 100;
            decimal notStartedPercentage = ((decimal)(transferReport.TransferMarketCount - transferReport.PurchasedPlayersCount - transferReport.DeclinedTransfersCount) / transferReport.TransferMarketCount) * 100;

            decimal successfulTransferRate = purchasedPercentage;
            decimal unsuccessfulTransferRate = declinedPercentage;
            decimal notStartedTransferRate = notStartedPercentage;

            transferReport.SuccessfulTranferRate = successfulTransferRate;
            transferReport.UnsuccessfulTranferRate = unsuccessfulTransferRate;
            transferReport.NotStartedTransferRate = notStartedTransferRate;

            var transferReports = await _context.TransfersReports
                 .Where(m => m.Season.IsCurrent)
                 .Include(m => m.Season)
                 .ToListAsync();

            var currentSeason = await _context.League
                  .Where(c => c.IsCurrent)
                  .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(transferReports);
        }
    }
}
