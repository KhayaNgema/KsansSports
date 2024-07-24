using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class StandingsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;
        private readonly EmailService _emailService;
        private readonly IEncryptionService _encryptionService;

        public StandingsController(Ksans_SportsDbContext context, 
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger,
            EmailService emailService,
            IEncryptionService encryptionService)
        {
            _context = context;
            _userManager = userManager; 
            _activityLogger = activityLogger;
            _emailService =  emailService;
            _encryptionService = encryptionService;
        }

        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> StandingsBackOffice()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }

            var standings = _context.Standing
                .Where(s => s.LeagueId == currentLeague.LeagueId)
                .Include(s => s.Club)
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .ThenBy(s => s.MatchPlayed)
                .ToList();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();

            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(standings);
        }

        [Authorize(Roles =("Sport Administrator"))]
        public async Task<IActionResult> Standings()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }

            var standings= _context.Standing
                .Where(s => s.League.IsCurrent)
                .Include(s => s.Club)
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .ThenBy(s => s.MatchPlayed)
                .ToList();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();

            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(standings);
        }


        public async Task<IActionResult> Index()
        {
            ViewBag.Leagues = await _context.League
                .OrderByDescending(l => l.CreatedDateTime)
                .ToListAsync();

            return PartialView("_StandingsPartial");
        }


        public async Task<IActionResult> StandingsTable(int? leagueId)
        {
            var standings = await _context.Standing
                .Where(s => s.LeagueId == leagueId)
                .Include(s => s.Club)
                .OrderByDescending(s => s.Points)
                .ThenByDescending(s => s.GoalDifference)
                .ThenBy(s => s.MatchPlayed)
                .ToListAsync();

            return PartialView("_StandingsTablePartial", standings);
        }

        [Authorize(Policy = "AnyRole")]
        public async Task<IActionResult> BackOfficeStandings()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }
            var standings = _context.Standing
                .Where(f => f.LeagueId == currentLeague.LeagueId)
                                  .Include(s => s.Club)
                                  .OrderByDescending(s => s.Points)
                                  .ThenByDescending(s => s.GoalDifference)
                                  .ThenBy(s => s.MatchPlayed);

            var currentSeason = await _context.League
               .Where(c => c.IsCurrent)
               .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return PartialView("_BackOfficeStandingsPartial", await standings.ToListAsync());
        }


        [Authorize]
        public async Task<IActionResult> ClubStandings(int? clubId)
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }
            var standings = _context.Standing
                .Where(f => f.LeagueId == currentLeague.LeagueId)
                                  .Include(s => s.Club)
                                  .OrderByDescending(s => s.Points)
                                  .ThenByDescending(s => s.GoalDifference)
                                  .ThenBy(s => s.MatchPlayed);


            ViewBag.SelectedClubId = clubId;

            return PartialView("_ClubStandingPartial", await standings.ToListAsync());
        }


        [Authorize(Roles =("Sport Administrator"))]
        public async Task<IActionResult> EditPoints(string standingId)
        {
            var logMessages = new List<string>();

            var decryptedStandingId = _encryptionService.DecryptToInt(standingId);

            if (decryptedStandingId == null)
            {
                var message = "EditPoints called with null id";
                Console.WriteLine(message);
                logMessages.Add(message);
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var standing = await _context.Standing
                                        .Include(s => s.Club)
                                        .FirstOrDefaultAsync(s => s.StandingId == decryptedStandingId);

            if (standing == null)
            {
                var message = $"Standing with id {decryptedStandingId} not found";
                Console.WriteLine(message);
                logMessages.Add(message);
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var viewModel = new StandingPointsViewModel
            {
                StandingId = decryptedStandingId,
                Points = standing.Points,
                Goals = standing.GoalDifference,
                ClubName = standing.Club?.ClubName,
                ClubBadge = standing.Club?.ClubBadge,
                Reason = standing.Reason,
            };

            ViewBag.ClubName = standing?.Club?.ClubName;

            TempData["LogMessages"] = logMessages;
            return View(viewModel);
        }


        [Authorize(Roles = ("Sport Administrator"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPoints(StandingPointsViewModel viewModel)
        {
            var logMessages = new List<string>();

            var standing = await _context.Standing
                .Include(s => s.Club)
                .FirstOrDefaultAsync(s => s.StandingId == viewModel.StandingId);

            if (standing == null)
            {
                var message = $"Standing with id {viewModel.StandingId} not found";
                Console.WriteLine(message);
                logMessages.Add(message);
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubName = standing.Club.ClubName;

            if (!string.IsNullOrEmpty(viewModel.ClubCode) && standing.Club.ClubCode != viewModel.ClubCode)
            {
                TempData["Message"] = $"The club code you have provided does not match {clubName} code. Please try again!";
                ViewBag.ClubName = standing?.Club?.ClubName;
                viewModel.ClubBadge = standing?.Club.ClubBadge;
                viewModel.Points = standing.Points;
                viewModel.Goals = standing.GoalDifference;
                TempData["LogMessages"] = logMessages;

                return View(viewModel);
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var originalPoints = standing.Points;
                var originalGoals = standing.GoalDifference;

                standing.Reason = viewModel.Reason;
                standing.ModifiedDateTime = DateTime.Now;
                standing.ModifiedById = userId;

                if (viewModel.PointsToBeAdded != 0)
                {
                    standing.Points = standing.Points + viewModel.PointsToBeAdded;
                }
                else if (viewModel.PointsToBeSubtracted != 0)
                {
                    standing.Points = standing.Points - viewModel.PointsToBeSubtracted;
                }

                if (viewModel.GoalsToBeSubtracted != 0)
                {
                    standing.GoalsConceded = standing.GoalsConceded + viewModel.GoalsToBeSubtracted;
                    standing.GoalsScored = standing.GoalsScored - viewModel.GoalsToBeSubtracted;
                }
                else if (viewModel.GoalsToBeAdded != 0)
                {
                    standing.GoalsScored = standing.GoalsScored + viewModel.GoalsToBeAdded;
                }

                standing.GoalDifference = standing.GoalsScored - standing.GoalsConceded;

                viewModel.Points = standing.Points;
                viewModel.Goals = standing.GoalDifference;

                TempData["Message"] = $"You have successfully updated standings for {standing.Club.ClubName} and the new points are {viewModel.Points}";

                TempData["LogMessages"] = logMessages;

                await _activityLogger.Log($"Modified {standing.Club.ClubName} points from {originalPoints} to {standing.Points} and goals from {originalGoals} to {standing.GoalDifference}", user.Id);

                var clubEmail = standing.Club.Email; 
                var subject = "Standings Updated Notification";
                var body = $@"
            Dear {standing.Club.ClubName},<br/><br/>
            Please note that your standings have been updated and your new standings are:<br/><br/>
            Points: {standing.Points}<br/>
            Goal Difference: {standing.GoalDifference}<br/><br/>
            Reason for updating: {viewModel.Reason}.<br/><br/>
            Please check the standings tables for updated standings.<br/><br/>
            If you have any questions, please contact us at support@ksfoundation.com.<br/><br/>
            Regards,<br/>
            K&S Foundation Management
                ";

                BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(clubEmail, subject, body));

                return RedirectToAction(nameof(Standings));
            }

            return View(viewModel);
        }

        private bool StandingExists(int id)
        {
            return _context.Standing.Any(e => e.StandingId == id);
        }
    }
}
