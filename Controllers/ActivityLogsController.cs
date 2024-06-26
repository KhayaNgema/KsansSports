using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Models;
using MyField.Services;

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

            var player = await _context.Player
                .Where(p => p.ClubId == clubId)
                .FirstOrDefaultAsync();

            if (player == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var playerLogs = await _context.ActivityLogs
                .Where(a => a.UserId == player.Id)
                .Include(p => p.UserBaseModel)
                .Include(p => p.DeviceInfo)
                .ToListAsync();

            return View(playerLogs);
        }


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
                .ToListAsync();

            return View(managerLogs);
        }

        public async Task<IActionResult> PersonnelLogs()
        {
            return View();
        }


        public async Task<IActionResult> FansActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> ClubAdministratorsActivityLogs()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var clubAdministrator = await _context.ClubAdministrator
                .Include(c => c.Club)
                .FirstOrDefaultAsync();

            if (clubAdministrator == null)
            {
                return RedirectToAction("Error", "Home");
            }

            var clubAdministratorLogs = await _context.ActivityLogs
                .Where(a => a.UserId == clubAdministrator.Id)
                .Include(a => a.UserBaseModel)
                 .ThenInclude(u => ((ClubAdministrator)u).Club)
                .Include(a => a.DeviceInfo)
                .ToListAsync();

            foreach (var a in clubAdministratorLogs)
            {
                var clubAdmin = await _context.ClubAdministrator
                    .Include(c => c.Club)
                    .FirstOrDefaultAsync();

                if (clubAdmin != null && clubAdmin.Club != null)
                {
                    ViewBag.ClubName = clubAdmin.Club.ClubName;
                }
                else
                {
                    ViewBag.ClubName = "N/A";
                }
            }

            return PartialView("_ClubAdministratorsActivityLogsPartial", clubAdministratorLogs);
        }

        public async Task<IActionResult> NewsAdministratorsActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> SportAdministratorsActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> ManagersActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> PlayersActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> NewsUpdatersActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> SportCoordinatorsActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> OfficialsActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> PersonnelAdministratorsActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> SportManagersActivityLogs()
        {
            return View();
        }

        public async Task<IActionResult> FansAdministratorsActivityLogs()
        {
            return View();
        }
    }
}
