using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class StandingsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;

        public StandingsController(Ksans_SportsDbContext context, 
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger)
        {
            _context = context;
            _userManager = userManager; 
            _activityLogger = activityLogger;
        }

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


        public async Task<IActionResult> Standings()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }

            var standings= _context.Standing
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

        // GET: Standings
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


        // GET: Standings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Standing == null)
            {
                return NotFound();
            }

            var standing = await _context.Standing
                .Include(s => s.Club)
                .FirstOrDefaultAsync(m => m.StandingId == id);
            if (standing == null)
            {
                return NotFound();
            }

            return View(standing);
        }

        // GET: Standings/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubName");
            return View();
        }

        // POST: Standings/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StandingsViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                            
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                Standing newStanding = new Standing
                {
                    Position = viewModel.Position,
                    Draw = viewModel.Draw,
                    Points = viewModel.Points,  
                    MatchPlayed = viewModel.MatchPlayed,    
                    ClubId = viewModel.ClubId,  
                    GoalDifference = viewModel.GoalsScored - viewModel.GoalsConceded,  
                    GoalsConceded = viewModel.GoalsConceded,    
                    GoalsScored = viewModel.GoalsScored,    
                    Lose=viewModel.Lose,    
                    Wins=viewModel.Wins,  
                    CreatedById = userId,
                    ModifiedById = userId,
                    CreatedDateTime = DateTime.Now,  
                    ModifiedDateTime = DateTime.Now,    

                };

                _context.Add(newStanding);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubName", viewModel.ClubId);
            return View(viewModel);
        }


        public async Task<IActionResult> EditPoints(int? id)
        {
            var logMessages = new List<string>();

            if (id == null)
            {
                var message = "EditPoints called with null id";
                Console.WriteLine(message);
                logMessages.Add(message);
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var standing = await _context.Standing
                                        .Include(s => s.Club)
                                        .FirstOrDefaultAsync(s => s.StandingId == id);

            if (standing == null)
            {
                var message = $"Standing with id {id} not found";
                Console.WriteLine(message);
                logMessages.Add(message);
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var viewModel = new StandingPointsViewModel
            {
                StandingId = standing.StandingId,
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
                return RedirectToAction(nameof(Standings));
            }

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    var errorMessage = $"Error in {state.Key}: {error.ErrorMessage}";
                    Console.WriteLine(errorMessage);
                    logMessages.Add(errorMessage);
                }
            }

            ViewBag.ClubName = standing?.Club?.ClubName;
            viewModel.ClubBadge = standing?.Club?.ClubBadge;
            viewModel.Points = standing.Points;
            viewModel.Goals = standing.GoalDifference;
            TempData["LogMessages"] = logMessages;

            return View(viewModel);
        }




        private bool StandingExists(int id)
        {
            return _context.Standing.Any(e => e.StandingId == id);
        }

        // GET: Standings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Standing == null)
            {
                return NotFound();
            }

            var standing = await _context.Standing
                .Include(s => s.Club)
                .FirstOrDefaultAsync(m => m.StandingId == id);
            if (standing == null)
            {
                return NotFound();
            }

            return View(standing);
        }

        // POST: Standings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Standing == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Standing'  is null.");
            }
            var standing = await _context.Standing.FindAsync(id);
            if (standing != null)
            {
                _context.Standing.Remove(standing);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
