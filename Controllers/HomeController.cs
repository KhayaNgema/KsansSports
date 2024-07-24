using Azure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Models;
using System.Diagnostics;
using MyField.ViewModels;
using MyField.Services;
using Microsoft.AspNetCore.Authorization;


namespace MyField.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly EmailService _emailService;

        public HomeController(ILogger<HomeController> logger, 
            Ksans_SportsDbContext db,
            UserManager<UserBaseModel> userManager,
            EmailService emailService)

        {
            _context = db;
            _logger = logger;
            _userManager = userManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {

                var user = await _userManager.GetUserAsync(User);

                var roles = await _userManager.GetRolesAsync(user);

                if (user.IsFirstTimeLogin == true && roles.Any())
                {
                    user.IsFirstTimeLogin = false;
                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return Redirect("/Identity/Account/Manage/ChangeFirstTimeLoginPassword");
                }
                else
                {
                    return RedirectToAction("Dashboard");
                }

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
                ViewBag.ClubsCount = await GetClubsCount();

                ViewBag.FixturesCount = await GetFixturesCount();

                ViewBag.MatchResultsCount = await GetMatchResultsCount();

                ViewBag.SportCoordinatorsMeetingsCount = await GetSportCoordinatorsMeetingsCount();

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

                ViewBag.MyClubFixturesCount = await GetMyClubFixturesCount();
                ViewBag.MyClubMatchResultsCount = await GetMyClubMatchResultsCount();
                ViewBag.PlayersMeetingsCount = await GetPlayersMeetingsCount();

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

                ViewBag.MyClubFixturesCount = await GetMyClubFixturesCount();
                ViewBag.MyClubMatchResultsCount = await GetMyClubMatchResultsCount();
                ViewBag.ClubManagersMeetingsCount = await GetClubManagersMeetingsCount();

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

                ViewBag.NewsUpdatersMeetingsCount = await GetNewsUpdaterMeetingsCount();

                ViewBag.ApprovedNewsCount = await GetApprovedNewsCount();

                ViewBag.PublishedNewsCount = await GetPublishedNewsCount();

                ViewBag.ToBeModifiedNewsCount = await GetToBeModifiedCount();

                return View("NewsUpdatorDashboard");
            }
            else if(roles.Contains("Sport Administrator"))
            {
                ViewBag.ClubsCount = await GetClubsCount();

                ViewBag.FixturesCount = await GetFixturesCount();

                ViewBag.MatchResultsCount = await GetMatchResultsCount();

                ViewBag.ClubFinesCount = await GetClubFinesCount();

                ViewBag.SportAdminsMeetings = await GetSportAdminsMeetingsCount();

                return View("SportAdministratorDashboard");
            }
            else if (roles.Contains("News Administrator"))
            {
                ViewBag.NewsPendingApprovalCount = await GetNewsPendingApprovalCount();

                ViewBag.ApprovedNewsCount = await GetApprovedNewsCount();

                ViewBag.PublishedNewsCount = await GetPublishedNewsCount();

                ViewBag.ToBeModifiedNewsCount = await GetToBeModifiedCount();

                ViewBag.NewsAdminsMeetingsCount = await GetNewsAdminMeetingsCount();

                return View("NewsAdministratorDashboard");
            }
            else if (roles.Contains("Sport Manager"))
            {
                ViewBag.ClubsCount = await GetClubsCount();

                ViewBag.FixturesCount = await GetFixturesCount();

                ViewBag.MatchResultsCount = await GetMatchResultsCount();

                ViewBag.ClubFinesCount = await GetClubFinesCount();

                ViewBag.SportManagersMeetings = await GetSportManagersMeetingsCount();


                return View("SportManagerDashboard");
            }
            else if (roles.Contains("Fans Administrator"))
            {
                ViewBag.DivisionFansCount = await GetDivisionFansCount();

                ViewBag.FansAdminsMeetingsCount = await GetFansAdminMeetingsCount();

                ViewBag.FansSupportQueriesCount = await GetFansSupportQueriesCount();

                return View("FansAdministratorDashboard");
            }
            else if (roles.Contains("Personnel Administrator"))
            {
                ViewBag.SportAdminsCount = await GetSportAdminsCount();

                ViewBag.SportManagersCount = await GetSportManagersCount();

                ViewBag.SportCoordinatorsCount = await GetSportCoordinatorsCount();

                ViewBag.OfficialsCount = await GetOfficialsCount();

                ViewBag.ClubAdminsCount = await GetClubAdminsCount();

                ViewBag.ClubManagersCount = await GetClubManagersCount();

                ViewBag.DivisionPlayersCount = await GetDivisionPlayersCount();

                ViewBag.NewsAdminsCount = await GetNewsAdminsCount();

                ViewBag.NewsUpdatersCount = await GetNewsUpdatersCount();

                ViewBag.FansAdminsCount = await GetFansAdminsCount();

                ViewBag.PersonnelAdminsMeetingsCount = await GetPersonnelAdminsMeetingsCount();

                return View("PersonnelAdministratorDashboard");
            }
            else if (roles.Contains("Official"))
            {
                ViewBag.MatchesToOfficiateCount = await GetMatchesToOfficiateCount();

                ViewBag.PreviouslyOfficiatedMatches = await PreviousylOfficiatedMatchesCount();

                ViewBag.OfficialsMeetingsCount = await GetOfficialsMeetingsCount();

                return View("OfficialsDashboard");
            }
            else
            {
                return View("Index");
            }
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<int> GetMyClubManagersCount()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return 0;
            }

            int myClubmanagersCount = await _context.ClubManager
                .Where(m => m.ClubId == clubAdministrator.ClubId && 
                !m.IsContractEnded && 
                !m.IsDeleted)
                .CountAsync();

                return myClubmanagersCount;
        }

        [Authorize(Roles = ("Club Administrator"))]
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

        [Authorize(Roles = ("Club Administrator, Club Manager, Player"))]
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

        [Authorize(Roles = "Club Administrator")]
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
                .Where(m => m.SellerClub.ClubId == clubId &&
                m.Status == TransferStatus.Pending)
                .CountAsync();

            return myClubTransferRequestsCount;
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<int> GetClubAdministratorsMeetingsCount()
        {
            var clubAdminsMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Club_Administrators)
                .CountAsync (); 

            return clubAdminsMeetings;
        }

        [Authorize]
        public async Task<int> GetAnnouncementsCount()
        {
            var announcementsCount = await _context.Announcements
                .CountAsync ();

            return announcementsCount;
        }

        [Authorize(Roles = ("Club Administrator, Club Manager, Player"))]
        public async Task<int> GetMyClubMatchResultsCount()
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


            int myClubMatchResultsCount = await _context.MatchResult
                .Where(m => m.HomeTeam.ClubId == clubId || 
                m.AwayTeam.ClubId == clubId)
                .CountAsync();

            return myClubMatchResultsCount;
        }

        [Authorize(Roles = ("Club Manager"))]
        public async Task<int> GetClubManagersMeetingsCount()
        {
            var clubManagersMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Club_Managers)
                .CountAsync();

            return clubManagersMeetings;
        }

        [Authorize(Roles = ("Fans Administrator"))]
        public async Task<int> GetDivisionFansCount()
        {
            var userIdsWithRoles = await _context.UserRoles
                .Select(ur => ur.UserId)
                .Distinct()
                .ToListAsync();

            var usersWithoutRoles = await _context.Users
                .Where(user => !userIdsWithRoles.Contains(user.Id))
                .Select(user => user.Id)
                .CountAsync();

            return usersWithoutRoles;
        }

        [Authorize(Roles = ("Fans Administrator"))]
        public async Task<int> GetFansAdminMeetingsCount()
        {
            var fansAdminMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Fans_Administrators)
                .CountAsync();

            return fansAdminMeetings;
        }

        [Authorize(Roles = ("Fans Administrator"))]
        public async Task<int> GetFansSupportQueriesCount()
        {
            var fansSupportQueries = 0;

            return fansSupportQueries;
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<int> GetNewsPendingApprovalCount()
        {
            var newsPendingApproval = await _context.SportNew
                .Where(n => n.NewsStatus == NewsStatus.Awaiting_Approval)
                .CountAsync();

            return newsPendingApproval;
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<int> GetApprovedNewsCount()
        {
            var approvedNews = await _context.SportNew
                .Where(n => n.NewsStatus == NewsStatus.Approved)
                .CountAsync();

            return approvedNews;
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<int> GetPublishedNewsCount()
        {
            var publishedNewsCount = await _context.SportNew
                .Where(n => n.NewsStatus == NewsStatus.Approved)
                .CountAsync();

            return publishedNewsCount;
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<int> GetToBeModifiedCount()
        {
            var toBeModifiedNewsCount = await _context.SportNew
                .Where(n => n.NewsStatus == NewsStatus.ToBeModified)
                .CountAsync();

            return toBeModifiedNewsCount;
        }

        [Authorize(Roles = ("News Administrator"))]
        public async Task<int> GetNewsAdminMeetingsCount()
        {
            var newsAdminMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.News_Administrators)
                .CountAsync();

            return newsAdminMeetings;
        }

        [Authorize(Roles = ("News Updator"))]
        public async Task<int> GetNewsUpdaterMeetingsCount()
        {
            var newsUpdaterMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.News_Updaters)
                .CountAsync();

            return newsUpdaterMeetings;
        }

        [Authorize(Roles = ("Official"))]
        public async Task<int> GetMatchesToOfficiateCount()
        {
            var user = await _userManager.GetUserAsync(User);

            var matchOfficials = await _context.MatchOfficials
                .Where(m => m.RefeereId == user.Id ||
                            m.AssistantOneId == user.Id ||
                            m.AssistantTwoId == user.Id)
                .Include(m => m.Fixture)
                .ToListAsync();

            var officiatedFixtureIds = matchOfficials.Select(m => m.FixtureId).Distinct().ToList();

            var matchesToOfficiateCount = await _context.Fixture
                .Where(m => officiatedFixtureIds.Contains(m.FixtureId) &&
                            m.FixtureStatus == FixtureStatus.Upcoming)
                .CountAsync();

            return matchesToOfficiateCount;
        }

        [Authorize(Roles = ("Official"))]
        public async Task<int> PreviousylOfficiatedMatchesCount()
        {
            var user = await _userManager.GetUserAsync(User);

            var matchOfficials = await _context.MatchOfficials
                .Where(m => m.RefeereId == user.Id ||
                            m.AssistantOneId == user.Id ||
                            m.AssistantTwoId == user.Id)
                .Include(m => m.Fixture)
                .ToListAsync();

            var officiatedFixtureIds = matchOfficials.Select(m => m.FixtureId).Distinct().ToList();

            var previoulsyOfficiatedMatchesCount = await _context.Fixture
                .Where(m => officiatedFixtureIds.Contains(m.FixtureId) &&
                            m.FixtureStatus == FixtureStatus.Ended)
                .CountAsync();

            return previoulsyOfficiatedMatchesCount;
        }

        [Authorize(Roles = ("Official"))]
        public async Task<int> GetOfficialsMeetingsCount()
        {
            var officialsMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Officials)
                .CountAsync();

            return officialsMeetings;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetSportAdminsCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Sport Administrator");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var sportAdminsCount = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return sportAdminsCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetSportManagersCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Sport Manager");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var sportManagersCount = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return sportManagersCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetSportCoordinatorsCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Sport Coordinator");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var sportCoordinatorsCount = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return sportCoordinatorsCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetOfficialsCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Official");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var officialsCount = await _context.Officials
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return officialsCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetClubAdminsCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Club Administrator");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var clubAdminsCount = await _context.ClubAdministrator
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return clubAdminsCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetClubManagersCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Club Manager");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var clubManagersCount = await _context.ClubManager
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return clubManagersCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetDivisionPlayersCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Player");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var divisionPlayersCount = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return divisionPlayersCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetNewsAdminsCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "News Administrator");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var newsAdminsCount = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return newsAdminsCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetNewsUpdatersCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "News Updator");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var newsUpdatersCount = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return newsUpdatersCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetFansAdminsCount()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Fans Administrator");

            if (role == null)
            {
                return 0;
            }

            var userIds = await _context.UserRoles
                .Where(ur => ur.RoleId == role.Id)
                .Select(ur => ur.UserId)
                .ToListAsync();

            var fansAdminsCount = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) && u.IsDeleted == false)
                .CountAsync();

            return fansAdminsCount;
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<int> GetPersonnelAdminsMeetingsCount()
        {
            var personnelAdminsMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Personnel_Administrators)
                .CountAsync();

            return personnelAdminsMeetings;
        }

        [Authorize(Roles = ("Player"))]
        public async Task<int> GetPlayersMeetingsCount()
        {
            var playersMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Players)
                .CountAsync();

            return playersMeetings;
        }

        [Authorize(Roles = ("Sport Administrator, Sport Coordinator"))]
        public async Task<int> GetClubsCount()
        {
            var clubCount = await _context.Club
                .Where( c => c.IsActive == true &&
                c.League.IsCurrent)
                .CountAsync();

            return clubCount;
        }

        [Authorize(Roles = ("Sport Administrator, Sport Coordinator"))]
        public async Task<int> GetFixturesCount()
        {
            var fixturesCount = await _context.Fixture
                .Where(f => f.League.IsCurrent)
                .CountAsync();

           return fixturesCount;
        }

        [Authorize(Roles = ("Sport Administrator, Sport Coordinator"))]
        public async Task<int> GetMatchResultsCount()
        {
            var matchResultsCount = await _context.MatchResult
                .Where(m => m.League.IsCurrent)
                .CountAsync();


            return matchResultsCount;
        }

        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<int> GetClubFinesCount()
        {
            var clubFinesCount = await _context.Fines
                .Where(c => c.Club != null &&
                c.Offender == null &&
                c.PaymentStatus == PaymentStatus.Pending)
                .CountAsync();

            return clubFinesCount;
        }

        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<int> GetSportAdminsMeetingsCount()
        {
            var sportAdminsMeetings = await _context.Meeting
                .CountAsync();

            return sportAdminsMeetings;
        }

        [Authorize(Roles = ("Sport Coordinator"))]
        public async Task<int> GetSportCoordinatorsMeetingsCount()
        {
            var sportCoordinatorsMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Sport_Coordinators)
                .CountAsync();

            return sportCoordinatorsMeetings;
        }

        [Authorize(Roles = ("Sport Manager"))]
        public async Task<int> GetSportManagersMeetingsCount()
        {
            var sportManagersMeetings = await _context.Meeting
                .Where(c => c.MeetingAttendees == MeetingAttendees.Everyone ||
                c.MeetingAttendees == MeetingAttendees.Sport_Managers)
                .CountAsync();

            return sportManagersMeetings;
        }

        public IActionResult PrivacyPolicy()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult ContactUs()
        {
            return View();
        }


        public IActionResult TermsAndConditions()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {

            var errorMessages = TempData["Errors"] as List<string> ?? new List<string>();


            var viewModel = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessages = errorMessages
            };

            return View(viewModel);
        }



    }
}