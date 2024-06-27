using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Models;
using MyField.ViewModels;
using MyField.Services;
using Microsoft.AspNetCore.Identity;
using MyField.Migrations;
using System.ComponentModel.DataAnnotations;
using MyField.Interfaces;

namespace MyField.Controllers
{
    public class ClubsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;

        public ClubsController(Ksans_SportsDbContext context, 
            FileUploadService fileUploadService, 
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
            _activityLogger = activityLogger;
        }

        public async Task<IActionResult> ClubPlayers(int clubId)
        {
            var clubPlayers = await _context.Player
                .Where(p => p.ClubId == clubId)
                .Include(s => s.Club)   
                .ToListAsync();

            return PartialView("_ClubPlayersPartial", clubPlayers);
        }

        public async Task<IActionResult> HeadToHead(int clubId)
        {
            // Retrieve the head-to-head records for the specified clubId
            var headToHead = await _context.HeadToHead
                .Include(s => s.HomeTeam)
                .Include(s => s.AwayTeam)
                .Where(h => h.ClubId == clubId)
                .ToListAsync();

            // Pass the filtered head-to-head records to the partial view
            return PartialView("_ClubStatsPartial", headToHead);
        }

        // GET: MatchResults
        public async Task<IActionResult> Clubs()
        {
            var clubs = await _context.Club
                .Include(s => s.CreatedBy)
                .Include(s => s.ModifiedBy)
                .ToListAsync();

            foreach(var club in clubs)
            {
                var clubManager = await _context.ClubManager
              .Where(mo => mo.ClubId == club.ClubId)
              .FirstOrDefaultAsync();

                if (clubManager != null && clubManager.FirstName != null && clubManager.LastName != null)
                {
                    ViewBag.ClubManager = $"{clubManager.FirstName} {clubManager.LastName}";
                }
                else
                {
                    ViewBag.ClubManager = "N/A";
                }
            }


            return View(clubs);
        }

        // GET: ClubsBackOffice
        public async Task<IActionResult> ClubsBackOffice()
        {
            var clubs = await _context.Club
                .Include(s => s.CreatedBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.ClubManager)
                .Include(s => s.League)
                .OrderByDescending(s => s.CreatedDateTime)
                .ToListAsync();

            return View(clubs);
        }


        // GET: MatchResults
        public async Task<IActionResult> MyClub()
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

            var clubs = await _context.Club
                                      .FirstOrDefaultAsync(mo => mo.ClubId == clubId);

            ViewBag.ClubName = clubs?.ClubName;

