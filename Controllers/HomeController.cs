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
        private readonly Ksans_SportsDbContext _db;
        private readonly UserManager<UserBaseModel> _userManager;

        public HomeController(ILogger<HomeController> logger, 
            Ksans_SportsDbContext db,
            UserManager<UserBaseModel> userManager)
        {
            _db = db;
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

            ViewBag.RejectedSportNews = await GetRejectedSportNewsCount();

            ViewBag.ApprovedSportNews = await GetApprovedSportNewsCount();

            ViewBag.PendingApprovalSportNews = await GetPendingApprovalSportNewsCount();

            var user = await _userManager.GetUserAsync(User);

            var roles = await _userManager.GetRolesAsync(user);


            // Determine the appropriate view based on user roles
            if (roles.Contains("System Administrator"))
            {
                return View("SystemAdministratorDashboard");
            }
            else if (roles.Contains("Sports Coordinator"))
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

                var club = await _db.Club
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

                var club = await _db.Club
                    .Where(mo => mo.ClubId == clubId)
                    .FirstOrDefaultAsync();

                return View("ClubManagerDashboard", club);
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

                var club = await _db.Club
                    .Where(mo => mo.ClubId == clubId)
                    .FirstOrDefaultAsync();

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
            else if (roles.Contains("News administrator"))
            {
                return View("NewsAdministratorDashboard");
            }
            else if (roles.Contains("Sport Manager"))
            {
                return View("SportManagerDashboard");
            }
            else if (roles.Contains("Fans administrator"))
            {
                return View("FansAdministratorDashboard");
            }
            else if (roles.Contains("Personnel administrator"))
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

        public async Task<int> GetApprovedSportNewsCount()
        {
            int approvedNewsCount = await _db.SportNew
                .Where(s => s.NewsStatus == NewsStatus.Approved)
                .CountAsync();

            return approvedNewsCount;
        }

        public async Task<int> GetPendingApprovalSportNewsCount()
        {
            int awaitingApprovalNewsCount = await _db.SportNew
                .Where(s => s.NewsStatus == NewsStatus.Awaiting_Approval)
                .CountAsync();

            return awaitingApprovalNewsCount;
        }

        public async Task<int> GetRejectedSportNewsCount()
        {
            int rejectedNewsCount = await _db.SportNew
                .Where(s => s.NewsStatus == NewsStatus.Rejected)
                .CountAsync();

            return rejectedNewsCount;
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