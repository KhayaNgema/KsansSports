using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;

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

            var viewModel = new StartLiveViewModel
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

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartMatch([FromBody] StartLiveViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var liveMatch = await _context.Live
                    .Where(l => l.IsLive)
                    .Include(l => l.Fixture)
                    .ToListAsync();

                if (liveMatch.Any(lm => lm.FixtureId == viewModel.FixtureId))
                {
                    TempData["Message"] = "Match already started!";
                    return View(viewModel);
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

            return BadRequest(ModelState);
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
                IsEnded = liveMatch.ISEnded
            };

            return Ok(response);
        }






        /*  [HttpGet]
              public async Task<IActionResult> HomeGoal()
              {
                  return PartialView("_HomeGoalPartial");
              }

              [HttpPost]
              public async Task<IActionResult> HomeGoal()
              {
                  return Ok();
              }


              [HttpGet]
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
              public async Task<IActionResult> HalfTime()
              {
                  return PartialView("_HalfTimePartial");
              }

              [HttpPost]
              public async Task<IActionResult> HalfTime()
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
