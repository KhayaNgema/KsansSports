using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;
using System.Linq;


namespace MyField.Controllers
{
    public class LivesController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;
        private readonly IEncryptionService _encryptionService;
        private readonly IServiceProvider _serviceProvider;

        public LivesController(Ksans_SportsDbContext context,
            FileUploadService fileUploadService,
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger,
            IEncryptionService encryptionService,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
            _activityLogger = activityLogger;
            _encryptionService = encryptionService;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> StartLive(string fixtureId)
        {
            var decryptedFixtureId = _encryptionService.DecryptToInt(fixtureId);

            var fixture = await _context.Fixture
                .Where(f => f.FixtureId == decryptedFixtureId)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync();

            if (fixture == null)
            {
                return NotFound("Fixture not found.");
            }

            var startLiveViewModel = new StartLiveViewModel
            {
                FixtureId = decryptedFixtureId,
                FixturedClubs = $"{fixture.HomeTeam.ClubName} vs {fixture.AwayTeam.ClubName}",
                KickOffDate = fixture.KickOffDate,
                KickOffTime = fixture.KickOffTime,
                HomeTeamId = fixture.HomeTeamId,
                AwayTeamId = fixture.AwayTeamId,
                HomeTeamName = fixture.HomeTeam.ClubName,
                HomeTeamBadge = fixture.HomeTeam.ClubBadge,
                AwayTeamName = fixture.AwayTeam.ClubName,
                AwayTeamBadge = fixture.AwayTeam.ClubBadge,
                LiveTime = 0,
                HomeTeamScore = 0,
                AwayTeamScore = 0
            };

            // Fetch players or relevant data
            var players = await _context.Player
                .Where(p => p.ClubId == fixture.HomeTeamId)
                .Select(p => new
                {
                    PlayerId = p.Id,
                    FullName = $"{p.FirstName} {p.LastName}",
                    JerseyNumber = p.JerseyNumber
                })
                .ToListAsync();

            var homeGoalCombinedViewModel = new HomeGoalCombinedViewModel
            {
                Players = players.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList()
            };

            var combinedViewModel = new CombinedStartLiveViewModel
            {
                StartLiveViewModel = startLiveViewModel,
                HomeGoalCombinedViewModel = homeGoalCombinedViewModel
            };

            return View(combinedViewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartMatch([FromBody] StartLiveViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLiveMatch = await _context.Live
                .Where(lm => lm.FixtureId == viewModel.FixtureId)
                .FirstOrDefaultAsync();

            if (existingLiveMatch != null)
            {
                if (existingLiveMatch.ISEnded)
                {
                    TempData["Message"] = "Cannot start match, it has already ended!";
                    return BadRequest(new { success = false, message = TempData["Message"] });
                }

                TempData["Message"] = "Match already started!";
                return BadRequest(new { success = false, message = TempData["Message"] });
            }

            var newLive = new Live
            {
                FixtureId = viewModel.FixtureId,
                HomeTeamScore = viewModel.HomeTeamScore,
                AwayTeamScore = viewModel.AwayTeamScore,
                ISEnded = false,
                IsHalfTime = false,
                IsLive = true,
                LiveTime = 0
            };

            _context.Live.Add(newLive);
            await _context.SaveChangesAsync();

            RecurringJob.AddOrUpdate(
                $"update-live-time-{newLive.LiveId}",
                () => UpdateLiveTime(newLive.LiveId),
                Cron.Minutely);

            TempData["Message"] = "Live match started successfully!";
            return Ok(new { success = true, liveId = newLive.LiveId, message = TempData["Message"] });
        }


        public async Task UpdateLiveTime(int liveId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Ksans_SportsDbContext>();

                var live = await context.Live.FindAsync(liveId);
                if (live == null || !live.IsLive) return;

                live.LiveTime++;
                context.Update(live);
                await context.SaveChangesAsync();
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetLiveMatchStatus(int fixtureId)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId)
                .FirstOrDefaultAsync();

            if (liveMatch == null)
            {
                return NotFound();
            }

            var response = new
            {
                LiveTime = liveMatch.LiveTime,
                IsLive = liveMatch.IsLive,
                IsHalfTime = liveMatch.IsHalfTime,
                IsEnded = liveMatch.ISEnded,
                WentToHalfTime = liveMatch.WentToHalfTime,
                HomeTeamScore = liveMatch.HomeTeamScore  
            };

            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> HalfTime(int fixtureId)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId)
                .FirstOrDefaultAsync();

            if (liveMatch != null)
            {
                liveMatch.IsLive = false;
                liveMatch.IsHalfTime = true;
                liveMatch.ISEnded = false;
                liveMatch.LiveTime = 45;
                liveMatch.WentToHalfTime = true;

                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound("Live match not found.");
        }


        [HttpPost]
        public async Task<IActionResult> ResumeMatch(int fixtureId)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId)
                .FirstOrDefaultAsync();

            if (liveMatch != null)
            {
                liveMatch.IsLive = true;
                liveMatch.IsHalfTime = false;
                liveMatch.ISEnded = false;
                liveMatch.LiveTime = 45;

                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound("Live match not found.");
        }


        [HttpPost]
        public async Task<IActionResult> EndMatch(int fixtureId, StartLiveViewModel viewModel)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId)
                .FirstOrDefaultAsync();

            if (liveMatch == null)
            {
                return NotFound("Live match not found.");
            }

