using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Migrations;
using MyField.Models;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class LineUpsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;

        public LineUpsController(Ksans_SportsDbContext context,
              UserManager<UserBaseModel> userManager,
              IActivityLogger activityLogger)
        {
            _context = context;
            _userManager = userManager; 
            _activityLogger = activityLogger;
        }


        public async Task<IActionResult> HeadToHead(int homeClubId, int awayClubId)
        {
            var headtoheadStats = await _context.HeadToHead
                .Where(mo => (mo.HomeTeamId == homeClubId && mo.AwayTeamId == awayClubId && mo.ClubId == homeClubId) ||
                         (mo.HomeTeamId == awayClubId && mo.AwayTeamId == homeClubId && mo.ClubId == homeClubId))
                .Include(mo => mo.HomeTeam)
                .Include(mo => mo.AwayTeam)
                .ToListAsync();

            ViewBag.homeClubId = homeClubId;

            return PartialView("_FixtureHeadToHeadPartial", headtoheadStats);
        }


        public IActionResult MatchLineUpsFans(int fixtureId)
        {
            var matchLineUps = _context.LineUpXI
                .Where(mo => mo.FixtureId == fixtureId)
                .Include(s => s.Fixture)
                .FirstOrDefault();

            return PartialView("_MatchLineUpsFansPartial", matchLineUps);
        }

        public async Task<IActionResult> HomeTeamLineUp(int fixtureId, int clubId)
        {
            var awayLineUp = await _context.LineUpXI
                .Where(mo => mo.FixtureId == fixtureId && mo.ClubId == clubId)
                .Include(s => s.Fixture)
                .Include(s => s.Club)
                .Include(s => s.ClubPlayer)
                .ToListAsync();

            var homeClubName = _context.Fixture
                .Where(mo => mo.HomeTeam.ClubId==clubId)
                .Include(s => s.HomeTeam)
                .FirstOrDefault();

            var clubFormation = await _context.MatchFormation
                .Where(mo => mo.ClubId == clubId && mo.FixtureId == fixtureId)
                .Include(s => s.Club)
                .Include(s => s.Formation)
                .FirstOrDefaultAsync();

            ViewBag.HomeTeamName = homeClubName?.HomeTeam?.ClubName;


            if (clubFormation != null && clubFormation.Formation != null && clubFormation.Formation.FormationName != null)
            {
                ViewBag.ClubFormation = clubFormation.Formation.FormationName;
            }
            else
            {
                ViewBag.ClubFormation = "Formation not found";
            }




            return PartialView("_HomeTeamLineUp", awayLineUp);
        }


        public async Task<IActionResult> AwayTeamLineUp(int fixtureId, int clubId)
        {
            var awayLineUp = await _context.LineUpXI
                .Where(mo => mo.FixtureId == fixtureId && mo.ClubId == clubId)
                .Include(s => s.Club)
                .Include(s => s.ClubPlayer)
                .Include(s => s.Fixture)
                .ToListAsync();

            var awayClubName = _context.Fixture
                    .Where(mo => mo.AwayTeam.ClubId == clubId)
                    .Include(s => s.AwayTeam)
                    .FirstOrDefault();

            var clubFormation = await _context.MatchFormation
                .Where(mo => mo.ClubId == clubId && mo.FixtureId == fixtureId)
                .Include(s => s.Club)
                .Include(s => s.Formation)
                .FirstOrDefaultAsync();

            ViewBag.AwayTeamName = awayClubName?.AwayTeam?.ClubName;

            if (clubFormation != null && clubFormation.Formation != null && clubFormation.Formation.FormationName != null)
            {
                ViewBag.ClubFormation = clubFormation.Formation.FormationName;
            }
            else
            {
                ViewBag.ClubFormation = "Formation not found";
            }

            return PartialView("_AwayTeamLineUp",awayLineUp );
        }

        public async Task<IActionResult> HomeTeamSubstitutes(int fixtureId, int clubId)
        {
            var awayLineUp = await _context.LineUpSubstitutes
                .Where(mo => mo.FixtureId == fixtureId && mo.ClubId == clubId)
                .Include(s => s.Club)
                 .Include(s => s.Fixture)
                .Include(s => s.ClubPlayer)
                .ToListAsync();

            var homeClubName = _context.Fixture
                .Where(mo => mo.HomeTeam.ClubId == clubId)
                .Include(s => s.HomeTeam)
                .FirstOrDefault();

            ViewBag.HomeTeamName = homeClubName?.HomeTeam?.ClubName;


            return PartialView("_HomeTeamSubstitutes", awayLineUp);
        }


        public async Task<IActionResult> AwayTeamSubstitutes(int fixtureId, int clubId)
        {
            var awayLineUp = await _context.LineUpSubstitutes
                .Where(mo => mo.FixtureId == fixtureId && mo.ClubId == clubId)
                .Include(s => s.Club)
                .Include(s => s.ClubPlayer)
                .Include(s => s.Fixture)
                .ToListAsync();


            return PartialView("_AwayTeamSubstitutes", awayLineUp);
        }


        public async Task<IActionResult> LineUpXIFinal(int fixtureId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null || !(loggedInUser is ClubManager clubManager))
            {
                return RedirectToAction("Error", "Home");
            }

            var matchXI = await _context.LineUpXI
                .Where(mo => mo.FixtureId == fixtureId)
                .Include(s => s.Club)
                .Include(s => s.Fixture)
                .Include(s => s.ClubPlayer)
                .ToListAsync();

            return PartialView("_MatchLineUpFinalPartial", matchXI);
        }

        public async Task<IActionResult> LineUpSubstitutesFinal(int fixtureId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null || !(loggedInUser is ClubManager clubManager))
            {
                return RedirectToAction("Error", "Home");
            }

            var matchSubsXI = await _context.LineUpSubstitutes
                .Where(mo => mo.FixtureId == fixtureId)
                .Include(s => s.Club)
                .Include(s => s.Fixture)
                .Include(s => s.ClubPlayer)
                .ToListAsync();

            return PartialView("_MatchLineUpSubstitutesFinalPartial", matchSubsXI);
        }

        public IActionResult CreateMatchLineUp()
        {
            return PartialView("_CreateMatchLineUpPartial");
        }


        public async Task<IActionResult> PlayerMatchLineUp()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null || !(loggedInUser is ClubManager clubManager))
            {
                return RedirectToAction("Error", "Home");
            }

            var players = await _context.Player
                .Where(mo => mo.ClubId == clubManager.ClubId)
                .Include(s => s.Club)
                .ToListAsync();

            return PartialView("_MatchLineUpClubPlayersPartial", players);
        }



        public async Task<IActionResult> MatchXIHolder(int fixtureId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null || !(loggedInUser is ClubManager clubManager))
            {
                return RedirectToAction("Error", "Home");
            }

            var matchXI = await _context.LineUpXIHolder
                .Where(mo => mo.FixtureId == fixtureId)
                .Include(s => s.Club)
                .Include(s => s.Fixture)
                .Include(s => s.ClubPlayer)
                .ToListAsync();

            return PartialView("_MatchXIHolderPartial", matchXI);
        }


        public async Task<IActionResult> MatchSubstitutes(int fixtureId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            if (loggedInUser == null || !(loggedInUser is ClubManager clubManager))
            {
                return RedirectToAction("Error", "Home");
            }

            // Ensure that the FixtureId matches the parameter passed to the action
            var matchSubstitutes = await _context.LineUpSubstitutesHolder
                .Where(mo => mo.FixtureId == fixtureId) // Filter by fixtureId
                .Include(s => s.Club)
                .Include(s => s.Fixture)
                .Include(s => s.ClubPlayer)
                .ToListAsync();

            return PartialView("_MatchSubstitutesHolderPartial", matchSubstitutes);
        }





        public async Task<IActionResult> ClubPlayers()
        {
            var loggedInUser = await _userManager.GetUserAsync(User);
            var clubManager = loggedInUser as ClubManager;

            if (clubManager != null)
            {
                var clubId = clubManager.ClubId;

                var clubPlayers = _context.Player
                    .Where(p => p.ClubId == clubId && p.ClubId == clubManager.ClubId)
                    .Include (s => s.Club)
                    .ToList();

                return View(clubPlayers);
            }
            else
            {
                string errorMessage = "A club manager needs to be logged into the system to see a list of their players for creating a lineup.";

                return RedirectToAction("ErrorPage","Home", new { errorMessage = errorMessage });
            }
        }


        // GET: LineUps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LineUp == null)
            {
                return NotFound();
            }

            var lineUp = await _context.LineUp
                .Include(l => l.Club)
                .Include(l => l.CreatedBy)
                .Include(l => l.Fixture)
                .Include(l => l.ModifiedBy)
                .FirstOrDefaultAsync(m => m.LineUpId == id);
            if (lineUp == null)
            {
                return NotFound();
            }

            return View(lineUp);
        }
        // GET: CreateMatchLineUpXIHolder
        public IActionResult CreateMatchLineUpXIHolder(int fixtureId, string playerId)
        {
            var newViewModel = new MatchLineUpXIHolderViewModel
            {
                FixtureId = fixtureId,
                UserId = playerId
            };

            return View(newViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMatchLineUpXIHolder(MatchLineUpXIHolderViewModel viewModel)
        {
            try
            {
                var count = _context.LineUpXIHolder.Count(l => l.FixtureId == viewModel.FixtureId);
                if (count >= 11)
                {
                    TempData["XiMessage"] = "Starting XI is limited to 11 players per match";
                    TempData.Remove("SubstitutesMessage");
                    return Ok();
                }

                var existingLineup = await _context.LineUpXIHolder
                    .FirstOrDefaultAsync(l => l.FixtureId == viewModel.FixtureId && l.PlayerId == viewModel.UserId);

                if (existingLineup != null)
                {
                    TempData["XiMessage"] = "This player has already been added to starting XI";
                    TempData.Remove("SubstitutesMessage");
                    return Ok();
                }

                var existingSubstitutes = await _context.LineUpSubstitutesHolder
                    .FirstOrDefaultAsync(l => l.FixtureId == viewModel.FixtureId && l.PlayerId == viewModel.UserId);

                if (existingSubstitutes != null)
                {
                    TempData["SubstitutesMessage"] = "This player has already been added to substitutes";
                    TempData.Remove("XiMessage");
                    return Ok();
                }

                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var userId = user.Id;

                    var loggedInUser = await _userManager.GetUserAsync(User);
                    var clubManager = loggedInUser as ClubManager;

                    var player = await _context.Player.FindAsync(viewModel.UserId);

                    if (player != null)
                    {
                        var newLineUpXI = new LineUpXIHolder
                        {
                            FixtureId = viewModel.FixtureId,
                            PlayerId = viewModel.UserId,
                            ClubId = clubManager?.ClubId ?? 0,
                            CreatedById = userId,
                            ModifiedById = userId,
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now
                        };

                        _context.LineUpXIHolder.Add(newLineUpXI);
                        await _context.SaveChangesAsync();

                        TempData["XiMessage"] = $"{player.FirstName} {player.LastName} ({player.Position}) added to starting XI successfully.";
                        TempData.Remove("SubstitutesMessage");
                        return Ok();
                    }
                    else
                    {
                        TempData["XiMessage"] = "Player not found.";
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a new MatchLineUpXIHolder: {ex.Message}");
                return View("Index");
            }
        }




        // GET: CreateMatchLineUpSubstitutesHolder
        public IActionResult CreateMatchLineUpSubstitutesHolder(int fixtureId, string playerId)
        {
            var newViewModel = new MatchLineUpSubstitutesViewModel
            {
                FixtureId = fixtureId,
                UserId = playerId,
            };
            return View(newViewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMatchLineUpSubstitutesHolder(MatchLineUpSubstitutesViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userManager.GetUserAsync(User);
                    var userId = user.Id;

                    var count = _context.LineUpSubstitutesHolder.Count(l => l.FixtureId == viewModel.FixtureId);
                    if (count >= 7)
                    {
                        TempData["SubstitutesMessage"] = "Substitutes are limited to only 7 players per match!";
                        TempData.Remove("XiMessage");
                        return Ok();
                    }

                    var existingLineup = await _context.LineUpXIHolder
                        .FirstOrDefaultAsync(l => l.FixtureId == viewModel.FixtureId && l.PlayerId == viewModel.UserId);

                    if (existingLineup != null)
                    {
                        TempData["XiMessage"] = "This player has already been added to starting XI";
                        TempData.Remove("SubstitutesMessage");
                        return Ok();
                    }

                    var existingSubstitutes = await _context.LineUpSubstitutesHolder
                        .FirstOrDefaultAsync(l => l.FixtureId == viewModel.FixtureId && l.PlayerId == viewModel.UserId);

                    if (existingSubstitutes != null)
                    {
                        TempData["SubstitutesMessage"] = "This player has already been added to substitutes";
                        TempData.Remove("XiMessage");
                        return Ok();
                    }

                    var loggedInUser = await _userManager.GetUserAsync(User);
                    var clubManager = loggedInUser as ClubManager;

                    var player = await _context.Player.FindAsync(viewModel.UserId);

                    if (player != null)
                    {
                        var newLineUpSubstitute = new LineUpSubstitutesHolder
                        {
                            FixtureId = viewModel.FixtureId,
                            PlayerId = viewModel.UserId,
                            ClubId = clubManager?.ClubId ?? 0,
                            CreatedById = userId,
                            ModifiedById = userId,
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                        };

                        _context.LineUpSubstitutesHolder.Add(newLineUpSubstitute);
                        await _context.SaveChangesAsync();

                        TempData["SubstitutesMessage"] = $"{player.FirstName} {player.LastName} ({player.Position}) added to substitutes successfully.";
                        TempData.Remove("XiMessage");
                        return Ok();
                    }
                    else
                    {
                        TempData["SubstitutesMessage"] = "Player not found.";
                        return Ok();
                    }
                }
                else
                {
                    return BadRequest(ModelState);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while creating a new MatchLineUpSubstitutesHolder: {ex.Message}");
                return View("Error");
            }
        }



        // GET: CreateMatchLineUpSubstitutesHolder/
        public IActionResult CreateMatchLineUpFinal(int fixtureId)
        {
            var viewModel = new  MatchLineUpFinalViewModel
                {
                   FixtureId= fixtureId,
                }; 

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMatchLineUpFinal(MatchLineUpFinalViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errorMessages = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return Json(new { success = false, error = string.Join(", ", errorMessages) });
                }

                var user = await _userManager.GetUserAsync(User);
                var userId = user?.Id;


                var loggedInUser = await _userManager.GetUserAsync(User);
                var clubManager = loggedInUser as ClubManager;

                if (string.IsNullOrEmpty(userId))
                {
                    // Handle the error, for example:
                    return Json(new { success = false, error = "User ID is null or empty" });
                }

                // Find lineupXIHolders and lineupSubstitutesHolders
                var lineupXIHolders = await _context.LineUpXIHolder.ToListAsync();
                Console.WriteLine("lineupXIHolders: " + lineupXIHolders); // Log lineupXIHolders
                var lineupSubstitutesHolders = await _context.LineUpSubstitutesHolder.ToListAsync();
                Console.WriteLine("lineupSubstitutesHolders: " + lineupSubstitutesHolders); // Log lineupSubstitutesHolders

                // Create a new LineUp object
                var lineUp = new LineUp
                {
                    ClubId = clubManager.ClubId,
                    FixtureId = viewModel.FixtureId,
                    CreatedById = userId,
                    ModifiedById = userId,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    LineUpXI = lineupXIHolders.Select(lineupXIHolder => new LineUpXI
                    {
                        FixtureId = lineupXIHolder.FixtureId,
                        PlayerId = lineupXIHolder.PlayerId,
                        ClubId = lineupXIHolder.ClubId,
                        CreatedById = userId,
                        CreatedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                        ModifiedById = userId
                    }).ToList(),

                    LineUpSubstitutes = lineupSubstitutesHolders.Select(lineupSubstitutesHolder => new LineUpSubstitutes
                    {
                        FixtureId = lineupSubstitutesHolder.FixtureId,
                        PlayerId = lineupSubstitutesHolder.PlayerId,
                        ClubId = lineupSubstitutesHolder.ClubId,
                        CreatedById = userId,
                        CreatedDateTime = DateTime.Now,
                        ModifiedDateTime = DateTime.Now,
                        ModifiedById = userId
                    }).ToList()
                };


                _context.Add(lineUp);
                await _context.SaveChangesAsync();

                _context.LineUpXIHolder.RemoveRange(lineupXIHolders);
                _context.LineUpSubstitutesHolder.RemoveRange(lineupSubstitutesHolders);
                await _context.SaveChangesAsync();

                var fixture = await _context.Fixture
                    .Where(f => f.FixtureId == viewModel.FixtureId)
                    .Include(f => f.HomeTeam)
                    .Include(f => f.AwayTeam)
                    .FirstOrDefaultAsync();

                await _activityLogger.Log($"Created match lineup for match of {fixture.HomeTeam.ClubName} and {fixture.AwayTeam.ClubName}", user.Id);

                return Ok();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("An error occurred while creating the match lineup.");
                Console.WriteLine("Error message: " + ex.Message);

                // Log inner exception details if available
                if (ex.InnerException != null)
                {
                    Console.WriteLine("Inner exception: " + ex.InnerException.Message);
                    Console.WriteLine("Inner exception stack trace: " + ex.InnerException.StackTrace);

                    // Return the inner exception details in the JSON error response
                    return Json(new { success = false, error = ex.Message, innerError = ex.InnerException.Message, innerStackTrace = ex.InnerException.StackTrace });
                }
                else
                {
                    return Json(new { success = false, error = ex.Message });
                }
            }

        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlayerFromLineUpXIHolder(int fixtureId, string playerId)
        {
            try
            {
                var lineUpXIHolder = await _context.LineUpXIHolder
                    .Include(s => s.ClubPlayer)
                    .FirstOrDefaultAsync(m => m.FixtureId == fixtureId && m.PlayerId == playerId);

                if (lineUpXIHolder != null)
                {
                    var playerFirstName = lineUpXIHolder.ClubPlayer.FirstName;
                    var playerLastName = lineUpXIHolder.ClubPlayer.LastName;
                    var playerPosition = lineUpXIHolder.ClubPlayer.Position;

                    _context.LineUpXIHolder.Remove(lineUpXIHolder);
                    await _context.SaveChangesAsync();

                    TempData["XiMessage"] = $"You have deleted {playerFirstName} {playerLastName} ({playerPosition}) from the lineup.";
                    TempData.Remove("SubstitutesMessage");
                }
                else
                {
                    TempData["XiMessage"] = "Player not found in the starting lineup.";
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the player from LineUpXIHolder: {ex.Message}");

                return View("Error");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePlayerFromLineUpSubstitutesHolder(int fixtureId, string playerId)
        {
            try
            {
                var lineUpSubstitutesHolder = await _context.LineUpSubstitutesHolder
                    .Include(s => s.ClubPlayer)
                    .FirstOrDefaultAsync(m => m.FixtureId == fixtureId && m.PlayerId == playerId);

                if (lineUpSubstitutesHolder != null)
                {
                    var playerFirstName = lineUpSubstitutesHolder.ClubPlayer.FirstName;
                    var playerLastName = lineUpSubstitutesHolder.ClubPlayer.LastName;
                    var playerPosition = lineUpSubstitutesHolder.ClubPlayer.Position;

                    _context.LineUpSubstitutesHolder.Remove(lineUpSubstitutesHolder);
                    await _context.SaveChangesAsync();

                    TempData["SubstitutesMessage"] = $"You have deleted {playerFirstName} {playerLastName} ({playerPosition}) from the substitutes.";
                    TempData.Remove("XiMessage");
                }
                else
                {
                    TempData["SubstitutesMessage"] = "Player not found in the substitutes.";
                }

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the player from LineUpSubstitutesHolder: {ex.Message}");

                return View("Error");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MovePlayerFromXItoSubstitutes(int fixtureId, string playerId, int clubId)
        {
            try
            {
                var count = await _context.LineUpSubstitutesHolder.CountAsync(l => l.FixtureId == fixtureId);
                if (count >= 7)
                {
                    TempData["XiMessage"] = "Please delete one or more players from substitutes to move a player";
                    TempData.Remove("SubstitutesMessage");
                    return Ok();
                }
                

                if (ModelState.IsValid)
                {

                    var player = await _context.LineUpXIHolder
                        .Include(p => p.ClubPlayer)
                        .FirstOrDefaultAsync(m => m.FixtureId == fixtureId && m.PlayerId == playerId);
                    var user = await _userManager.GetUserAsync(User);
                    var userId = user?.Id;

                    if (player != null)
                    {

                        _context.LineUpXIHolder.Remove(player);

                        _context.LineUpSubstitutesHolder.Add(new LineUpSubstitutesHolder
                        {
                            ClubId = clubId,
                            FixtureId = fixtureId,
                            PlayerId = playerId,
                            CreatedById = userId,
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            ModifiedById = userId
                        });

                        await _context.SaveChangesAsync();
                    }

                    TempData["XiMessage"] = $"You have successfully moved  {player.ClubPlayer.FirstName}  {player.ClubPlayer.LastName} ({player.ClubPlayer.Position}) to substitutes.";
                    TempData.Remove("SubstitutesMessage");
                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the player from LineUpXIHolder: {ex.Message}");

                return View("Error");
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MovePlayerFromSubstitutesToXI(int fixtureId, string playerId, int clubId)
        {
            try
            {
                // Check if the maximum number of LineUpXIHolder entries has been reached
                var count = await _context.LineUpXIHolder.CountAsync(l => l.FixtureId == fixtureId);
                if (count >= 11)
                {
                    TempData["SubstitutesMessage"] = "Please delete one or more players from starting XI to move a player";
                    TempData.Remove("XiMessage");
                    return Ok();
                }

                if (ModelState.IsValid)
                {
                    // Find the player in LineUpSubstitutesHolder
                    var player = await _context.LineUpSubstitutesHolder
                        .Include(s => s.ClubPlayer)
                        .FirstOrDefaultAsync(m => m.FixtureId == fixtureId && m.PlayerId == playerId);
                    var user = await _userManager.GetUserAsync(User);
                    var userId = user?.Id;

                    if (player != null)
                    {
                        // Remove the player from LineUpSubstitutesHolder
                        _context.LineUpSubstitutesHolder.Remove(player);

                        // Add the player to LineUpXIHolder
                        _context.LineUpXIHolder.Add(new LineUpXIHolder
                        {
                            ClubId = clubId,
                            FixtureId = fixtureId,
                            PlayerId = playerId,
                            CreatedById = userId,
                            CreatedDateTime = DateTime.Now,
                            ModifiedDateTime = DateTime.Now,
                            ModifiedById = userId
                        });

                        await _context.SaveChangesAsync();
                    }


                    TempData["SubstitutesMessage"] = $" You have successfully moved {player.ClubPlayer.FirstName}  {player.ClubPlayer.LastName} ({player.ClubPlayer.Position}) to starting XI.";
                    TempData.Remove("XiMessage");
                    return Ok();
                }
                else
                {
                    return BadRequest(ModelState); // Return a bad request response with model state errors
                }
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while moving the player from LineUpSubstitutesHolder to LineUpXIHolder: {ex.Message}");

                // Return a generic error view
                return View("Error");
            }
        }




        private bool LineUpExists(int id)
        {
          return (_context.LineUp?.Any(e => e.LineUpId == id)).GetValueOrDefault();
        }
    }
}
