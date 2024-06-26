using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class FormationsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;

        public FormationsController(Ksans_SportsDbContext context,
               FileUploadService fileUploadService,
               UserManager<UserBaseModel> userManager,
               IActivityLogger activityLogger)
        {
            _fileUploadService = fileUploadService;
            _userManager = userManager;
            _context = context;
            _activityLogger = activityLogger;   
        }


        public async Task<IActionResult> CreateMatchFormation()
        {
            var matchFormation = await _context.Formations
                    .Include(s => s.CreatedBy)
                    .Include(s => s.ModifiedBy)
                    .FirstOrDefaultAsync();

            ViewBag.Formations = await _context.Formations.ToListAsync();

            return PartialView("_CreateMatchFormationPartial", matchFormation);
        }

        public async Task<IActionResult> MatchFormationFind(int fixtureId)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                // Ensure user is authenticated and is a ClubManager
                if (user == null || !(user is ClubManager clubManager))
                {
                    return RedirectToAction("Error", "Home");
                }

                // Retrieve match formations for the specified fixture and club
                var matchFormations = await _context.MatchFormation
                    .Where(mo => mo.FixtureId == fixtureId)
                    .Include(s => s.Fixture)
                    .Include(s => s.Formation)
                    .ToListAsync();

                // Return the match formations as a partial view
                return PartialView("_ClubFormationPartial", matchFormations);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error in MatchFormation action: " + ex.Message);

                // Return a specific error response with details about the error
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        // GET: Formations
        public async Task<IActionResult> Index()
        {
              return _context.Formations != null ? 
                          View(await _context.Formations.ToListAsync()) :
                          Problem("Entity set 'Ksans_SportsDbContext.Formations'  is null.");
        }

        // GET: Formations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Formations == null)
            {
                return NotFound();
            }

            var formation = await _context.Formations
                .FirstOrDefaultAsync(m => m.FormationId == id);
            if (formation == null)
            {
                return NotFound();
            }

            return View(formation);
        }


        [HttpGet]
        public IActionResult CreateMatchFormationFinal(int fixtureId, int formationId)
        {
            var viewModel = new MatchFormationFinalViewModel
            {
                FixtureId = fixtureId,
                FormationId = formationId
            };

            return View(viewModel);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMatchFormationFinal(MatchFormationFinalViewModel viewModel)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                // Ensure the user is authenticated and is a ClubManager
                if (user == null || !(user is ClubManager clubManager))
                {
                    return RedirectToAction("Error", "Home");
                }

                // Get the fixture
                var fixture = await _context.Fixture.FindAsync(viewModel.FixtureId);

                if (!(fixture.HomeTeamId == clubManager.ClubId || fixture.AwayTeamId == clubManager.ClubId))
                {
                    TempData["Message"] = "You can't set formation for the club you are not related to!";
                    return Ok(viewModel);
                }

                var existingFormation = _context.MatchFormation
                    .Include( e=> e.Formation)
                    .FirstOrDefault(x => x.FixtureId == viewModel.FixtureId && x.ClubId == clubManager.ClubId);

                if (existingFormation != null)
                {
                    TempData["Message"] = "You can set formation once per match!";
                    return Ok(existingFormation);
                }

                if (ModelState.IsValid)
                {
                    var newMatchFormation = new MatchFormation
                    {
                        FixtureId = viewModel.FixtureId,
                        ClubId = clubManager.ClubId,
                        FormationId = viewModel.FormationId,
                        CreatedById = userId,
                        CreatedDateTime = DateTime.UtcNow,
                        ModifiedById = userId,
                        ModifiedDateTime = DateTime.UtcNow,
                    };

                    _context.Add(newMatchFormation);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    // Return a bad request response with model state errors
                    return BadRequest(ModelState);
                }

                var existingFixture = await _context.Fixture
                                    .Where(f => f.FixtureId == viewModel.FixtureId)
                                    .Include(f => f.HomeTeam)
                                    .Include(f => f.AwayTeam)
                                    .FirstOrDefaultAsync();

                TempData["Message"] = "Formation set successfully!";
                await _activityLogger.Log($"Setted formation between {fixture.HomeTeam.ClubName} and {fixture.AwayTeam.ClubName}", user.Id);
                return Ok();

            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while creating a new formation: {ex.Message}");

                // Return a specific error response with details about the error
                return StatusCode(500, "An error occurred while processing the request.");
            }
        }


        private bool FormationExists(int id)
        {
          return (_context.Formations?.Any(e => e.FormationId == id)).GetValueOrDefault();
        }
    }
}