            if (liveMatch.ISEnded)
            {
                return View(viewModel);
            }

            liveMatch.IsLive = false;
            liveMatch.IsHalfTime = false;
            liveMatch.ISEnded = true;
            liveMatch.LiveTime = 90;

            _context.Update(liveMatch);
            await _context.SaveChangesAsync();

            RecurringJob.RemoveIfExists($"update-live-time-{liveMatch.LiveId}");

            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> HomeGoal(int fixtureId)
        {
            var fixture = await _context.Fixture
                .Where(f => f.FixtureId == fixtureId)
                .Include(f => f.HomeTeam)
                .FirstOrDefaultAsync();

            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId)
                .FirstOrDefaultAsync();

            if (fixture == null)
            {
                return NotFound("Fixture not found.");
            }

            var homeLineUp = await _context.LineUp
                .Where(l => l.FixtureId == fixtureId && l.ClubId == fixture.HomeTeam.ClubId)
                .Include(l => l.LineUpXI)
                .ThenInclude(l => l.ClubPlayer)
                .Include(l => l.LineUpSubstitutes)
                .ThenInclude(l => l.ClubPlayer)
                .ToListAsync();

            var players = homeLineUp
                .SelectMany(l => l.LineUpXI.Select(xi => new
                {
                    xi.PlayerId,
                    FullName = $"{xi.ClubPlayer.FirstName} {xi.ClubPlayer.LastName}",
                    xi.ClubPlayer.JerseyNumber
                }))
                .Concat(homeLineUp
                    .SelectMany(l => l.LineUpSubstitutes.Select(sub => new
                    {
                        sub.PlayerId,
                        FullName = $"{sub.ClubPlayer.FirstName} {sub.ClubPlayer.LastName}",
                        sub.ClubPlayer.JerseyNumber
                    })))
                .ToList();

            var viewModel = new HomeGoalCombinedViewModel
            {
                HomeGoalViewModel = new HomeGoalViewModel
                {
                    LiveId = liveMatch?.LiveId ?? 0
                },
                Players = players,
                FixtureId = fixtureId
               
            };

            ViewBag.HomeTeam = fixture.HomeTeam.ClubName;

            return PartialView("_HomeGoalPartial", viewModel);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HomeGoal(int fixtureId, string goalScoredBy, string assistedBy, string scoredTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .FirstOrDefaultAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if(!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                liveMatch.HomeTeamScore++;

                var newGoal = new LiveGoalHolder
                {
                    PlayerId = goalScoredBy,
                    LiveId = liveMatch.LiveId,
                    ScoredTime = scoredTime,
                };

                var newAssist = new LiveAssistHolder
                {
                    PlayerId = assistedBy,
                    LiveId = liveMatch.LiveId,
                };

                _context.Add(newGoal);
                _context.Add(newAssist);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}"); // Log the exception message
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }





        /* [HttpGet]
         public async Task<IActionResult> AwayGoal()
         {
             return PartialView("_AwayGoalPartial");
         }

         [HttpPost]
         public async Task<IActionResult> AwayGoal()
         {
             return Ok();
         }

         [HttpGet]
         public async Task<IActionResult> HomeYellow()
         {
             return PartialView("_HomeYellowPartial");
         }

         [HttpPost]
         public async Task<IActionResult> HomeYellow()
         {
             return Ok();
         }


         [HttpGet]
         public async Task<IActionResult> AwayYellow()
         {
             return PartialView("_AwayYellowPartial");
         }

         [HttpPost]
         public async Task<IActionResult> AwayYellow()
         {
             return Ok();
         }


         [HttpGet]
         public async Task<IActionResult> HomeSub()
         {
             return PartialView("_HomeSubPartial");
         }

         [HttpPost]
         public async Task<IActionResult> HomeSub()
         {
             return Ok();
         }


         [HttpGet]
         public async Task<IActionResult> AwaySub()
         {
             return PartialView("_AwaySubPartial");
         }

         [HttpPost]
         public async Task<IActionResult> AwaySub()
         {
             return Ok();
         }


         [HttpGet]
         public async Task<IActionResult> HomeRed()
         {
             return PartialView("_HomeRedPartial");
         }

         [HttpPost]
         public async Task<IActionResult> HomeRed()
         {
             return Ok();
         }


         [HttpGet]
         public async Task<IActionResult> AwayRed()
         {
             return PartialView("_AwayRedPartial");
         }

         [HttpPost]
         public async Task<IActionResult> AwayRed()
         {
             return Ok();
         }

         [HttpGet]
         public async Task<IActionResult> HomePenalty()
         {
             return PartialView("_HomePenaltyPartial");
         }

         [HttpPost]
         public async Task<IActionResult> HomePenalty()
         {
             return Ok();
         }


         [HttpGet]
         public async Task<IActionResult> AwayPenalty()
         {
             return PartialView("_AwayPenaltyPartial");
         }

         [HttpPost]
         public async Task<IActionResult> AwayPenalty()
         {
             return Ok();
         }


         [HttpGet]
         public async Task<IActionResult> FullTime()
         {
             return PartialView("_FullTimePartial");
         }

         [HttpPost]
         public async Task<IActionResult> FullTime()
         {
             return Ok();
         }

         [HttpGet]
         public async Task<IActionResult> AddTime()
         {
             return PartialView("_AddTimePartial");
         }

         [HttpPost]
         public async Task<IActionResult> AddTime()
         {
             return Ok();
         }*/
    }
}
