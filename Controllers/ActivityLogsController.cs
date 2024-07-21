using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class ActivityLogsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;

        public ActivityLogsController(Ksans_SportsDbContext context,
            UserManager<UserBaseModel> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [Authorize(Roles =("Club Administrator"))]
        public async Task<IActionResult> MyPlayersActivityLogs()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
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

            var players = await _context.Player
                .Where(p => p.ClubId == clubId)
                .ToListAsync();

            var playerLogs = new List<ActivityLog>();

            foreach (var player in players)
            {
                var logs = await _context.ActivityLogs
                    .Where(a => a.UserId == player.Id)
                    .Include(a => a.UserBaseModel)
                    .Include(a => a.DeviceInfo)
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync();

                playerLogs.AddRange(logs);
            }

            return View(playerLogs);
        }


        [Authorize]
        public async Task<IActionResult> MyActivityLogs()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                if (user == null)
                {
                    return NotFound();
                }

                var myActivityLogs = await _context.ActivityLogs
                    .Where(a => a.UserId == user.Id)
                    .Include(a => a.DeviceInfo)
                    .Include(a => a.UserBaseModel)
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync();

                return View(myActivityLogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> MyManagersActivityLogs()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
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

            var manager = await _context.ClubManager
                .Where(m => m.ClubId == clubId)
                .FirstOrDefaultAsync();

            if (manager == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var managerLogs = await _context.ActivityLogs
                .Where(a => a.UserId == manager.Id)
                .Include(a => a.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return View(managerLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> PersonnelLogs()
        {
            return View();
        }


        [Authorize(Roles = ("Fans Administrator"))]
        public async Task<IActionResult> FansActivityLogs()
        {
            var fansActivityLogs = await _context.ActivityLogs
                .Where(log => !_context.Roles
                    .Any(r => _context.UserRoles
                        .Any(ur => ur.UserId == log.UserId && ur.RoleId == r.Id)))
                .Include(log => log.UserBaseModel)
                .Include(log => log.DeviceInfo)
                .OrderByDescending( log => log.Timestamp)
                .ToListAsync();

            return View(fansActivityLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> ClubAdministratorsActivityLogs()
        {
            var clubAdministrators = await _context.ClubAdministrator
                .Include(c => c.Club)
                .ToListAsync();

            if (clubAdministrators == null || clubAdministrators.Count == 0)
            {
                return RedirectToAction("Error", "Home");
            }

            List<ActivityLogViewModel> allClubAdminLogs = new List<ActivityLogViewModel>();

            foreach (var clubAdministrator in clubAdministrators)
            {
                var clubAdministratorLogs = await _context.ActivityLogs
                    .Where(a => a.UserId == clubAdministrator.Id)
                    .Include(a => a.UserBaseModel)
                    .Include(a => a.DeviceInfo)
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync();

                foreach (var log in clubAdministratorLogs)
                {
                    var viewModel = new ActivityLogViewModel
                    {
                        FirstName = log.UserBaseModel?.FirstName,
                        LastName = log.UserBaseModel?.LastName,
                        ClubName = clubAdministrator.Club?.ClubName,
                        Activity = log.Activity,
                        Timestamp = log.Timestamp,
                        DeviceDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.DeviceName}, {log.DeviceInfo.OSName}, {log.DeviceInfo.OSVersion}, {log.DeviceInfo.DeviceModel}, {log.DeviceInfo.IpAddress}" : "",
                        BrowserDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.Browser}, {log.DeviceInfo.BrowserVersion}, {log.DeviceInfo.DeviceModel}" : "",
                        LocationDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.City}, {log.DeviceInfo.PostalCode}, {log.DeviceInfo.Region}, {log.DeviceInfo.Country}" : ""
                    };

                    allClubAdminLogs.Add(viewModel);
                }

                allClubAdminLogs = allClubAdminLogs.OrderByDescending(a => a.Timestamp).ToList();
            }

            return PartialView("_ClubAdministratorsActivityLogsPartial", allClubAdminLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> ClubManagersActivityLogs()
        {
            var clubManagers = await _context.ClubManager
                .Include(c => c.Club)
                .ToListAsync();

            if (clubManagers == null || clubManagers.Count == 0)
            {
                return RedirectToAction("Error", "Home");
            }

            List<ActivityLogViewModel> allClubManagerLogs = new List<ActivityLogViewModel>();

            foreach (var clubManager in clubManagers)
            {
                var clubManagerLogs = await _context.ActivityLogs
                    .Where(a => a.UserId == clubManager.Id)
                    .Include(a => a.UserBaseModel)
                    .Include(a => a.DeviceInfo)
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync();

                var club = await _context.Club
                    .Where(c => c.ClubId == clubManager.ClubId)
                    .FirstOrDefaultAsync();

                foreach (var log in clubManagerLogs)
                {
                    var viewModel = new ActivityLogViewModel
                    {
                        FirstName = log.UserBaseModel?.FirstName,
                        LastName = log.UserBaseModel?.LastName,
                        ClubName = club?.ClubName,
                        Activity = log.Activity,
                        Timestamp = log.Timestamp,
                        DeviceDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.DeviceName}, {log.DeviceInfo.OSName}, {log.DeviceInfo.OSVersion}, {log.DeviceInfo.DeviceModel}, {log.DeviceInfo.IpAddress}" : "",
                        BrowserDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.Browser}, {log.DeviceInfo.BrowserVersion}, {log.DeviceInfo.DeviceModel}" : "",
                        LocationDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.City}, {log.DeviceInfo.PostalCode}, {log.DeviceInfo.Region}, {log.DeviceInfo.Country}" : ""
                    };

                    allClubManagerLogs.Add(viewModel);
                }
            }

            allClubManagerLogs = allClubManagerLogs.OrderByDescending(a => a.Timestamp).ToList();

            return PartialView("_ClubManagersActivityLogsPartial", allClubManagerLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> NewsAdministratorsActivityLogs()
        {
            var newsAdminRoleId = await _context.Roles
                .Where(r => r.Name == "News Administrator")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (newsAdminRoleId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var newsAdminsActivityLogs = await _context.ActivityLogs
                .Include(n => n.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .OrderByDescending(a => a.Timestamp)
                .Where(a => _context.UserRoles
                    .Where(ur => ur.RoleId == newsAdminRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(a.UserId))
                .ToListAsync();

            return PartialView("_NewsAdministratorsActivityLogsPartial", newsAdminsActivityLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> SportAdministratorsActivityLogs()
        {
            var sportAdminRoleId = await _context.Roles
                .Where(r => r.Name == "Sport Administrator")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (sportAdminRoleId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var sportAdminsActivityLogs = await _context.ActivityLogs
                .Include(n => n.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .Where(a => _context.UserRoles
                    .Where(ur => ur.RoleId == sportAdminRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(a.UserId))
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return PartialView("_SportAdministratorsActivityLogsPartial", sportAdminsActivityLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> ManagersActivityLogs()
        {
            return View();
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> PlayersActivityLogs()
        {
            var clubPlayers = await _context.Player
                .Include(c => c.Club)
                .ToListAsync();

            if (clubPlayers == null || clubPlayers.Count == 0)
            {
                return RedirectToAction("Error", "Home");
            }

            List<ActivityLogViewModel> allClubPlayerLogs = new List<ActivityLogViewModel>();

            foreach (var clubPlayer in clubPlayers)
            {
                var clubPlayerLogs = await _context.ActivityLogs
                    .Where(a => a.UserId == clubPlayer.Id)
                    .Include(a => a.UserBaseModel)
                    .Include(a => a.DeviceInfo)
                    .OrderByDescending(a => a.Timestamp)
                    .ToListAsync();

                foreach (var log in clubPlayerLogs)
                {
                    var viewModel = new ActivityLogViewModel
                    {
                        FirstName = log.UserBaseModel?.FirstName,
                        LastName = log.UserBaseModel?.LastName,
                        ClubName = clubPlayer.Club?.ClubName,
                        Activity = log.Activity,
                        Timestamp = log.Timestamp,
                        DeviceDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.DeviceName}, {log.DeviceInfo.OSName}, {log.DeviceInfo.OSVersion}, {log.DeviceInfo.DeviceModel}, {log.DeviceInfo.IpAddress}" : "",
                        BrowserDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.Browser}, {log.DeviceInfo.BrowserVersion}, {log.DeviceInfo.DeviceModel}" : "",
                        LocationDetails = log.DeviceInfo != null ? $"{log.DeviceInfo.City}, {log.DeviceInfo.PostalCode}, {log.DeviceInfo.Region}, {log.DeviceInfo.Country}" : ""
                    };

                    allClubPlayerLogs.Add(viewModel);
                }
            }

            allClubPlayerLogs = allClubPlayerLogs.OrderByDescending(a => a.Timestamp).ToList();

            return PartialView("_PlayerActivityLogsPartial", allClubPlayerLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> NewsUpdatersActivityLogs()
        {
            var newsUpdaterRoleId = await _context.Roles
               .Where(r => r.Name == "News Updator")
               .Select(r => r.Id)
               .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(newsUpdaterRoleId))
            {
                return RedirectToAction("Error", "Home");
            }

            var newsUpdatersActivityLogs = await _context.ActivityLogs
                .Include(n => n.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .Where(a => _context.UserRoles
                    .Where(ur => ur.RoleId == newsUpdaterRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(a.UserId))
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return PartialView("_NewsUpdatersActivityLogsPartial", newsUpdatersActivityLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> SportCoordinatorsActivityLogs()
        {
            var sportCoordinatorRoleId = await _context.Roles
                .Where(r => r.Name == "Sport Coordinator")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (sportCoordinatorRoleId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var sportCoordinatorsActivityLogs = await _context.ActivityLogs
                .Include(n => n.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .Where(a => _context.UserRoles
                    .Where(ur => ur.RoleId == sportCoordinatorRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(a.UserId))
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return PartialView("_SportCoordinatorsActivityLogsPartial", sportCoordinatorsActivityLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> OfficialsActivityLogs()
        {
            var officialRoleId = await _context.Roles
              .Where(r => r.Name == "Official")
              .Select(r => r.Id)
              .FirstOrDefaultAsync();

            if (string.IsNullOrEmpty(officialRoleId))
            {
                return RedirectToAction("Error", "Home");
            }

            var officialsActivityLogs = await _context.ActivityLogs
                .Include(n => n.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .Where(a => _context.UserRoles
                    .Where(ur => ur.RoleId == officialRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(a.UserId))
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return PartialView("_OfficialsActivityLogsPartial", officialsActivityLogs);
        }

        [Authorize(Roles = ("System Administrator"))]
        public async Task<IActionResult> PersonnelAdministratorsActivityLogs()
        {
            var personnelAdminRoleId = await _context.Roles
                .Where(r => r.Name == "Personnel Administrator")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (personnelAdminRoleId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var personnelAdminsActivityLogs = await _context.ActivityLogs
                .Include(n => n.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .Where(a => _context.UserRoles
                    .Where(ur => ur.RoleId == personnelAdminRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(a.UserId))
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return PartialView("_PersonnelAdministratorsActivityLogsPartial", personnelAdminsActivityLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> SportManagersActivityLogs()
        {
            var sportManagerRoleId = await _context.Roles
                .Where(r => r.Name == "Sport Manager")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (sportManagerRoleId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var sportManagersActivityLogs = await _context.ActivityLogs
                .Include(n => n.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .Where(a => _context.UserRoles
                    .Where(ur => ur.RoleId == sportManagerRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(a.UserId))
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return PartialView("_SportManagersActivityLogsPartial", sportManagersActivityLogs);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> FansAdministratorsActivityLogs()
        {
            var fansAdminRoleId = await _context.Roles
                .Where(r => r.Name == "Fans Administrator")
                .Select(r => r.Id)
                .FirstOrDefaultAsync();

            if (fansAdminRoleId == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var fansAdminsActivityLogs = await _context.ActivityLogs
                .Include(n => n.UserBaseModel)
                .Include(a => a.DeviceInfo)
                .Where(a => _context.UserRoles
                    .Where(ur => ur.RoleId == fansAdminRoleId)
                    .Select(ur => ur.UserId)
                    .Contains(a.UserId))
                .OrderByDescending(a => a.Timestamp)
                .ToListAsync();

            return PartialView("_FansAdministratorsActivityLogsPartial", fansAdminsActivityLogs);
        }

    }
}
