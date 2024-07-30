using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class ReportsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;
        private readonly RoleManager<IdentityRole> _roleManager;


        public ReportsController(Ksans_SportsDbContext context,
            FileUploadService fileUploadService,
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
            _activityLogger = activityLogger;
            _roleManager = roleManager;
        }


        [Authorize(Roles = ("Fans Administrator"))]
        public async Task<IActionResult> FansAccountsReports()
        {
            var fansAccountsReports = await _context.FansAccountsReports
                .ToListAsync();

            var overallFansAccountsCount = await GetOverallFansAccountsCountAsync();

            var activeFansAccountsCount = await GetActiveFansAccountsCountAsync();

            var inactiveFansAccountsCount = await GetInactiveFansAccountsCountAsync();

            var suspendedFansAccountsCount = await GetSuspendedFansAccountsCountAsync();

            foreach(var fansAccountReport in fansAccountsReports)
            {
                fansAccountReport.OverallFansAccountsCount = overallFansAccountsCount;

                fansAccountReport.ActiveFansAccountsCount = activeFansAccountsCount;

                fansAccountReport.InactiveFansAccountsCount = inactiveFansAccountsCount;

                fansAccountReport.SuspendedFansAccountsCount = suspendedFansAccountsCount;

                if(overallFansAccountsCount > 0)
                {
                    if(activeFansAccountsCount > 0)
                    {
                        fansAccountReport.ActiveFansAccountsRate = ((decimal)activeFansAccountsCount / overallFansAccountsCount) * 100;
                    }

                    if (inactiveFansAccountsCount > 0)
                    {
                        fansAccountReport.InactiveFansAccountsRate = ((decimal)inactiveFansAccountsCount / overallFansAccountsCount) * 100;
                    }

                    if (suspendedFansAccountsCount > 0)
                    {
                        fansAccountReport.SuspendedFansAccountsRate = ((decimal)suspendedFansAccountsCount / overallFansAccountsCount) * 100;
                    }

                    decimal totalFansAccountsRate = fansAccountReport.ActiveFansAccountsRate +
                              fansAccountReport.InactiveFansAccountsRate +
                              fansAccountReport.SuspendedFansAccountsRate;
                    if (totalFansAccountsRate > 0)
                    {
                        decimal adjustmentFactor = 100 / totalFansAccountsRate;
                        fansAccountReport.ActiveFansAccountsRate *= adjustmentFactor;
                        fansAccountReport.InactiveFansAccountsRate *= adjustmentFactor;
                        fansAccountReport.SuspendedFansAccountsRate *= adjustmentFactor;
                    }
                }
            }


            return View(fansAccountsReports);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> PersonnelFinancialReports()
        {
            var personnelFinancialReports = await _context.PersonnelFinancialReports.ToListAsync();

            foreach (var personnelFinancialReport in personnelFinancialReports)
            {
                personnelFinancialReport.PendingPaymentFinesCount = personnelFinancialReport.RepayableFinesCount - personnelFinancialReport.PaidPaymentFinesCount;

                personnelFinancialReport.TotalUnpaidAmount = personnelFinancialReport.ExpectedRepayableAmount - personnelFinancialReport.TotalPaidAmount;

                if (personnelFinancialReport.RepayableFinesCount > 0)
                {
                    personnelFinancialReport.PaidFinesRate = ((decimal)personnelFinancialReport.PaidPaymentFinesCount / personnelFinancialReport.RepayableFinesCount) * 100;
                    personnelFinancialReport.PendingFinesRate = ((decimal)personnelFinancialReport.PendingPaymentFinesCount / personnelFinancialReport.RepayableFinesCount) * 100;
                    personnelFinancialReport.OverdueFinesRate = ((decimal)personnelFinancialReport.OverduePaymentFineCount / personnelFinancialReport.RepayableFinesCount) * 100;
                }
                else
                {
                    personnelFinancialReport.PaidFinesRate = 0;
                    personnelFinancialReport.PendingFinesRate = 0;
                    personnelFinancialReport.OverdueFinesRate = 0;
                }

                decimal totalFineRate = personnelFinancialReport.PaidFinesRate + personnelFinancialReport.PendingFinesRate + personnelFinancialReport.OverdueFinesRate;

                if (totalFineRate > 0)
                {
                    decimal adjustmentFactor = 100 / totalFineRate;
                    personnelFinancialReport.PaidFinesRate *= adjustmentFactor;
                    personnelFinancialReport.PendingFinesRate *= adjustmentFactor;
                    personnelFinancialReport.OverdueFinesRate *= adjustmentFactor;
                }
            }

            return View(personnelFinancialReports);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> PersonnelAccountsReports()
        {
            var personnelAccountsReports = await _context.PersonnelAccountsReports
                .ToListAsync();

            var overallPersonnelAccountsCount = await GetOverallPersonnelAccountsCountAsync();

            var activePersonnelAccountsCount = await GetActivePersonnelAccountsCountAsync();

            var inactivePersonnelAccountsCount = await GetInactivePersonnelAccountsCountAsync();

            var suspendedPersonnelAccountsCount = await GetSuspendedPersonnelAccountsCountAsync();


            foreach (var personnelAccountReport in personnelAccountsReports)
            {
                personnelAccountReport.OverallAccountsCount = overallPersonnelAccountsCount;

                personnelAccountReport.ActiveAccountsCount = activePersonnelAccountsCount;

                personnelAccountReport.InactiveAccountsCount = inactivePersonnelAccountsCount;

                personnelAccountReport.SuspendedAccountsCount = suspendedPersonnelAccountsCount;

                if(personnelAccountReport.OverallAccountsCount > 0)
                {
                    if(personnelAccountReport.ActiveAccountsCount > 0)
                    {
                        personnelAccountReport.ActiveAccountsRate = ((decimal)activePersonnelAccountsCount / overallPersonnelAccountsCount) * 100;
                    }

                    if(personnelAccountReport.InactiveAccountsCount > 0)
                    {
                        personnelAccountReport.InactiveAccountsRate = ((decimal)inactivePersonnelAccountsCount / overallPersonnelAccountsCount) *100;
                    }

                    if(personnelAccountReport.SuspendedAccountsCount > 0)
                    {
                        personnelAccountReport.SuspendedAccountsRate = ((decimal)suspendedPersonnelAccountsCount / overallPersonnelAccountsCount) * 100;
                    }

                    decimal totalPersonnelAccountsRate = personnelAccountReport.ActiveAccountsRate +
                               personnelAccountReport.InactiveAccountsRate +
                               personnelAccountReport.SuspendedAccountsRate;
                    if (totalPersonnelAccountsRate > 0)
                    {
                        decimal adjustmentFactor = 100 / totalPersonnelAccountsRate;
                        personnelAccountReport.ActiveAccountsRate *= adjustmentFactor;
                        personnelAccountReport.InactiveAccountsRate *= adjustmentFactor;
                        personnelAccountReport.SuspendedAccountsRate *= adjustmentFactor;
                    }

                }
            }


            return View(personnelAccountsReports);
        }

        public async Task<IActionResult> WarningsReports()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> TestFeedback()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> TestFeedback(FeedbackViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var feedback = new TestUserFeedback
                {
                    FeedbackText = viewModel.FeedbackText,
                    CreatedDateTime = DateTime.Now
                };

                _context.Add(feedback);
                await _context.SaveChangesAsync();

                TempData["Message"] = $"Thanks for sharing your feedback successfully. We value your opinion.";

                return RedirectToAction(nameof(TestFeedback));
            }

            return View(viewModel);
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> IndividualNewsReports()
        {
            var newsReports = await _context.IndividualNewsReports
                .Include(n => n.SportNews)
                .ToListAsync();

            return View(newsReports);
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> NewsReports()
        {
            var newsReports = await _context.OverallNewsReports
                .ToListAsync();

            return View(newsReports);
        }

        [Authorize(Roles = ("Club Administrator, Club Manager, Player"))]
        public async Task<IActionResult> ClubPerformanceReports()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!(user is ClubAdministrator clubAdministrator) &&
                !(user is ClubManager clubManager) &&
                !(user is Player clubPlayer))
            {
                return RedirectToAction("Error", "Home");
            }

            var clubId = (user as ClubAdministrator)?.ClubId ??
                         (user as ClubManager)?.ClubId ??
                         (user as Player)?.ClubId;

            if (clubId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var clubPerformanceReports = await _context.ClubPerformanceReports
                .Where(c => c.ClubId == clubId && c.League.IsCurrent)
                .Include(c => c.League)
                .Include(c => c.Club)
                .ToListAsync();

            var overallMatchCount = await GetOverallMatchCountAsync();



            foreach (var clubPerformanceReport in clubPerformanceReports)
            {
                clubPerformanceReport.GamesToPlayCount = overallMatchCount * overallMatchCount - 2;

                clubPerformanceReport.GamesNotPlayedCount = clubPerformanceReport.GamesToPlayCount - clubPerformanceReport.GamesPlayedCount;

                if (overallMatchCount > 0)
                {
                    clubPerformanceReport.GamesPlayedRate = ((decimal)clubPerformanceReport.GamesPlayedCount / overallMatchCount) * 100;
                    clubPerformanceReport.GamesNotPlayedRate = ((decimal)clubPerformanceReport.GamesNotPlayedCount / overallMatchCount) * 100;
                    clubPerformanceReport.GamesWinRate = ((decimal)clubPerformanceReport.GamesWinCount / overallMatchCount) * 100;
                    clubPerformanceReport.GamesLoseRate = ((decimal)clubPerformanceReport.GamesLoseCount / overallMatchCount) * 100;
                    clubPerformanceReport.GamesDrawRate = ((decimal)clubPerformanceReport.GamesDrawCount / overallMatchCount) * 100;

                    decimal totalMatchesRate = clubPerformanceReport.GamesPlayedRate + clubPerformanceReport.GamesNotPlayedRate;
                    if (totalMatchesRate > 0)
                    {
                        decimal adjustmentFactor = 100 / totalMatchesRate;
                        clubPerformanceReport.GamesPlayedRate *= adjustmentFactor;
                        clubPerformanceReport.GamesNotPlayedRate *= adjustmentFactor;
                    }

                    decimal totalPerformanceRate = clubPerformanceReport.GamesWinRate +
                                                   clubPerformanceReport.GamesLoseRate +
                                                   clubPerformanceReport.GamesDrawRate;
                    if (totalPerformanceRate > 0)
                    {
                        decimal adjustmentFactor = 100 / totalPerformanceRate;
                        clubPerformanceReport.GamesWinRate *= adjustmentFactor;
                        clubPerformanceReport.GamesLoseRate *= adjustmentFactor;
                        clubPerformanceReport.GamesDrawRate *= adjustmentFactor;
                    }
                }
            }

            var club = await _context.Club
                .FirstOrDefaultAsync(mo => mo.ClubId == clubId);

            ViewBag.ClubName = club?.ClubName;

            return View(clubPerformanceReports);
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> ClubTransferReport()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            if (!(user is ClubAdministrator clubAdministrator) &&
                !(user is ClubManager clubManager) &&
                !(user is Player clubPlayer))
            {
                return RedirectToAction("Error", "Home");
            }

            var clubId = (user as ClubAdministrator)?.ClubId ??
                         (user as ClubManager)?.ClubId ??
                         (user as Player)?.ClubId;

            if (clubId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var clubTransferReports = await _context.ClubTransferReports
                .Where(c => c.ClubId == clubId && 
                 c.League.IsCurrent)
                .Include(c => c.Club)
                .Include(c => c.League)
                .ToListAsync();

            foreach (var clubTransferReport in clubTransferReports)
            {
                var overallMatchCount = await GetOverallTransfersCountAsync(clubId.Value);

                clubTransferReport.OverallTransfersCount = overallMatchCount;

                clubTransferReport.NotActionedIncomingTransferCount = clubTransferReport.IncomingTransfersCount - 
                    (clubTransferReport.SuccessfulIncomingTransfersCount + 
                    clubTransferReport.RejectedIncomingTransfersCount);
                clubTransferReport.NotActionedOutgoigTransferCount = clubTransferReport.OutgoingTransfersCount -
                    (clubTransferReport.SuccessfulOutgoingTransfersCount + clubTransferReport.RejectedOutgoingTransfersCount);

                if (clubTransferReport.OverallTransfersCount > 0)
                {
                    var overallTransfersCount = clubTransferReport.OverallTransfersCount;


                    clubTransferReport.OutgoingTransferRate = ((decimal)clubTransferReport.OutgoingTransfersCount / overallTransfersCount) * 100;
                    clubTransferReport.IncomingTransferRate = ((decimal)clubTransferReport.IncomingTransfersCount / overallTransfersCount) * 100;
                    clubTransferReport.SuccessfullIncomingTransferRate = ((decimal)clubTransferReport.SuccessfulIncomingTransfersCount / clubTransferReport.IncomingTransfersCount) * 100;
                    clubTransferReport.SuccessfullOutgoingTransferRate = ((decimal)clubTransferReport.SuccessfulOutgoingTransfersCount / clubTransferReport.OutgoingTransfersCount) * 100;
                    clubTransferReport.RejectedIncomingTransferRate = ((decimal)clubTransferReport.RejectedIncomingTransfersCount / clubTransferReport.IncomingTransfersCount) * 100;
                    clubTransferReport.RejectedOutgoingTransferRate = ((decimal)clubTransferReport.RejectedOutgoingTransfersCount / clubTransferReport.OutgoingTransfersCount) * 100;
                    clubTransferReport.NotActionedIncomingTransferRate = ((decimal)clubTransferReport.IncomingTransfersCount - (clubTransferReport.SuccessfulIncomingTransfersCount + clubTransferReport.RejectedIncomingTransfersCount)) * 100;
                    clubTransferReport.NotActionedOutgoingTransferRate = ((decimal)clubTransferReport.OutgoingTransfersCount - (clubTransferReport.SuccessfulOutgoingTransfersCount + clubTransferReport.RejectedOutgoingTransfersCount)) * 100;

                    decimal totalTransfersRate = clubTransferReport.OutgoingTransferRate + clubTransferReport.IncomingTransferRate;
                    if (totalTransfersRate != 0)
                    {
                        decimal adjustmentFactor = 100 / totalTransfersRate;
                        clubTransferReport.OutgoingTransferRate *= adjustmentFactor;
                        clubTransferReport.IncomingTransferRate *= adjustmentFactor;
                    }

                    decimal totalSuccessRates = clubTransferReport.SuccessfullIncomingTransferRate + 
                        clubTransferReport.RejectedIncomingTransferRate +
                        clubTransferReport.NotActionedIncomingTransferRate;
                    if (totalSuccessRates != 0)
                    {
                        decimal adjustmentFactor = 100 / totalSuccessRates;
                        clubTransferReport.SuccessfullIncomingTransferRate *= adjustmentFactor;
                        clubTransferReport.RejectedIncomingTransferRate *= adjustmentFactor;
                        clubTransferReport.NotActionedIncomingTransferRate *= adjustmentFactor;
                    }

                    decimal totalRejectedRates = clubTransferReport.SuccessfullOutgoingTransferRate + 
                        clubTransferReport.RejectedOutgoingTransferRate +
                        clubTransferReport.NotActionedOutgoingTransferRate;
                    if (totalRejectedRates != 0)
                    {
                        decimal adjustmentFactor = 100 / totalRejectedRates;
                        clubTransferReport.SuccessfullOutgoingTransferRate *= adjustmentFactor;
                        clubTransferReport.RejectedOutgoingTransferRate *= adjustmentFactor;
                        clubTransferReport.NotActionedOutgoingTransferRate *= adjustmentFactor;
                    }


                }
            }

            var club = await _context.Club
                .FirstOrDefaultAsync(mo => mo.ClubId == clubId);

            ViewBag.ClubName = club?.ClubName;

            return View(clubTransferReports);
        }

        [Authorize(Roles = "Club Administrator, Club Manager, Player")]
        public async Task<IActionResult> PlayerPerformance()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var clubId = (user as ClubAdministrator)?.ClubId ??
                         (user as ClubManager)?.ClubId ??
                         (user as Player)?.ClubId;

            if (clubId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var club = await _context.Club.FirstOrDefaultAsync(mo => mo.ClubId == clubId);

            if (club == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var myPlayersPerformanceReports = await _context.PlayerPerformanceReports
                .Where(p => p.Player.ClubId == clubId
                        && p.League.IsCurrent
                        && !p.Player.IsDeleted)
                .Include(m => m.Player)
                .ToListAsync();

            ViewBag.ClubName = club.ClubName;

            return View(myPlayersPerformanceReports);
        }


        public async Task<IActionResult> TopScores()
        {
            var topScores = await _context.TopScores
                .Where(t => t.League.IsCurrent)
                .ToListAsync();

            return View(topScores);
        }

        public async Task<IActionResult> TopAssists()
        {
            var topAssists = await _context.TopScores
               .Where(t => t.League.IsCurrent)
               .ToListAsync();

            return View(topAssists);
        }

        public async Task<IActionResult> OverallPlayerStats()
        {
/*            var overallPlayerStats = await _context.PlayerPerformanceReports
                .Where( o => o.)*/

            return View();
        }


        [Authorize(Roles = ("Sport Administrator, Sport Coordinator"))]
        public async Task<IActionResult> MatchReports()
        {
            var matchReport = await _context.MatchReports
                .Where(m => m.Season.IsCurrent)
                .Include(m => m.Season)
                .FirstOrDefaultAsync();

            var overallMatchCount = await GetOverallMatchCountAsync();

            if (overallMatchCount > 0)
            {
                matchReport.MatchesToBePlayedCount = overallMatchCount * (overallMatchCount - 1);
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


        [Authorize(Roles = ("Sport Administrator, Sport Coordinator"))]
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


        public async Task<int> GetOverallFansAccountsCountAsync()
        {
            return await _context.Fans
                .CountAsync();
        }

        public async Task<int> GetActiveFansAccountsCountAsync()
        {
            return await _context.Fans
                .Where( f => f.IsActive)
                .CountAsync();
        }

        public async Task<int> GetInactiveFansAccountsCountAsync()
        {
            return await _context.Fans
                .Where(f => !f.IsActive)
                .CountAsync();
        }

        public async Task<int> GetSuspendedFansAccountsCountAsync()
        {
            return await _context.Fans
                .Where(f => !f.IsSuspended)
                .CountAsync();
        }

        public async Task<int> GetOverallPersonnelAccountsCountAsync()
        {
            var users = _userManager.Users.ToList();
            int count = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    count++;
                }
            }

            return count;
        }

        public async Task<int> GetActivePersonnelAccountsCountAsync()
        {
            var users = _userManager.Users
                .Where( u => u.IsActive).ToList();
            int count = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    count++;
                }
            }

            return count;
        }


        public async Task<int> GetInactivePersonnelAccountsCountAsync()
        {
            var users = _userManager.Users
                .Where(u => !u.IsActive).ToList();
            int count = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    count++;
                }
            }

            return count;
        }


        public async Task<int> GetSuspendedPersonnelAccountsCountAsync()
        {
            var users = _userManager.Users
                .Where(u => u.IsSuspended).ToList();
            int count = 0;

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Any())
                {
                    count++;
                }
            }

            return count;
        }


        public async Task<int> GetOverallMatchesToPlayCountAsync()
        {
            return await _context.Club
                .Where(c => c.League.IsCurrent)
                .CountAsync();
        }

        public async Task<int> GetOverallMatchResultsCountAsync()
        {
            return await _context.Club
                .Where(c => c.League.IsCurrent)
                .CountAsync();
        }

        public async Task<int> GetOverallTransfersCountAsync(int clubId)
        {
            return await _context.Transfer
                .Where(c =>
                    (c.SellerClub.ClubId == clubId || c.CustomerClub.ClubId == clubId) &&
                    (c.League.IsCurrent || c.League.IsCurrent))
                .CountAsync();
        }


        public async Task<int> GetOverallMatchCountAsync()
        {
            return await _context.Club
                  .Where(c => c.League.IsCurrent)
                  .CountAsync();
        }



        [Authorize(Roles = ("Personnel Administrator"))]
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
