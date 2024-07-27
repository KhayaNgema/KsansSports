using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public LivesController(Ksans_SportsDbContext context,
            FileUploadService fileUploadService,
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger,
            IEncryptionService encryptionService)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
            _activityLogger = activityLogger;
            _encryptionService = encryptionService;
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
                LiveTime = "0",
                HomeTeamScore = 0,
                AwayTeamScore = 0
            };

            return View(viewModel);
        }

/*        [HttpPost]
        public async Task<IActionResult> StartMatch(StartLiveViewModel viewModel)
        {
            return Ok();
        }

        [HttpGet]
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
            return PartialView("_HomeYellow");
        }

        [HttpPost]
        public async Task<IActionResult> HomeYellow()
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> AwayYellow()
        {
            return PartialView("_AwayYellow");
        }

        [HttpPost]
        public async Task<IActionResult> AwayYellow()
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> HomeSub()
        {
            return PartialView("_HomeSub");
        }

        [HttpPost]
        public async Task<IActionResult> HomeSub()
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> AwaySub()
        {
            return PartialView("_AwaySub");
        }

        [HttpPost]
        public async Task<IActionResult> AwaySub()
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> HomeRed()
        {
            return PartialView("_HomeRed");
        }

        [HttpPost]
        public async Task<IActionResult> HomeRed()
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> AwayRed()
        {
            return PartialView("_AwayRed");
        }

        [HttpPost]
        public async Task<IActionResult> AwayRed()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> HomePenalty()
        {
            return PartialView("_HomePenalty");
        }

        [HttpPost]
        public async Task<IActionResult> HomePenalty()
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> AwayPenalty()
        {
            return PartialView("_AwayPenalty");
        }

        [HttpPost]
        public async Task<IActionResult> AwayPenalty()
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> HalfTime()
        {
            return PartialView("_HalfTime");
        }

        [HttpPost]
        public async Task<IActionResult> HalfTime()
        {
            return Ok();
        }


        [HttpGet]
        public async Task<IActionResult> FullTime()
        {
            return PartialView("_FullTime");
        }

        [HttpPost]
        public async Task<IActionResult> FullTime()
        {
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> AddTime()
        {
            return PartialView("_AddTime");
        }

        [HttpPost]
        public async Task<IActionResult> AddTime()
        {
            return Ok();
        }*/
    }
}