            return View(clubs);
        }


        public async Task<IActionResult> Index()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }


            var clubs = await _context.Club
                .Where(mo => mo.LeagueId == currentLeague.LeagueId)
                .ToListAsync();

            return PartialView("_ClubsPartial", clubs);
        }

        public async Task<IActionResult> BackOfficeClubs()
        {
            var clubs = await _context.Club
                .ToListAsync();

            return PartialView("_BackOfficeClubsPartial", clubs);
        }

        public async Task<IActionResult> ClubSummary(int? clubId)
        {
            if (clubId == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .Include(c => c.ClubManager)
                .FirstOrDefaultAsync(m => m.ClubId == clubId);
            if (club == null)
            {
                return NotFound();
            }

            return PartialView("_ClubSummaryPartial", club);
        }




        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Club == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        public async Task<IActionResult> RejoinSeason(int clubId)
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
                return View();
            }

            var existingClub = await _context.Club.FindAsync(clubId);

            if (existingClub == null)
            {
                ModelState.AddModelError(string.Empty, "Club not found.");
                return View();
            }

            var existingStanding = await _context.Standing.FirstOrDefaultAsync(s => s.LeagueId == currentLeague.LeagueId && s.ClubId == existingClub.ClubId);

            if (existingStanding != null)
            {
                TempData["Message"] = $"{existingClub.ClubName} has already joined the current league.";
                return RedirectToAction("ClubsBackOffice");
            }

            existingClub.LeagueId = currentLeague.LeagueId;
            existingClub.IsActive = true;
            existingClub.Status = ClubStatus.Active;

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

             var matchResultsReport = await _context.MatchResultsReports
                .Where(m => m.Season.IsCurrent)
                .Include(m => m.Season)
                .FirstOrDefaultAsync();

            var newStanding = new Standing
            {
                LeagueId = currentLeague.LeagueId,
                ClubId = existingClub.ClubId,
                CreatedDateTime = DateTime.Now,
                ModifiedDateTime = DateTime.Now,
                CreatedById = userId,
                ModifiedById = userId,
                Position = 0,
                Draw = 0,
                Points = 0,
                MatchPlayed = 0,
                GoalDifference = 0,
                GoalsConceded = 0,
                GoalsScored = 0,
                Lose = 0,
                Wins = 0
            };

            matchResultsReport.ExpectedResultsCount++;

            _context.Add(newStanding);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"{existingClub.ClubName} has successfully rejoined the current league.";
            return RedirectToAction("ClubsBackOffice");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClubViewModel viewModel, IFormFile ClubBadges)
        {
            if (!User.Identity.IsAuthenticated)
            {
                string errorMessage = "You need to have a valid account and be logged into this system to add a club";
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = errorMessage });
            }

            if (!User.IsInRole("System Administrator"))
            {
                string errorMessage = "Only a system administrator can add a club into this system";
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = errorMessage });
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

                var matchResultsReport = await _context.MatchResultsReports
                     .Where(m => m.Season.IsCurrent)
                     .Include(m => m.Season)
                     .FirstOrDefaultAsync();

                if (currentLeague == null)
                {
                    ModelState.AddModelError(string.Empty, "No current league found.");
                    return View(viewModel);
                }

                var newClub = new Club
                {
                    LeagueId = currentLeague.LeagueId,
                    ClubName = viewModel.ClubName,
                    ClubLocation = viewModel.ClubLocation,
                    ClubAbbr = viewModel.ClubAbbr,
                    CreatedById = userId,
                    ModifiedById = userId,
                    ClubHistory = viewModel.ClubHistory,
                    ClubSummary = viewModel.ClubSummary,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    Status = ClubStatus.Active,
                    IsActive = true,
                    ClubCode = GenerateClubCode(viewModel),
                 /*   ClubManagerId = "485ebf7a - 68a9 - 4416 - a32a - 78435ed8ba0d",*/
                };

                if (ClubBadges != null && ClubBadges.Length > 0)
                {
                    var uploadedImagePath = await _fileUploadService.UploadFileAsync(ClubBadges);

                    newClub.ClubBadge = uploadedImagePath;
                }

                _context.Add(newClub);
                await _context.SaveChangesAsync();

                await _activityLogger.Log($"Added {newClub.ClubName} as a new club", user.Id);

                var newStanding = new Standing
                {
                    LeagueId = currentLeague.LeagueId,
                    ClubId = newClub.ClubId,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedById = userId,
                    ModifiedById = userId,
                    Position = 0,
                    Draw = 0,
                    Points = 0,
                    MatchPlayed = 0,
                    GoalDifference = 0,
                    GoalsConceded = 0,
                    GoalsScored = 0,
                    Lose = 0,
                    Wins = 0
                };

                matchResultsReport.ExpectedResultsCount++;

                _context.Add(newStanding);
                await _context.SaveChangesAsync();
                TempData["Message"] = $"{viewModel.ClubName} has been added successfully.";
                return RedirectToAction(nameof(ClubsBackOffice));
            }

            return View(viewModel);
        }


        private string GenerateClubCode(ClubViewModel viewModel)
        {
            var year = DateTime.Now.Year.ToString().Substring(2);
            var month = DateTime.Now.Month.ToString("D2");
            var day = DateTime.Now.Day.ToString("D2");

            var clubNameAbbreviation = viewModel.ClubName.Substring(0, 3).ToUpper();

            const string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var random = new Random();
            var randomLetters = new string(Enumerable.Repeat(letters, 3)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"{year}{month}{day}{clubNameAbbreviation}{randomLetters}";
        }



        public async Task<IActionResult> ClubManagerDetails(string id)
        {
            if (id == null || _context.Club == null)
            {
                return NotFound();
            }

            var clubManager = await _context.ClubManager
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clubManager == null)
            {
                return NotFound();
            }

            return View(clubManager);
        }

        public async Task<IActionResult> ClubPlayerDetails(string id)
        {
            if (id == null || _context.Club == null)
            {
                return NotFound();
            }

            var clubPlayer = await _context.Player
                .FirstOrDefaultAsync(m => m.Id == id);
            if (clubPlayer  == null)
            {
                return NotFound();
            }

            return View(clubPlayer);
        }

        public async Task<IActionResult> SuspendClub(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var club = await _context.Club
                .Where( c => c.ClubId == id)
                .FirstOrDefaultAsync(); 

            if (club.Status == ClubStatus.Suspended)
            {
                TempData["Message"] = $"This club is already suspended.";

                return RedirectToAction(nameof(ClubsBackOffice));
            }
            

            club.Status = ClubStatus.Suspended;
            club.ModifiedById = user.Id;
            club.ModifiedDateTime = DateTime.Now;

            _context.Update(club);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"You have successfully unsuspended {club.ClubName} and now they will be able to access all features of the system.";

            return RedirectToAction(nameof(ClubsBackOffice));
        }

        public async Task<IActionResult> UnsuspendClub(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var club = await _context.Club
                .Where(c => c.ClubId == id)
                .FirstOrDefaultAsync();

            if (club.Status == ClubStatus.Active)
            {
                TempData["Message"] = $"This club is already active";

                return RedirectToAction(nameof(ClubsBackOffice));
            }

            club.Status = ClubStatus.Active;
            club.ModifiedById = user.Id;
            club.ModifiedDateTime = DateTime.Now;

            _context.Update(club);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"You have successfully suspended {club.ClubName} and now they won't be able to access some system features due to te decision you have made.";

            return RedirectToAction(nameof(ClubsBackOffice));
        }



        // GET: Club/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var logMessages = new List<string>();

            if (id == null || _context.Club == null)
            {
                var message = $"Edit called with null id or Club context is null";
                Console.WriteLine(message);
                logMessages.Add(message);
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var club = await _context.Club.FindAsync(id);
            if (club == null)
            {
                var message = $"Club with id {id} not found";
                Console.WriteLine(message);
                logMessages.Add(message);
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubViewModel = new UpdateClubViewModel
            {
                ClubId = club.ClubId,
                ClubName = club.ClubName,
                ClubLocation = club.ClubLocation,
                ClubAbbr = club.ClubAbbr,
                ClubBadges = club.ClubBadge,
                ClubHistory = club.ClubHistory,
                ClubSummary = club.ClubSummary
            };

            TempData["LogMessages"] = logMessages;
            return View(clubViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateClubViewModel viewModel, IFormFile ClubBadgeFile)
        {
            var logMessages = new List<string>();

            var user = await _userManager.GetUserAsync(User);

            if (id != viewModel.ClubId)
            {
                return NotFound();
            }

            if (ValidateUpdatedProperties(viewModel))
            {
                var club = await _context.Club.FindAsync(id);

                try
                {

                    if (club == null)
                    {
                        return NotFound();
                    }

                    club.ClubAbbr = viewModel.ClubAbbr;
                    club.ClubName = viewModel.ClubName;
                    club.ClubLocation = viewModel.ClubLocation;
                    club.ClubHistory = viewModel.ClubHistory;
                    club.ClubSummary = viewModel.ClubSummary;

                    if (ClubBadgeFile != null && ClubBadgeFile.Length > 0)
                    {
                        var uploadedImagePath = await _fileUploadService.UploadFileAsync(ClubBadgeFile);
                        club.ClubBadge = uploadedImagePath;
                    }

                    _context.Update(club);
                    await _context.SaveChangesAsync();

                    await _activityLogger.Log($"Updated {club.ClubName} information", user.Id);
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    if (!ClubExists(viewModel.ClubId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                }
           
                if(User.IsInRole("Club Administrator"))
                {
                    TempData["Message"] = $"{club.ClubName} information has been updated successfully.";
                    return RedirectToAction(nameof(MyClub));
                }
                else if (User.IsInRole("Sport Administrator"))
                {
                    TempData["Message"] = $"{club.ClubName} information has been updated successfully.";
                    return RedirectToAction(nameof(ClubsBackOffice));
                }
                else if (User.IsInRole("Sport Coordinator"))
                {
                    TempData["Message"] = $"{club.ClubName} information has been updated successfully.";
                    return RedirectToAction(nameof(ClubsBackOffice));
                }
                
            }

            var existingClub = await _context.Club.FindAsync(id);
            viewModel.ClubBadges = existingClub.ClubBadge;
            return View(viewModel);
        }

        private bool ValidateUpdatedProperties(UpdateClubViewModel viewModel)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(viewModel.ClubName, new ValidationContext(viewModel, null, null) { MemberName = "ClubName" }, validationResults);
            Validator.TryValidateProperty(viewModel.ClubLocation, new ValidationContext(viewModel, null, null) { MemberName = "ClubLocation" }, validationResults);
            Validator.TryValidateProperty(viewModel.ClubSummary, new ValidationContext(viewModel, null, null) { MemberName = "ClubSummary" }, validationResults);
            return validationResults.Count == 0;
        }





        private bool ClubExists(int id)
        {
            return _context.Club.Any(e => e.ClubId == id);
        }


        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Club == null)
            {
                return NotFound();
            }

            var club = await _context.Club
                .FirstOrDefaultAsync(m => m.ClubId == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Club == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Club'  is null.");
            }
            var club = await _context.Club.FindAsync(id);
            if (club != null)
            {
                _context.Club.Remove(club);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
