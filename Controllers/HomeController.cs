using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Models;
using System.Diagnostics;
using MyField.ViewModels;


namespace MyField.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;

        public HomeController(ILogger<HomeController> logger, 
            Ksans_SportsDbContext db,
            UserManager<UserBaseModel> userManager)
        {
            _context = db;
            _logger = logger;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Dashboard");
            }
            else
            {
                return View(); 
            }
        }

        public IActionResult ErrorPage(string errorMessage)
        {
            ViewData["ErrorMessage"] = errorMessage;
            return View();
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }

            ViewBag.AnnouncementsCount = await GetAnnouncementsCount();

            var user = await _userManager.GetUserAsync(User);

            var roles = await _userManager.GetRolesAsync(user);


            if (roles.Contains("System Administrator"))
            {
                return View("SystemAdministratorDashboard");
            }
            else if (roles.Contains("Sport Coordinator"))
            {
                return View("SportsCoordinatorDashboard");
            }
            else if (roles.Contains("Player"))
            {
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

                var club = await _context.Club
                    .Where(mo => mo.ClubId == clubId)
                    .FirstOrDefaultAsync();

                return View("PlayerDashboard", club);
            }
            else if (roles.Contains("Club Manager"))
            {
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


                var club = await _context.Club
                    .Where(mo => mo.ClubId == clubId)
                    .FirstOrDefaultAsync();

                if(club.ClubManager.IsContractEnded)
                {
                    return View("Index");
                }
                else
                {
                    return View("ClubManagerDashboard", club);
                }
                
            }
            else if (roles.Contains("Club Administrator"))
            {


               
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

                var club = await _context.Club
                    .Where(mo => mo.ClubId == clubId)
                    .FirstOrDefaultAsync();

                ViewBag.MyClubmanagersCount = await GetMyClubManagersCount();

                ViewBag.MyClubPlayersCount = await GetMyClubPlayersCount();

                ViewBag.MyClubFixturesCount = await GetMyClubFixturesCount();

                ViewBag.MyClubTransferRequestsCount = await GetMyClubTransferRequestsCount();

                ViewBag.CLubAdministratorsMeetingsCount = await GetClubAdministratorsMeetingsCount();

                return View("ClubAdministratorDashboard", club);
            }
            else if (roles.Contains("News Updator"))
            {
                return View("NewsUpdatorDashboard");
            }
            else if(roles.Contains("Sport Administrator"))
            {
                return View("SportAdministratorDashboard");
            }
            else if (roles.Contains("News Administrator"))
            {
                return View("NewsAdministratorDashboard");
            }
            else if (roles.Contains("Sport Manager"))
            {
                return View("SportManagerDashboard");
            }
            else if (roles.Contains("Fans Administrator"))
            {
                return View("FansAdministratorDashboard");
            }
            else if (roles.Contains("Personnel Administrator"))
            {
                return View("PersonnelAdministratorDashboard");
            }
            else if (roles.Contains("Official"))
            {
                return View("OfficialsDashboard");
            }
            else
            {
                return View("Index");
            }
        }

        public async Task<int> GetMyClubManagersCount()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return 0;
            }

            int myClubmanagersCount = await _context.ClubManager
                .Where(m => m.ClubId == clubAdministrator.ClubId)
                .CountAsync();

                return myClubmanagersCount;
        }

        public async Task<int> GetMyClubPlayersCount()
        {
            var user = await _userManager.GetUserAsync(User);

            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return 0;
            }

            int myPlayersCount = await _context.Player
                .Where(m => m.ClubId == clubAdministrator.ClubId)
                .CountAsync();

            return myPlayersCount;
        }

        public async Task<int> GetMyClubFixturesCount()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return 0 ;
            }

            if (!(user is ClubAdministrator clubAdministrator) &&
                !(user is ClubManager clubManager) &&
                !(user is Player clubPlayer))
            {
                return 0;
            }

            var clubId = (user as ClubAdministrator)?.ClubId ??
                         (user as ClubManager)?.ClubId ??
                         (user as Player)?.ClubId;

            if (clubId == null)
            {
                return 0;
            }


            int myClubFixturesCount = await _context.Fixture
                .Where(m => m.HomeTeam.ClubId == clubId ||
                 m.AwayTeam.ClubId == clubId && 
                 m.FixtureStatus == FixtureStatus.Upcoming ||
                 m.FixtureStatus == FixtureStatus.Postponed ||
                   m.FixtureStatus == FixtureStatus.Interrupted)
                .CountAsync();

            return myClubFixturesCount;
        }


        public async Task<int> GetMyClubTransferRequestsCount()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return 0;
            }

            if (!(user is ClubAdministrator clubAdministrator) &&
                !(user is ClubManager clubManager) &&
                !(user is Player clubPlayer))
            {
                return 0;
            }

            var clubId = (user as ClubAdministrator)?.ClubId ??
                         (user as ClubManager)?.ClubId ??
                         (user as Player)?.ClubId;

            if (clubId == null)
            {
                return 0;
            }


            int myClubTransferRequestsCount = await _context.Transfer
                .Where(m => m.CustomerClub.ClubId == clubId &&
                m.Status == TransferStatus.Pending)
                .CountAsync();

            return myClubTransferRequestsCount;
        }

        public async Task<int> GetClubAdministratorsMeetingsCount()
        {
            var clubAdminsMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Club_Administrators)
                .CountAsync (); 

            return clubAdminsMeetings;
        }

        public async Task<int> GetAnnouncementsCount()
        {
            var announcementsCount = await _context.Announcements
                .CountAsync ();

            return announcementsCount;
        }


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Retrieve error messages from TempData
            var errorMessages = TempData["Errors"] as List<string> ?? new List<string>();

            // Pass error messages to the view
            var viewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessages = errorMessages
            };

            return View(viewModel);
        }



    }
}