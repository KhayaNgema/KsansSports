using System;
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
    public class FixturesController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IActivityLogger _activityLogger;

        public FixturesController(Ksans_SportsDbContext context, 
            UserManager<UserBaseModel> userManager,
            RoleManager<IdentityRole> roleManager,
            IActivityLogger activityLogger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _activityLogger = activityLogger;
        }

        public async Task<IActionResult> FixtureMatchOffcials(int fixtureId)
        {

            var fixtureOfficials = await _context.MatchOfficials
                .Where(mo => mo.FixtureId == fixtureId)
                .Include(s => s.Fixture)
                .Include(s => s.Refeere)
                .Include(s => s.AssistantOne)
                .Include(s => s.AssistantTwo)
                .ToListAsync();
            return PartialView("_FixtureMatchOfficialsPartial", fixtureOfficials);
        }


        // GET: FixturesBackOffice
        public async Task<IActionResult> FixtureLineUpUpdate()
        {
            var user = await _userManager.Users
                .Include(u => (u as ClubManager).Club)
                .Include(u => (u as ClubAdministrator).Club)
                .Include(u => (u as Player).Club)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
                return View(); // Assuming there's a view to show the error.
            }

            ClubManager clubManager = user as ClubManager;
            ClubAdministrator clubAdministrator = user as ClubAdministrator;
            Player player = user as Player;

            if (clubManager == null && clubAdministrator == null && player == null)
            {
                return RedirectToAction("Error", "Home");
            }

            int clubId = 0;
            string clubName = string.Empty;

            if (clubManager != null)
            {
                clubId = clubManager.ClubId;
                clubName = clubManager.Club?.ClubName ?? "Unknown Club";
            }
            else if (clubAdministrator != null)
            {
                clubId = clubAdministrator.ClubId;
                clubName = clubAdministrator.Club?.ClubName ?? "Unknown Club";
            }
            else if (player != null)
            {
                clubId = player.ClubId;
                clubName = player.Club?.ClubName ?? "Unknown Club";
            }

            ViewBag.ClubName = clubName;

            var fixtures = await _context.Fixture
                .Where(f => (f.HomeTeam.ClubId == clubId || f.AwayTeam.ClubId == clubId) &&
                            (f.FixtureStatus == FixtureStatus.Upcoming ||
                             f.FixtureStatus == FixtureStatus.Postponed ||
                             f.FixtureStatus == FixtureStatus.Interrupted) &&
                             f.LeagueId == currentLeague.LeagueId)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .Include(f => f.CreatedBy)
                .Include(f => f.ModifiedBy)
                .ToListAsync();

            return View(fixtures);
        }






        //GET : FixtureDetailsFans
        public async Task<IActionResult> FixtureDetailsFans(int? FixtureId)
        {
            if (FixtureId == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixture
                .Where(f => f.FixtureStatus == FixtureStatus.Upcoming ||
                            f.FixtureStatus == FixtureStatus.Postponed ||
                            f.FixtureStatus == FixtureStatus.Interrupted)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync(m => m.FixtureId == FixtureId);

            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }


        public async Task<IActionResult> FixtureDetails(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixture
                .Where(f => f.FixtureStatus == FixtureStatus.Upcoming ||
                            f.FixtureStatus == FixtureStatus.Postponed ||
                            f.FixtureStatus == FixtureStatus.Interrupted)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync(m => m.FixtureId == id);

            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        public async Task<IActionResult> FixtureDetailsBackOffice(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixture
                .Where(f => f.FixtureStatus == FixtureStatus.Upcoming ||
                            f.FixtureStatus == FixtureStatus.Postponed ||
                            f.FixtureStatus == FixtureStatus.Interrupted)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync(m => m.FixtureId == id);

            var matchOfficials = await _context.MatchOfficials
                .Where(m => m.FixtureId == id)
                .Include(m => m.AssistantOne)
                .Include(m => m.AssistantTwo)
                .Include(m => m.Refeere)
                .FirstOrDefaultAsync();

            var newfixture = new FixtureDetailsBackOfficeViewModel
            {
                HomeTeamName = fixture.HomeTeam.ClubName,
                AwayTeamName = fixture.AwayTeam.ClubName,
                HomeTeamBadge = fixture.HomeTeam.ClubBadge,
                AwayTeamBadge = fixture.AwayTeam.ClubBadge,
                KickOffDate  = fixture.KickOffDate,
                KickOffTime = fixture.KickOffTime,
                StadiumName = fixture.Location,
                RefereeName = $"{matchOfficials.Refeere.FirstName} {matchOfficials.Refeere.LastName}",
                AssistantOneName = $"{matchOfficials.AssistantOne.FirstName} {matchOfficials.AssistantOne.LastName}",
                AssistantTwoName = $"{matchOfficials.AssistantTwo.FirstName} {matchOfficials.AssistantTwo.LastName}",
            };

            if (fixture == null)
            {
                return NotFound();
            }

            return View(newfixture);
        }




        // GET: FixturesBackOffice
        public async Task<IActionResult> FixturesBackOffice()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }

            var fixtures = await _context.Fixture
                .Where(f => f.FixtureStatus == FixtureStatus.Upcoming ||
                            f.FixtureStatus == FixtureStatus.Postponed ||
                            f.FixtureStatus == FixtureStatus.Interrupted ||
                            f.FixtureStatus == FixtureStatus.Ended &&
                            f.LeagueId == currentLeague.LeagueId)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .Include (f => f.CreatedBy)
                .Include(f => f.ModifiedBy)
                .OrderByDescending(f => f.CreatedDateTime)
                .ToListAsync();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;

            return View(fixtures);
        }

        // GET: FixturesBackOffice
        public async Task<IActionResult> FixturesBackOfficeUsers()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }

            var fixtures = await _context.Fixture
                .Where(f => f.FixtureStatus == FixtureStatus.Upcoming ||
                            f.FixtureStatus == FixtureStatus.Postponed ||
                            f.FixtureStatus == FixtureStatus.Interrupted &&
                            f.LeagueId == currentLeague.LeagueId)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .Include(f => f.CreatedBy)
                .Include(f => f.ModifiedBy)
                .ToListAsync();

            return View(fixtures);
        }


        // GET: Fixtures
        public async Task<IActionResult> Fixtures()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }


            var upcomingFixtures = await _context.Fixture
                .Where(f => (f.FixtureStatus == FixtureStatus.Upcoming) &&
                            f.LeagueId == currentLeague.LeagueId)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .Include(f => f.MatchOfficials)
                .ToListAsync();

            return View(upcomingFixtures);
        }

        // GET: Fixtures/Index
        public async Task<IActionResult> Index()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }

            var upcomingFixtures = await _context.Fixture
                .Where(f => (f.FixtureStatus == FixtureStatus.Upcoming ||
                             f.FixtureStatus == FixtureStatus.Postponed ||
                             f.FixtureStatus == FixtureStatus.Interrupted) &&
                            f.LeagueId == currentLeague.LeagueId)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .Include(f => f.MatchOfficials)
                .ToListAsync();

            foreach (var fixture in upcomingFixtures)
            {
                var matchReferee = await _context.MatchOfficials
                    .Where(mo => mo.FixtureId == fixture.FixtureId)
                    .Include(s => s.Refeere)
                    .FirstOrDefaultAsync();

                if (matchReferee != null && matchReferee.Refeere != null)
                {
                    ViewBag.MatchReferee = matchReferee.Refeere.FirstName + " " + matchReferee.Refeere.LastName;
                }
                else
                {
                    ViewBag.MatchReferee = "Referee information not found";
                }
            }

            return PartialView("_FixturesPartial", upcomingFixtures);
        }


        public async Task<IActionResult> BackOfficeFixtures()
        {
            var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

            if (currentLeague == null)
            {
                ModelState.AddModelError(string.Empty, "No current league found.");
            }

            var upcomingFixtures = await _context.Fixture
                .Where(f => f.FixtureStatus == FixtureStatus.Upcoming ||
                            f.FixtureStatus == FixtureStatus.Postponed ||
                            f.FixtureStatus == FixtureStatus.Interrupted &&
                            f.LeagueId == currentLeague.LeagueId)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .Include(f => f.CreatedBy)
                .Include(f => f.ModifiedBy)
                .ToListAsync();

            foreach (var fixture in upcomingFixtures)
            {
                var matchReferee = await _context.MatchOfficials
                    .Where(mo => mo.FixtureId == fixture.FixtureId)
                    .Include(s => s.Refeere)
                    .FirstOrDefaultAsync();

                if (matchReferee != null && matchReferee.Refeere != null)
                {
                    ViewBag.MatchReferee = matchReferee.Refeere.FirstName + " " + matchReferee.Refeere.LastName;
                }
                else
                {
                    ViewBag.MatchReferee = "Referee information not found";
                }
            }

            return PartialView("_BackOfficeFixturesPartial", upcomingFixtures);
        }



        // GET: Fixtures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixture
                .Where(f => f.FixtureStatus == FixtureStatus.Upcoming ||
                            f.FixtureStatus == FixtureStatus.Postponed ||
                            f.FixtureStatus == FixtureStatus.Interrupted)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync(m => m.FixtureId == id);

            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        // GET: Fixtures/Create
        public async Task<IActionResult> Create()
        {
            var officials = await _context.Officials.ToListAsync();

            ViewBag.Officials = new SelectList(officials.Select(o => new { UserId = o.Id, FullName = o.FirstName + " " + o.LastName }), "UserId", "FullName");

            ViewBag.Clubs = new SelectList(await _context.Club.Where(mo => mo.League.IsCurrent).ToListAsync(), "ClubId", "ClubName");
            return View(new FixtureViewModel());
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(FixtureViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var currentLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

                var matchReport = await _context.MatchReports
                     .Where(m => m.Season.IsCurrent)
                     .Include(m => m.Season)
                     .FirstOrDefaultAsync();

                if (currentLeague == null)
                {
                    ModelState.AddModelError(string.Empty, "No current league found.");
                    return View(viewModel);
                }

                var newFixture = new Fixture
                {
                    LeagueId = currentLeague.LeagueId,
                    FixtureId = viewModel.FixtureId,
                    HomeTeamId = viewModel.HomeTeamId,
                    AwayTeamId = viewModel.AwayTeamId,
                    KickOffDate = viewModel.KickOffDate,
                    KickOffTime = viewModel.KickOffTime,
                    Location = viewModel.Location,
                    CreatedById = userId,
                    ModifiedById = userId,
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    FixtureStatus = FixtureStatus.Upcoming
                };

                _context.Add(newFixture);
                await _context.SaveChangesAsync();

                var newOfficials = new MatchOfficials
                {
                    FixtureId = newFixture.FixtureId,
                    RefeereId = viewModel.Refeere, 
                    AssistantOneId = viewModel.AssistantOne,  
                    AssistantTwoId = viewModel.AssistantTwo,  
                };

                _context.Add(newOfficials);
                await _context.SaveChangesAsync();

                matchReport.FixturedMatchesCount++;

                await _context.SaveChangesAsync();

                var newSavedFixture = await _context.Fixture
                    .Where(f => f.Equals(newFixture))
                    .Include(f => f.HomeTeam)
                    .Include(f => f.AwayTeam)
                    .FirstOrDefaultAsync();

                await _activityLogger.Log($"Created a new fixture between {newSavedFixture.HomeTeam.ClubName} and {newSavedFixture.AwayTeam.ClubName}", user.Id);
                TempData["Message"] = $"You have successfully created a new fixture between {newSavedFixture.HomeTeam.ClubName} and {newSavedFixture.AwayTeam.ClubName} that will kickoff at {newSavedFixture.KickOffTime} on {newSavedFixture.KickOffDate.ToString("dddd, dd MMM yyyy")} at  {newSavedFixture.Location}.";
                return RedirectToAction(nameof(FixturesBackOffice));
            }

            var officials = await _context.Officials.ToListAsync();

            ViewBag.Officials = new SelectList(officials.Select(o => new { UserId = o.Id, FullName = o.FirstName + " " + o.LastName }), "UserId", "FullName");


            ViewBag.Clubs = new SelectList(await _context.Club.Where(mo => mo.League.IsCurrent).ToListAsync(), "ClubId", "ClubName");
            return View(viewModel);
        }



        // GET: Fixtures/Edit/5
        public async Task<IActionResult> ModifyFixture(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixture
                .Where(f => f.FixtureStatus == FixtureStatus.Upcoming ||
                            f.FixtureStatus == FixtureStatus.Postponed ||
                            f.FixtureStatus == FixtureStatus.Interrupted)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync(f => f.FixtureId == id);

            var matchOfficial = await _context.MatchOfficials
                .Where(m => m.FixtureId == id)
                .Include(m => m.Refeere)
                .Include(m => m.AssistantOne)
                .Include(m => m.AssistantTwo)
                .FirstOrDefaultAsync();

            if (fixture == null)
            {
                return NotFound();
            }

            var viewModel = new ModifyFixtureViewModel
            {

                FixtureId = fixture.FixtureId,
                HomeTeamId = fixture.HomeTeamId,
                AwayTeamId = fixture.AwayTeamId,
                KickOffDate = fixture.KickOffDate,
                KickOffTime = fixture.KickOffTime,
                Stadium = fixture.Location,
                FixtureStatus = fixture.FixtureStatus,
                RefereeId = matchOfficial.RefeereId,
                AssistantOneId = matchOfficial.AssistantOneId,
                AssistantTwoId = matchOfficial.AssistantTwoId

            };

                ViewBag.Clubs = new SelectList(_context.Club, "ClubId", "ClubName");
            ViewBag.FixtureStatusOptions = Enum.GetValues(typeof(FixtureStatus))
                                              .Cast<FixtureStatus>()
                                              .Select(v => new SelectListItem
                                              {
                                                  Text = v.ToString(),
                                                  Value = v.ToString()
                                              });

            var officials = await _context.Officials.ToListAsync();

            ViewBag.Officials = new SelectList(officials.Select(o => new { UserId = o.Id, FullName = o.FirstName + " " + o.LastName }), "UserId", "FullName");

            return View(viewModel);
        }


        // POST: Fixtures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ModifyFixture(int id, ModifyFixtureViewModel viewModel)
        {
            if (id != viewModel.FixtureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var existingFixture = await _context.Fixture
                    .Include(f => f.HomeTeam)
                    .Include(f => f.AwayTeam)
                    .FirstOrDefaultAsync(f => f.FixtureId == id);

                try
                {

                    var existingOfficial = await _context.MatchOfficials
                        .Where(e => e.FixtureId == id)
                        .Include( e => e.Refeere)
                        .Include(e => e.AssistantOne)
                        .Include(e => e.AssistantTwo)
                        .FirstOrDefaultAsync();

                    if (existingFixture == null)
                    {
                        return NotFound();
                    }

                    existingFixture.HomeTeamId = viewModel.HomeTeamId;
                    existingFixture.AwayTeamId = viewModel.AwayTeamId;
                    existingFixture.KickOffTime = viewModel.KickOffTime;
                    existingFixture.KickOffDate = viewModel.KickOffDate;
                    existingFixture.Location = viewModel.Stadium;
                    existingFixture.FixtureStatus = viewModel.FixtureStatus;
                    existingFixture.ModifiedDateTime = DateTime.Now;
                    existingFixture.ModifiedById = userId;

                    existingOfficial.RefeereId = viewModel.RefereeId;
                    existingOfficial.AssistantOneId = viewModel.AssistantOneId;
                    existingOfficial.AssistantTwoId = viewModel.AssistantTwoId;

                    _context.Update(existingFixture);
                    _context.Update(existingOfficial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FixtureExists(viewModel.FixtureId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                await _activityLogger.Log($"Modified a fixture between {existingFixture.HomeTeam.ClubName} and {existingFixture.AwayTeam.ClubName} ", user.Id);
                TempData["Message"] = $"You have successfully updated the fixture between {existingFixture.HomeTeam.ClubName} and {existingFixture.AwayTeam.ClubName}";
                return RedirectToAction(nameof(FixturesBackOffice));
            }

            ViewBag.Clubs = new SelectList(_context.Club, "ClubId", "ClubName");
            ViewBag.FixtureStatusOptions = Enum.GetValues(typeof(FixtureStatus))
                                              .Cast<FixtureStatus>()
                                              .Select(v => new SelectListItem
                                              {
                                                  Text = v.ToString(),
                                                  Value = v.ToString()
                                              });

            var officials = await _context.Officials.ToListAsync();

            ViewBag.Officials = new SelectList(officials.Select(o => new { UserId = o.Id, FullName = o.FirstName + " " + o.LastName }), "UserId", "FullName");

            return View(viewModel);
        }

        public async Task<IActionResult> InterruptFixture(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var matchReport = await _context.MatchReports
                 .Where(m => m.Season.IsCurrent)
                 .Include(m => m.Season)
                  .FirstOrDefaultAsync();

            var fixture = await _context.Fixture
                .Where(f => f.FixtureId == id)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync();

            fixture.FixtureStatus = FixtureStatus.Interrupted;
            fixture.ModifiedDateTime = DateTime.Now;
            fixture.ModifiedById = user.Id;

            _context.Update(fixture);

            matchReport.InterruptedMatchesCount++;
          
            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Interrupted a fixture between {fixture.HomeTeam.ClubName} and {fixture.AwayTeam.ClubName} ", user.Id);
            TempData["Message"] = $"You have successfully interrupted a match between {fixture.HomeTeam.ClubName} and {fixture.AwayTeam.ClubName}";

            return RedirectToAction(nameof(FixturesBackOffice));
        }

        public async Task<IActionResult> PostponeFixture(int id)
        {
            var user = await _userManager.GetUserAsync(User);

            var matchReport = await _context.MatchReports
                .Where(m => m.Season.IsCurrent)
                .Include(m => m.Season)
                .FirstOrDefaultAsync();

            var fixture = await _context.Fixture
                .Where(f => f.FixtureId == id)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync();

            fixture.FixtureStatus = FixtureStatus.Postponed;
            fixture.ModifiedDateTime = DateTime.Now;
            fixture.ModifiedById = user.Id;
            _context.Update(fixture);

            matchReport.PostponedMatchesRate++;

            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Postponed a fixture between {fixture.HomeTeam.ClubName} and {fixture.AwayTeam.ClubName} ", user.Id);

            TempData["Message"] = $"You have successfully posteponed a fixture between {fixture.HomeTeam.ClubName} and {fixture.AwayTeam.ClubName}";
            return RedirectToAction(nameof(FixturesBackOffice));
        }

/*        public async Task<IActionResult> Cancel(int id)
        {
            var fixture = await _context.Fixture
                .Where(f => f.FixtureId == id)
                .FirstOrDefaultAsync();

            fixture.FixtureStatus = FixtureStatus.Ca;

            _context.Update(fixture);
            await _context.SaveChangesAsync();

            return Ok();
        }*/



        // GET: Fixtures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Fixture == null)
            {
                return NotFound();
            }

            var fixture = await _context.Fixture
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync(m => m.FixtureId == id);

            if (fixture == null)
            {
                return NotFound();
            }

            return View(fixture);
        }

        // POST: Fixtures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fixture == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Fixture' is null.");
            }

            var fixture = await _context.Fixture.FindAsync(id);

            if (fixture != null)
            {
                _context.Fixture.Remove(fixture);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool FixtureExists(int id)
        {
            return (_context.Fixture?.Any(e => e.FixtureId == id)).GetValueOrDefault();
        }
    }
}
