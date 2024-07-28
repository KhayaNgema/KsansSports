using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class MatchResultsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;
        private readonly EmailService _emailServcie;
        private readonly IEncryptionService _encryptionService;

        public MatchResultsController(Ksans_SportsDbContext context,
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger,
            EmailService emailService,
            IEncryptionService encryptionService)
        {
            _context = context;
            _userManager = userManager;
            _activityLogger = activityLogger;
            _emailServcie = emailService;
            _encryptionService = encryptionService;
        }


        public async Task<IActionResult> LeagueResults()
        {
            return PartialView("LeagueResultsPartial");
        }

        [Authorize(Roles = ("Sport Administrator, Sport Coordinator"))]
        public async Task<IActionResult> MatchResultsBackOffice()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }
            var matchResults = await _context.MatchResult
                .Where(m => m.LeagueId == currentLeague.LeagueId)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.CreatedBy)
                .Include(m => m.ModifiedBy)
                .ToListAsync();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(matchResults);
        }


        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> MatchResults()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }
            var matchResults = await _context.MatchResult
                .Where(m => m.LeagueId == currentLeague.LeagueId)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .Include(m => m.CreatedBy)
                .Include(m => m.ModifiedBy)
                .ToListAsync();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(matchResults);
        }

        [Authorize(Roles =("Club Administrator, Club Manager, Player"))]
        public async Task<IActionResult> MyMatchResults()
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

            var club = await _context.Club.FindAsync(clubId);

            if (club == null)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ClubName = club.ClubName;

            var matchResults = await _context.MatchResult
                                             .Where( m => m.League.IsCurrent)
                                             .Include(m => m.HomeTeam)
                                             .Include(m => m.AwayTeam)
                                             .Where(mo => mo.HomeTeam.ClubId == clubId || mo.AwayTeam.ClubId == clubId)
                                             .ToListAsync();

            return View(matchResults);
        }

        public async Task<IActionResult> Index()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }


            var matchResults = await _context.MatchResult
               .Where(m => m.LeagueId == currentLeague.LeagueId)
               .Include(m => m.HomeTeam)
               .Include(m => m.AwayTeam)
               .Include(m => m.CreatedBy)
               .Include(m => m.ModifiedBy)
               .ToListAsync();

            return PartialView("_MatchResultsPartial", matchResults);
        }

        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> BackOfficeMatchResults()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }

            var matchResults = await _context.MatchResult
                .Where(m => m.LeagueId == currentLeague.LeagueId)
                .Include(m => m.CreatedBy)
                .Include(m => m.ModifiedBy)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToListAsync();

            return PartialView("_BackOfficeMatchResultsPartial", matchResults);
        }

        [Authorize(Roles = ("Sport Administrator, Sport Coordinator"))]


        [Authorize(Roles = ("Sport Administrator, Sport Coordinator"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int fixtureId, StartLiveViewModel viewModel)
        {
            try
            {
                    var liveMatch = await _context.Live
                        .Where(l => l.FixtureId == fixtureId)
                        .Include(l => l.Fixture)
                        .FirstOrDefaultAsync();

                    if (liveMatch == null)
                    {
                        return Json(new { success = false, message = "Live match not found." });
                    }

                    liveMatch.IsLive = false;
                    liveMatch.IsHalfTime = false;
                    liveMatch.ISEnded = true;
                    liveMatch.LiveTime = 90;

                    _context.Update(liveMatch);
                    await _context.SaveChangesAsync();

                    RecurringJob.RemoveIfExists($"update-live-time-{liveMatch.LiveId}");

                    var fixture = await _context.Fixture
                        .Where(f => f.FixtureId == fixtureId)
                        .Include(f => f.HomeTeam)
                        .Include(f => f.AwayTeam)
                        .FirstOrDefaultAsync();

                    var user = await _userManager.GetUserAsync(User);
                    var userId = user?.Id;

                    if (user == null || userId == null)
                    {
                        throw new InvalidOperationException("User not authenticated or user ID not found.");
                    }

                    var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

                    if (currentLeague == null)
                    {
                        ModelState.AddModelError(string.Empty, "No current league found.");
                        return Json(new { success = false, message = "No current league found." });
                    }

                    var matchResultsReport = await _context.MatchResultsReports
                        .Where(m => m.Season.IsCurrent)
                        .Include(m => m.Season)
                        .FirstOrDefaultAsync();

                    var matchReport = await _context.MatchReports
                        .Where(m => m.Season.IsCurrent)
                        .Include(m => m.Season)
                        .FirstOrDefaultAsync();

                    var newMatchResults = new MatchResults
                    {
                        LeagueId = currentLeague.LeagueId,
                        HomeTeamId = fixture.HomeTeam.ClubId,
                        AwayTeamId = fixture.AwayTeam.ClubId,
                        CreatedById = userId,
                        ModifiedById = userId,
                        CreatedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                        MatchDate = fixture.KickOffDate,
                        MatchTime = fixture.KickOffTime,
                        FixtureId = fixtureId,
                        HomeTeamScore = liveMatch.HomeTeamScore,
                        AwayTeamScore = liveMatch.AwayTeamScore,
                        Location = fixture.Location,
                    };

                    _context.Add(newMatchResults);
                    await _context.SaveChangesAsync();

                    var homeStanding = await _context.Standing
                        .Where(s => s.LeagueId == currentLeague.LeagueId && s.ClubId == fixture.HomeTeam.ClubId)
                        .FirstOrDefaultAsync();

                    if (homeStanding != null)
                    {
                        homeStanding.MatchPlayed++;
                        homeStanding.GoalsScored += liveMatch.HomeTeamScore;
                        homeStanding.GoalsConceded += liveMatch.AwayTeamScore;
                        homeStanding.GoalDifference += liveMatch.HomeTeamScore - liveMatch.AwayTeamScore;
                        UpdateStandingStats(homeStanding, liveMatch.HomeTeamScore, liveMatch.AwayTeamScore);

                        homeStanding.ModifiedById = userId;
                        homeStanding.ModifiedDateTime = DateTime.Now;

                        _context.Update(homeStanding);
                        await _context.SaveChangesAsync();
                    }

                    var awayStanding = await _context.Standing
                        .Where(s => s.LeagueId == currentLeague.LeagueId && s.ClubId == fixture.AwayTeam.ClubId)
                        .FirstOrDefaultAsync();

                    if (awayStanding != null)
                    {
                        awayStanding.MatchPlayed++;
                        awayStanding.GoalsScored += liveMatch.AwayTeamScore;
                        awayStanding.GoalsConceded += liveMatch.HomeTeamScore;
                        awayStanding.GoalDifference += liveMatch.AwayTeamScore - liveMatch.HomeTeamScore;
                        UpdateStandingStats(awayStanding, liveMatch.AwayTeamScore, liveMatch.HomeTeamScore);

                        awayStanding.ModifiedById = userId;
                        awayStanding.ModifiedDateTime = DateTime.Now;

                        _context.Update(awayStanding);
                        await _context.SaveChangesAsync();
                    }

                    await _context.SaveChangesAsync();

                    var homeHeadToHead = new HeadTohead
                    {
                        HeadToHeadDate = fixture.KickOffDate,
                        ClubId = fixture.HomeTeam.ClubId,
                        HomeTeamId = fixture.HomeTeam.ClubId,
                        AwayTeamId = fixture.AwayTeam.ClubId,
                        MatchResults = liveMatch.HomeTeamScore > liveMatch.AwayTeamScore ? "W" :
                                       liveMatch.HomeTeamScore == liveMatch.AwayTeamScore ? "D" :
                                       "L",
                        AwayTeamGoals = liveMatch.AwayTeamScore,
                        HomeTeamGoals = liveMatch.HomeTeamScore
                    };

                    _context.Add(homeHeadToHead);
                    await _context.SaveChangesAsync();

                    var awayHeadToHead = new HeadTohead
                    {
                        HeadToHeadDate = fixture.KickOffDate,
                        ClubId = fixture.AwayTeam.ClubId,
                        HomeTeamId = fixture.HomeTeam.ClubId,
                        AwayTeamId = fixture.AwayTeam.ClubId,
                        MatchResults = liveMatch.AwayTeamScore > liveMatch.HomeTeamScore ? "W" :
                                       liveMatch.AwayTeamScore == liveMatch.HomeTeamScore ? "D" :
                                       "L",
                        AwayTeamGoals = liveMatch.AwayTeamScore,
                        HomeTeamGoals = liveMatch.HomeTeamScore
                    };

                    _context.Add(awayHeadToHead);
                    await _context.SaveChangesAsync();

                    matchResultsReport.ReleasedResultsCount++;
                    if (liveMatch.HomeTeamScore > liveMatch.AwayTeamScore || liveMatch.AwayTeamScore > liveMatch.HomeTeamScore)
                    {
                        matchResultsReport.WinsCount++;
                        matchResultsReport.LosesCount++;
                    }
                    else
                    {
                        matchResultsReport.DrawsCount++;
                    }

                    matchReport.PlayedMatchesCounts++;

                    await _context.SaveChangesAsync();

                    var savedMatchResults = await _context.MatchResult
                        .Where(m => m.Equals(newMatchResults))
                        .Include(m => m.HomeTeam)
                        .Include(m => m.AwayTeam)
                        .FirstOrDefaultAsync();

                    var homeTeamPerformanceReport = await _context.ClubPerformanceReports
                        .Where(h => h.ClubId == savedMatchResults.HomeTeam.ClubId &&
                                    h.League.IsCurrent)
                        .FirstOrDefaultAsync();

                    var awayTeamPerformanceReport = await _context.ClubPerformanceReports
                        .Where(h => h.ClubId == savedMatchResults.AwayTeam.ClubId &&
                                    h.League.IsCurrent)
                        .FirstOrDefaultAsync();

                    if (liveMatch.HomeTeamScore > liveMatch.AwayTeamScore)
                    {
                        homeTeamPerformanceReport.GamesWinCount++;
                        awayTeamPerformanceReport.GamesLoseCount++;
                    }
                    else if (liveMatch.HomeTeamScore < liveMatch.AwayTeamScore)
                    {
                        homeTeamPerformanceReport.GamesLoseCount++;
                        awayTeamPerformanceReport.GamesWinCount++;
                    }
                    else
                    {
                        homeTeamPerformanceReport.GamesDrawCount++;
                        awayTeamPerformanceReport.GamesDrawCount++;
                    }

                    homeTeamPerformanceReport.GamesPlayedCount++;
                    awayTeamPerformanceReport.GamesPlayedCount++;

                    await _context.SaveChangesAsync();

                    var fixtureToUpdate = await _context.Fixture.FindAsync(fixtureId);

                    if (fixtureToUpdate != null)
                    {
                        fixtureToUpdate.FixtureStatus = FixtureStatus.Ended;
                        _context.Update(fixtureToUpdate);
                        await _context.SaveChangesAsync();
                    }

                    TempData["Message"] = $"You have successfully uploaded results for a match between {savedMatchResults.HomeTeam.ClubName} and {savedMatchResults.AwayTeam.ClubName}";

                    return Json(new { success = true, message = "Match successfully ended." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while ending the match." });
            }
        }


        private async Task UpdateStandingsOrder()
        {
            var standings = await _context.Standing.ToListAsync();
            var orderedStandings = standings.OrderByDescending(s => s.Points)
                                            .ThenByDescending(s => s.GoalDifference)
                                            .ThenBy(s => s.MatchPlayed)
                                            .ThenByDescending(s => s.Last5Games)
                                            .ToList();

            for (int i = 0; i < orderedStandings.Count; i++)
            {
                var standing = orderedStandings[i];
                standing.Position = i + 1;
                _context.Update(standing);
            }

            await _context.SaveChangesAsync();
        }

        private void UpdateStandingStats(Standing standing, int teamScore, int opponentScore)
        {
            if (teamScore > opponentScore)
            {
                standing.Points += 3;
                standing.Wins++;
                standing.Last5Games = AppendOutcome(standing.Last5Games, "W");
            }
            else if (teamScore == opponentScore)
            {
                standing.Points++;
                standing.Draw++;
                standing.Last5Games = AppendOutcome(standing.Last5Games, "D");
            }
            else
            {
                standing.Lose++;
                standing.Last5Games = AppendOutcome(standing.Last5Games, "L");
            }

            if (standing.Last5Games.Length > 5)
            {
                standing.Last5Games = standing.Last5Games.Substring(1);
            }
        }

        private string AppendOutcome(string last5Games, string outcome)
        {
            if (last5Games.Length >= 5)
            {
                last5Games = last5Games.Substring(1);
            }
            return last5Games + outcome;
        }

        private bool MatchResultsExists(int id)
        {
          return (_context.MatchResult?.Any(e => e.ResultsId == id)).GetValueOrDefault();
        }
    }
}
