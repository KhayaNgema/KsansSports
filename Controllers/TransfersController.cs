using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Migrations;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;
using NuGet.Packaging;


namespace MyField.Controllers
{
    public class TransfersController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IMapper _mapper;
        private readonly IActivityLogger _activityLogger;
        private readonly EmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;

        public TransfersController(Ksans_SportsDbContext context,
            UserManager<UserBaseModel> userManager,
             IMapper mapper,
             IActivityLogger activityLogger,
             EmailService emailService,
             RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _activityLogger = activityLogger;
            _emailService = emailService;
            _roleManager = roleManager;
        }

        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<IActionResult> TransferPeriod()
        {
            var transferPeriod = await _context.TransferPeriod
                .Include(t => t.League)
                .Include(t => t.CreatedBy)
                .Include(t => t.ModifiedBy)
                .OrderByDescending(t => t.CreatedDateTime)
                .ToListAsync();

            return View(transferPeriod);
        }


        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> PlayerTransfers()
        {

            var currentSeason = await _context.League
                 .Where(c => c.IsCurrent)
                  .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;


            return View();
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> PendingTransfers()
        {
            var pendingTrasfers = await _context.Transfer
                .Where(p => p.Status == TransferStatus.Pending && 
                       p.SellerClub.League.IsCurrent && 
                       p.CustomerClub.League.IsCurrent &&
                       p.League.IsCurrent)
                .Include(p => p.SellerClub)
                .Include(p => p.CustomerClub)
                .Include(p => p.Player)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return PartialView("_PendingPlayerTransfersPartial", pendingTrasfers);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> PaidTransfers()
        {
            var paidTrasfers = await _context.Transfer
                .Where(p => p.Status == TransferStatus.Completed &&
                       p.SellerClub.League.IsCurrent &&
                       p.CustomerClub.League.IsCurrent &&
                       p.League.IsCurrent)
                .Include(p => p.SellerClub)
                .Include(p => p.CustomerClub)
                .Include(p => p.Player)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return PartialView("_PaidPlayerTransfersPartial", paidTrasfers);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> RejectedTransfers()
        {
            var rejectedTrasfers = await _context.Transfer
                .Where(p => p.Status == TransferStatus.Rejected &&
                       p.SellerClub.League.IsCurrent &&
                       p.CustomerClub.League.IsCurrent &&
                       p.League.IsCurrent)
                .Include(p => p.SellerClub)
                .Include(p => p.CustomerClub)
                .Include(p => p.Player)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return PartialView("_RejectedPlayerTransfersPartial", rejectedTrasfers);
        }

        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> AcceptedTransfers()
        {
            var acceptedTrasfers = await _context.Transfer
                .Where(p => p.Status == TransferStatus.Accepted &&
                       p.SellerClub.League.IsCurrent &&
                       p.CustomerClub.League.IsCurrent &&
                       p.League.IsCurrent)
                .Include(p => p.SellerClub)
                .Include(p => p.CustomerClub)
                .Include(p => p.Player)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return PartialView("_AcceptedPlayerTransfersPartial", acceptedTrasfers);
        }

        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<IActionResult> FindPlayerTransferMarket()
        {
            return View();
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> MyTransferRequestsTabs()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var clubs = await _context.Club
                                      .FirstOrDefaultAsync(mo => mo.ClubId == clubId);

            ViewBag.ClubName = clubs?.ClubName;

            return View();
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> MyTransfersTabs()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var clubs = await _context.Club
                                      .FirstOrDefaultAsync(mo => mo.ClubId == clubId);

            ViewBag.ClubName = clubs?.ClubName;

            return View();
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> MyPendingPlayerTransfers()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var pendingTransfers = await _context.Transfer
                .Where(mo => mo.Status == TransferStatus.Pending && 
                mo.SellerClub.ClubId == clubId &&
                 mo.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                 .Include(s => s.CustomerClub)
                .ToListAsync();

            return PartialView("MyPendingTransfers", pendingTransfers);
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> MyAcceptedPlayerTransfers()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var acceptedTransfers = await _context.Transfer
                .Where(mo => mo.Status == TransferStatus.Accepted && 
                mo.SellerClub.ClubId == clubId &&
                mo.SellerClub.League.IsCurrent &&
                mo.CustomerClub.League.IsCurrent &&
                 mo.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .Include(s => s.CustomerClub)
                .ToListAsync();

            return PartialView("MyAcceptedTransfers", acceptedTransfers);
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> MyRejectedPlayerTransfers()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var rejectedTransfers = await _context.Transfer
                .Where(mo => mo.Status == TransferStatus.Rejected && 
                mo.SellerClub.ClubId == clubId &&
                mo.SellerClub.League.IsCurrent &&
                mo.CustomerClub.League.IsCurrent &&
                 mo.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .Include(s => s.CustomerClub)
                .ToListAsync();

            return PartialView("MyRejectedTransfers", rejectedTransfers);
        }

        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> PendingPlayerTransfers()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var pendingTransfers = await _context.Transfer
                .Where(mo => mo.Status == TransferStatus.Pending && 
                mo.CustomerClub.ClubId == clubId &&
                mo.SellerClub.League.IsCurrent &&
                mo.CustomerClub.League.IsCurrent &&
                 mo.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .ToListAsync();

            return PartialView("PendingTransfers", pendingTransfers);
        }


        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> CancelledPlayerTransfers()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var pendingTransfers = await _context.Transfer
                .Where(mo => mo.Status == TransferStatus.Cancelled &&
                mo.CustomerClub.ClubId == clubId &&
                mo.SellerClub.League.IsCurrent &&
                mo.CustomerClub.League.IsCurrent &&
                mo.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .ToListAsync();

            return PartialView("CancelledTransfers", pendingTransfers);
        }


        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> AcceptedPlayerTransfers()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var acceptedTransfers = await _context.Transfer
                .Where(mo => mo.Status == TransferStatus.Accepted &&
                mo.CustomerClub.ClubId == clubId &&
                mo.SellerClub.League.IsCurrent &&
                mo.CustomerClub.League.IsCurrent &&
                mo.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .ToListAsync();

            foreach(var acceptedTransfer in acceptedTransfers)
            {
                var playerMarketValue = await _context.Transfer
                    .Where(mo => mo.Status == TransferStatus.Accepted)
                    .Include(s => s.Player)
                    .FirstOrDefaultAsync();


                if(playerMarketValue != null && playerMarketValue.Player.MarketValue != null)
                {
                    ViewBag.PlayerMarketValue = playerMarketValue.Player.MarketValue;
                }

                else
                {
                    ViewBag.PlayerMarketValue = "NA";
                }
            }


            return PartialView("AcceptedTransfers", acceptedTransfers);
        }


        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> RejectedPlayerTransfers()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var rejectedTransfers = await _context.Transfer
               .Where(mo => mo.Status == TransferStatus.Rejected &&
                mo.CustomerClub.ClubId == clubId &&
                mo.SellerClub.League.IsCurrent &&
                mo.CustomerClub.League.IsCurrent &&
                mo.League.IsCurrent)
               .Include(s => s.Player)
               .Include(s => s.SellerClub)
               .ToListAsync();

            return PartialView("RejectedTransfers", rejectedTransfers);
        }


        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> CompletedPlayerTransfers()
        {
            var user = await _userManager.GetUserAsync(User);

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

            var completedTransfers = await _context.Transfer
                     .Where(mo => mo.Status == TransferStatus.Completed &&
                      mo.CustomerClub.ClubId == clubId &&
                      mo.SellerClub.League.IsCurrent &&
                      mo.CustomerClub.League.IsCurrent &&
                      mo.League.IsCurrent)
                     .Include(s => s.Player)
                     .Include(s => s.SellerClub)
                     .ToListAsync();

            return PartialView("CompletedTransfers", completedTransfers);
        }


        [Authorize(Roles = ("Personnel Administrator"))]
        public async Task<IActionResult> TransferList()
        {
            var transferLists = await _context.Transfer
                .Where(t => t.League.IsCurrent)
                .Include(s => s.SellerClub)
                .Include(s => s.CustomerClub)
                .Include(s => s.CreatedBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.Player)
                .ToListAsync();

            return PartialView(transferLists);
        }


        [Authorize]
        public async Task<IActionResult> TransferMarket()
        {
            var playerTransferMarket = await _context.PlayerTransferMarket
                .Where(p => !p.IsArchived && p.League.IsCurrent)
                .Include(s => s.Club)
                .Include(s => s.CreatedBy)
                .Include(s => s.Player)
                .ToListAsync();

            return View(playerTransferMarket);
        }


        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<IActionResult> PlayerTransferMarket()
        {
            var playerTransferMarket = await _context.PlayerTransferMarket
                .Where(p => !p.IsArchived && p.League.IsCurrent)
                .Include(s => s.Club)
                .Include(s => s.CreatedBy)
                .Include(s => s.Player)
                .ToListAsync();

            return PartialView("_PlayerTransferMarketPartial", playerTransferMarket);
        }

        [Authorize(Roles = ("Sport Administrator"))]
        [HttpPost]
        public async Task<IActionResult> OpenPlayerTransferPeriod(string seasonCode)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var currentSeason = await _context.League
                        .Where(league => league.IsCurrent)
                        .FirstOrDefaultAsync();

                    var transferPeriod = await _context.TransferPeriod
                        .Where(t => t.IsCurrent)
                        .FirstOrDefaultAsync();

                    var transferReport = await _context.TransfersReports
                        .Where(t => t.Season.IsCurrent)
                        .FirstOrDefaultAsync();

                    var currentSeasonCode = currentSeason?.LeagueCode;

                    if (transferPeriod.PeriodOpenCount >= 2)
                    {
                        TempData["Message"] = "Transfer period can only be opened twice per season. Please wait until next season.";
                        return NotFound();
                    }

                    if (currentSeason != null && currentSeasonCode == seasonCode)
                    {
                        var players = await _context.Player
                            .Where(p => p.Club.LeagueId == currentSeason.LeagueId)
                            .Include(s => s.Club)
                            .ToListAsync();

                        var user = await _userManager.GetUserAsync(User);
                        var userId = user.Id;

                        var transferMarkets = new List<PlayerTransferMarket>();

                        foreach (var player in players)
                        {
                            var transfermarket = new PlayerTransferMarket
                            {
                                PlayerId = player.Id,
                                ClubId = player.ClubId,
                                CreatedById = userId,
                                CreatedDateTime = DateTime.UtcNow,
                                SaleStatus = SaleStatus.Available,
                                LeagueId = currentSeason.LeagueId,
                            };

                            transferMarkets.Add(transfermarket);
                        }

                        transferPeriod.PeriodOpenCount++;
                        transferPeriod.IsOpened = true;
                        transferPeriod.ModifiedDateTime = DateTime.Now;
                        transferPeriod.ModifiedById = user.Id;

                        await _context.PlayerTransferMarket.AddRangeAsync(transferMarkets);
                        await _context.SaveChangesAsync();

                        int transferMarketCount = await GetTransferMarketCountAsync();
                        transferReport.TransferMarketCount = transferMarketCount;

                        var roleNames = new[] { "Club Administrator", "Club Manager" };
                        var roleUsers = new List<UserBaseModel>();

                        foreach (var roleName in roleNames)
                        {
                            var role = await _roleManager.FindByNameAsync(roleName);
                            if (role != null)
                            {
                                var userIds = await _userManager.GetUsersInRoleAsync(roleName);
                                roleUsers.AddRange(userIds);
                            }
                        }

                        var subject = "Transfer Period Opened";
                        var emailBodyTemplate = $@"
                Dear Club Administrator/Manager,<br/><br/>
                The player transfer period for season <b>{currentSeason.LeagueYears}</b> has been opened.<br/><br/>
                Clubs can now start making player purchases.<br/><br/>
                If you have any questions or need further assistance, please contact us at support@ksfoundation.com.<br/><br/>
                Regards,<br/>
                K&S Foundation Support Team
                ";

                        foreach (var roleUser in roleUsers.Distinct())
                        {
                            BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(roleUser.Email, subject, emailBodyTemplate));
                        }

                        TempData["Message"] = $"Transfer period for season {currentSeason.LeagueYears} has been opened successfully and clubs can now start making player purchases";
                        await _activityLogger.Log($"Opened player transfer period for season {currentSeason.LeagueYears}", user.Id);
                        return Ok();
                    }
                    else
                    {
                        TempData["Message"] = "Failed to open transfer period. Season code does not match the code of the current season.";
                        return Ok();
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage));
                    return Json(new { success = false, error = "Model state is invalid", errors = errors });
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to open transfer period: " + ex.Message,
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    }
                });
            }
        }


        public async Task<int> GetTransferMarketCountAsync()
        {
            return await _context.PlayerTransferMarket.CountAsync();
        }

        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<IActionResult> ClosePlayerTransferPeriod(string seasonCode)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);

                var currentSeason = await _context.League
                    .Where(league => league.IsCurrent)
                    .FirstOrDefaultAsync();

                var transferPeriod = await _context.TransferPeriod
                    .Where(t => t.IsOpened)
                    .FirstOrDefaultAsync();

                if (currentSeason == null)
                {
                    TempData["Message"] = "Failed to close transfer period. No current season found.";
                    return NotFound();
                }

                var currentSeasonCode = currentSeason.LeagueCode;

                if (currentSeasonCode != seasonCode)
                {
                    TempData["Message"] = $"Failed to close transfer period. Season code '{seasonCode}' does not match the code of the current season '{currentSeasonCode}'.";
                    return NotFound();
                }

                var recordsToArchive = await _context.PlayerTransferMarket
                    .ToListAsync();

                transferPeriod.IsOpened = false;
                transferPeriod.ModifiedDateTime = DateTime.Now;
                transferPeriod.ModifiedById = user.Id;

                _context.PlayerTransferMarket.RemoveRange(recordsToArchive);

                await _context.SaveChangesAsync();
                await _activityLogger.Log($"Closed player transfer period for season {currentSeason.LeagueYears}", user.Id);
                TempData["Message"] = $"Transfer period for season {currentSeason.LeagueYears} has been ended and no player purchases are allowed.";
                return Ok();
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "An error occurred while processing your request: " + ex.Message,
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    }
                });
            }
        }



        public async Task<IActionResult> ValidateSeasonCode(string seasonCode)
        {
            var currentSeason = await _context.League
                .FirstOrDefaultAsync(league => league.IsCurrent);

            var isValid = currentSeason != null && currentSeason.LeagueCode == seasonCode;

            return Json(new { isValid = isValid });
        }


        [Authorize(Roles = ("Club Administrator"))]
        [HttpGet]
        public async Task<IActionResult> InitiatePlayerTransfer(string playerId, int marketId, int clubId)
        {
            try
            {
                var player = await _context.Player
                    .Where(mo => mo.Id == playerId)
                    .FirstOrDefaultAsync();

                var playerClub = await _context.Club
                    .Where(mo => mo.ClubId == clubId)
                    .FirstOrDefaultAsync();


                if (player == null)
                {
                    TempData["Errors"] = new List<string> { "Player not found." };
                    return RedirectToAction("ErrorPage", "Home", new { errorMessage = "Player not found." + $"PlayerId  = {playerId}" });
                }

                var transferMarket = await _context.PlayerTransferMarket
                    .Where(mo => mo.PlayerTransferMarketId == marketId &&
                    mo.League.IsCurrent)
                    .FirstOrDefaultAsync();

                if (transferMarket == null)
                {
                    TempData["Errors"] = new List<string> { "Transfer market not found." };
                    return RedirectToAction("ErrorPage", "Home", new { errorMessage = "Transfer market not found." });
                }

                if (playerClub == null)
                {
                    TempData["Errors"] = new List<string> { "Player club not found." };
                    return RedirectToAction("ErrorPage", "Home", new { errorMessage = "Player club not found." });
                }

                var viewModel = new InitiatePlayerTransferViewModel
                {
                    LeagueId = transferMarket.LeagueId,
                    MarketId = transferMarket.PlayerTransferMarketId,
                    PlayerId = playerId,
                    SellerClubId = clubId,
                    FirstName = player.FirstName,
                    LastName = player.LastName,
                    Position = player.Position,
                    ProfilePicture = player.ProfilePicture,
                    JerseyNumber = player.JerseyNumber,
                    DateOfBirth = player.DateOfBirth,
                    MarketValue = player.MarketValue,
                    ClubName = playerClub.ClubName,
                    ClubBadge = playerClub.ClubBadge,
                };

                ViewBag.Positions = Enum.GetValues(typeof(Position))
                    .Cast<Position>()
                    .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() });
                return View(viewModel);
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while processing the request: " + ex.Message;
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = errorMessage });
            }
        }


        [Authorize(Roles = ("Club Administrator"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InitiatePlayerTransfer(InitiatePlayerTransferViewModel viewModel)
        {
            try
            {
                var loggedInUser = await _userManager.GetUserAsync(User);

                if (loggedInUser == null || !(loggedInUser is ClubAdministrator clubAdministrator))
                {
                    TempData["Errors"] = new List<string> { "User is not authorized" };
                    return View(viewModel);
                }

                var transferMarket = await _context.PlayerTransferMarket
                    .Where(mo => mo.PlayerTransferMarketId == viewModel.MarketId)
                    .Include(s => s.Player)
                    .Include(s => s.Club)
                    .FirstOrDefaultAsync();

                if (transferMarket == null)
                {
                    TempData["Errors"] = new List<string> { "Transfer market not found" };
                    return RedirectToAction("ErrorPage", "Home", new { errorMessage = "Transfer market not found." });
                }

                if (transferMarket.Player == null)
                {
                    TempData["Errors"] = new List<string> { "Player not found in the transfer market" };
                    return RedirectToAction("ErrorPage", "Home", new { errorMessage = "Player not found in the transfer market." });
                }

                if (transferMarket.Player.ClubId == clubAdministrator.ClubId)
                {
                    TempData["Message"] = $"You can't initiate transfer communication for your own player! Please select other clubs' players.";
                    return RedirectToAction(nameof(TransferMarket));
                }

                var transfer = await _context.Transfer
                    .Where(t => t.PlayerId == viewModel.PlayerId && t.League.IsCurrent)
                    .Include(t => t.Player)
                    .FirstOrDefaultAsync();

                if (transfer != null && transfer.PlayerId == viewModel.PlayerId && transfer.Status == TransferStatus.Accepted)
                {
                    TempData["Message"] = $"You cannot initiate transfer communication for this player since it has been already accepted for another buyer club.";
                    return RedirectToAction(nameof(TransferMarket));
                }

                if (transferMarket.SaleStatus == SaleStatus.Unavailable)
                {
                    TempData["Message"] = $"This player has signed a new contract with another club. Please try signing available players.";
                    return RedirectToAction(nameof(TransferMarket));
                }

                var newPlayerTransfer = new Transfer
                {
                    LeagueId = viewModel.LeagueId,
                    PlayerTransferMarketId = transferMarket.PlayerTransferMarketId,
                    PlayerId = viewModel.PlayerId,
                    CustomerClubId = clubAdministrator.ClubId,
                    SellerClubId = viewModel.SellerClubId,
                    CreatedDateTime = DateTime.Now,
                    CreatedById = loggedInUser.Id,
                    ModifiedById = loggedInUser.Id,
                    ModifiedDateTime = DateTime.Now,
                    Approved_Declined_ById = loggedInUser.Id,
                    Status = TransferStatus.Pending,
                };

                _context.Transfer.Add(newPlayerTransfer);
                await _context.SaveChangesAsync();

                var sellerClubTransferReport = await _context.ClubTransferReports
                    .Where(c => c.ClubId == viewModel.SellerClubId && c.League.IsCurrent)
                    .FirstOrDefaultAsync();

                var buyerClubTransferReport = await _context.ClubTransferReports
                    .Where(c => c.ClubId == clubAdministrator.ClubId && c.League.IsCurrent)
                    .FirstOrDefaultAsync();

                if (sellerClubTransferReport != null)
                {
                    sellerClubTransferReport.IncomingTransfersCount++;
                }

                if (buyerClubTransferReport != null)
                {
                    buyerClubTransferReport.OutgoingTransfersCount++;
                }

                if (sellerClubTransferReport != null || buyerClubTransferReport != null)
                {
                    await _context.SaveChangesAsync();
                }

                var sellerClubAdmin = await _context.ClubAdministrator
                    .Where(u => u.ClubId == viewModel.SellerClubId)
                    .FirstOrDefaultAsync();

                var sellerClub = await _context.Club
                    .Where(c => c.ClubId == viewModel.SellerClubId)
                    .FirstOrDefaultAsync();

                if (sellerClubAdmin != null)
                {
                    var subject = "Player Transfer Request Notification";
                    var emailBodyTemplate = $@"
            Dear Club Administrator,<br/><br/>
            A new transfer request has been initiated for the player <b>{viewModel.FirstName} {viewModel.LastName}</b>.<br/><br/>
            <b>Transfer Details:</b><br/>
            - Market Value: {viewModel.MarketValue:C}<br/>
            - Position: {viewModel.Position}<br/>
            - Club Name: {viewModel.ClubName}<br/><br/>
            Please review the transfer request in your dashboard.<br/><br/>
            If you have any questions or need further assistance, please contact us at support@knsfoundation.com.<br/><br/>
            Regards,<br/>
            K&S Foundation Management
            ";

                    BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(sellerClubAdmin.Email, subject, emailBodyTemplate));
                }

                if (sellerClub != null && !string.IsNullOrEmpty(sellerClub.Email))
                {
                    var clubEmailSubject = "New Player Transfer Request";
                    var clubEmailBodyTemplate = $@"
            Dear {sellerClub.ClubName} Management,<br/><br/>
            A new transfer request has been initiated for the player <b>{viewModel.FirstName} {viewModel.LastName}</b>.<br/><br/>
            <b>Transfer Details:</b><br/>
            - Market Value: {viewModel.MarketValue:C}<br/>
            - Position: {viewModel.Position}<br/>
            - Club Name: {viewModel.ClubName}<br/><br/>
            Please review the transfer request in your dashboard.<br/><br/>
            If you have any questions or need further assistance, please contact us at support@knsfoundation.com.<br/><br/>
            Regards,<br/>
            K&S Foundation Management
            ";

                    BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(sellerClub.Email, clubEmailSubject, clubEmailBodyTemplate));
                }

                TempData["Message"] = $"You have successfully initiated communication with {viewModel.ClubName} for {viewModel.FirstName} {viewModel.LastName}'s transfer.";
                return RedirectToAction(nameof(TransferMarket));
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to process player transfer: " + ex.Message,
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    }
                });
            }
        }


        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> AcceptPlayerTransfer(int transferId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var transfer = await _context.Transfer
                .Where(t => t.League.IsCurrent)
                .Include(t => t.Player)
                .Include(t => t.CustomerClub)
                .FirstOrDefaultAsync(t => t.TransferId == transferId);

            if (transfer == null)
            {
                return NotFound();
            }

            if (loggedInUser != null)
            {
                transfer.Status = TransferStatus.Accepted;
                transfer.paymentTransferStatus = PaymentTransferStatus.Pending_Payment;
                transfer.Approved_Declined_ById = loggedInUser.Id;
                transfer.ModifiedDateTime = DateTime.Now;
            }

            _context.Update(transfer);
            await _context.SaveChangesAsync();

            var customerClubAdmin = await _context.ClubAdministrator
                .Where(ca => ca.ClubId == transfer.CustomerClubId)
                .FirstOrDefaultAsync();

            var customerClub = await _context.Club
                .Where(c => c.ClubId == transfer.CustomerClubId)
                .FirstOrDefaultAsync();

            var subject = "Player Transfer Accepted Notification";
            var emailBodyTemplate = $@"
        Dear {transfer.CustomerClub?.ClubName} Management,<br/><br/>
        The transfer request for player <b>{transfer.Player?.FirstName} {transfer.Player?.LastName}</b> has been accepted.<br/><br/>
        <b>Transfer Details:</b><br/>
        - Player: {transfer.Player?.FirstName} {transfer.Player?.LastName}<br/>
        - Customer Club: {transfer.CustomerClub?.ClubName}<br/><br/>
        Please proceed with the next steps as outlined in your dashboard.<br/><br/>
        If you have any questions or need further assistance, please contact us at support@knsfoundation.com.<br/><br/>
        Regards,<br/>
        K&S Foundation Management
    ";

            if (customerClubAdmin != null && !string.IsNullOrEmpty(customerClubAdmin.Email))
            {
                BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(customerClubAdmin.Email, subject, emailBodyTemplate));
            }

            if (customerClub != null && !string.IsNullOrEmpty(customerClub.Email))
            {
                BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(customerClub.Email, subject, emailBodyTemplate));
            }

            string message = $"You have successfully accepted player transfer for ";

            if (User.IsInRole("Personnel Administrator"))
            {
                if (transfer.Player != null && transfer.CustomerClub != null)
                {
                    message += $"{transfer.Player.FirstName} {transfer.Player.LastName} with {transfer.CustomerClub.ClubName}!";
                }
                else
                {
                    message += "transfer!";
                }

                TempData["Message"] = message;
                return RedirectToAction(nameof(PlayerTransfers));
            }
            else if (User.IsInRole("Club Administrator"))
            {
                if (transfer.Player != null && transfer.CustomerClub != null)
                {
                    message += $"{transfer.Player.FirstName} {transfer.Player.LastName} with {transfer.CustomerClub.ClubName}!";
                }
                else
                {
                    message += "transfer!";
                }

                TempData["Message"] = message;
                return RedirectToAction(nameof(MyTransferRequestsTabs));
            }

            return View();
        }


        [Authorize(Roles = ("Club Administrator"))]
        public async Task<IActionResult> RejectPlayerTransfer(int transferId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var transfer = await _context.Transfer
                .Where(t => t.League.IsCurrent)
                .Include(t => t.Player)
                .Include(t => t.CustomerClub)
                .Include(t => t.SellerClub)
                .FirstOrDefaultAsync(t => t.TransferId == transferId);

            var sellerClubTransferReport = await _context.ClubTransferReports
                .Where(c => c.ClubId == transfer.SellerClub.ClubId &&
                c.League.IsCurrent)
                .Include(c => c.Club)
                .FirstOrDefaultAsync();

            var buyerClubTransferReport = await _context.ClubTransferReports
                .Where(c => c.ClubId == transfer.CustomerClub.ClubId &&
                c.League.IsCurrent)
                .Include(c => c.Club)
                .FirstOrDefaultAsync();

            if (transfer == null)
            {
                return NotFound();
            }

            if (loggedInUser != null)
            {
                transfer.Approved_Declined_ById = loggedInUser.Id;
                transfer.ModifiedDateTime = DateTime.Now;
                transfer.Status = TransferStatus.Rejected;
            }

            buyerClubTransferReport.RejectedOutgoingTransfersCount++;
            sellerClubTransferReport.RejectedIncomingTransfersCount++;

            _context.Update(transfer);
            await _context.SaveChangesAsync();

            var customerClubAdmin = await _context.ClubAdministrator
                .Where(ca => ca.ClubId == transfer.CustomerClub.ClubId)
                .FirstOrDefaultAsync();

            var sellerClubAdmin = await _context.ClubAdministrator
                .Where(ca => ca.ClubId == transfer.SellerClub.ClubId)
                .FirstOrDefaultAsync();

            var subject = "Player Transfer Rejected Notification";
            var emailBodyTemplate = $@"
        Dear {transfer.CustomerClub?.ClubName} Management,<br/><br/>
        The transfer request for player <b>{transfer.Player?.FirstName} {transfer.Player?.LastName}</b> has been rejected.<br/><br/>
        <b>Transfer Details:</b><br/>
        - Player: {transfer.Player?.FirstName} {transfer.Player?.LastName}<br/>
        - Seller Club: {transfer.SellerClub?.ClubName}<br/>
        - Customer Club: {transfer.CustomerClub?.ClubName}<br/><br/>
        Please review the transfer details and contact us if you have any questions.<br/><br/>
        If you need further assistance, please contact us at support@knsfoundation.com.<br/><br/>
        Regards,<br/>
        K&S Foundation Management
    ";

            if (customerClubAdmin != null && !string.IsNullOrEmpty(customerClubAdmin.Email))
            {
                BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(customerClubAdmin.Email, subject, emailBodyTemplate));
            }

            if (sellerClubAdmin != null && !string.IsNullOrEmpty(sellerClubAdmin.Email))
            {
                BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(sellerClubAdmin.Email, subject, emailBodyTemplate));
            }

            string message = $"You have rejected player transfer communication for ";

            if (transfer.Player != null && transfer.CustomerClub != null)
            {
                message += $"{transfer.Player.FirstName} {transfer.Player.LastName} with {transfer.CustomerClub.ClubName}!";
            }
            else
            {
                message += "transfer!";
            }

            TempData["Message"] = message;
            return RedirectToAction(nameof(MyTransferRequestsTabs));
        }

        [Authorize(Roles = ("Club Administrator"))]
        [HttpPost]
        public async Task<IActionResult> CancelPlayerTransfer(int transferId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var transfer = await _context.Transfer
                .Include(t => t.Player)
                .Include(t => t.SellerClub)
                .FirstOrDefaultAsync(t => t.TransferId == transferId);

            if (transfer == null)
            {
                return NotFound();
            }

            if(loggedInUser != null)
            {

            transfer.Approved_Declined_ById = loggedInUser.Id;
            transfer.ModifiedDateTime = DateTime.Now;   
            transfer.Status = TransferStatus.Cancelled;

            }

            _context.Update(transfer);
            await _context.SaveChangesAsync();


            string message = $"You have cancelled player transfer communication for ";
            if (transfer.Player != null && transfer.SellerClub != null)
            {
                message += $"{transfer.Player.FirstName} {transfer.Player.LastName} with {transfer.SellerClub.ClubName}!";
            }
            else
            {
                message += "transfer!";
            }

            TempData["Message"] = message;
            return RedirectToAction(nameof(MyTransfersTabs));
        }





        public async Task<IActionResult> PaymentPlayerTransfer()
        {
            return View();
        }
    }
}
