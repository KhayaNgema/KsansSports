using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Migrations;
using MyField.Models;
using MyField.ViewModels;


namespace MyField.Controllers
{
    public class TransfersController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IMapper _mapper;
        private readonly IActivityLogger _activityLogger;

        public TransfersController(Ksans_SportsDbContext context,
            UserManager<UserBaseModel> userManager,
             IMapper mapper,
             IActivityLogger activityLogger)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _activityLogger = activityLogger;
        }

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


        public async Task<IActionResult> PlayerTransfers()
        {

            var currentSeason = await _context.League
                 .Where(c => c.IsCurrent)
                  .FirstOrDefaultAsync();


            ViewBag.CurrentSeason = currentSeason.LeagueYears;


            return View();
        }

        public async Task<IActionResult> PendingTransfers()
        {
            var pendingTrasfers = await _context.Transfer
                .Where(p => p.Status == TransferStatus.Pending && 
                       p.SellerClub.League.IsCurrent && 
                       p.CustomerClub.League.IsCurrent)
                .Include(p => p.SellerClub)
                .Include(p => p.CustomerClub)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return PartialView("_PendingPlayerTransfersPartial", pendingTrasfers);
        }

        public async Task<IActionResult> PaidTransfers()
        {
            var paidTrasfers = await _context.Transfer
                .Where(p => p.Status == TransferStatus.Completed &&
                       p.SellerClub.League.IsCurrent &&
                       p.CustomerClub.League.IsCurrent)
                .Include(p => p.SellerClub)
                .Include(p => p.CustomerClub)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return PartialView("_PaidPlayerTransfersPartial", paidTrasfers);
        }

        public async Task<IActionResult> RejectedTransfers()
        {
            var rejectedTrasfers = await _context.Transfer
                .Where(p => p.Status == TransferStatus.Rejected &&
                       p.SellerClub.League.IsCurrent &&
                       p.CustomerClub.League.IsCurrent)
                .Include(p => p.SellerClub)
                .Include(p => p.CustomerClub)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return PartialView("_RejectedPlayerTransfersPartial", rejectedTrasfers);
        }

        public async Task<IActionResult> AcceptedTransfers()
        {
            var acceptedTrasfers = await _context.Transfer
                .Where(p => p.Status == TransferStatus.Accepted &&
                       p.SellerClub.League.IsCurrent &&
                       p.CustomerClub.League.IsCurrent)
                .Include(p => p.SellerClub)
                .Include(p => p.CustomerClub)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return PartialView("_AcceptedPlayerTransfersPartial", acceptedTrasfers);
        }

        public async Task<IActionResult> FindPlayerTransferMarket()
        {
            return View();
        }

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

        public async Task<IActionResult> TransferRequests()
        {
            return View();
        }

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
                .Where(mo => mo.Status == TransferStatus.Pending && mo.SellerClub.ClubId == clubId)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                 .Include(s => s.CustomerClub)
                .ToListAsync();

            return PartialView("MyPendingTransfers", pendingTransfers);
        }

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
                mo.CustomerClub.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .Include(s => s.CustomerClub)
                .ToListAsync();

            return PartialView("MyAcceptedTransfers", acceptedTransfers);
        }

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
                mo.CustomerClub.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .Include(s => s.CustomerClub)
                .ToListAsync();

            return PartialView("MyRejectedTransfers", rejectedTransfers);
        }


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
                mo.CustomerClub.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .ToListAsync();

            return PartialView("PendingTransfers", pendingTransfers);
        }

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
                mo.CustomerClub.League.IsCurrent)
                .Include(s => s.Player)
                .Include(s => s.SellerClub)
                .ToListAsync();

            return PartialView("CancelledTransfers", pendingTransfers);
        }

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
                mo.CustomerClub.League.IsCurrent)
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
                mo.CustomerClub.League.IsCurrent)
               .Include(s => s.Player)
               .Include(s => s.SellerClub)
               .ToListAsync();

            return PartialView("RejectedTransfers", rejectedTransfers);
        }

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
                      mo.CustomerClub.League.IsCurrent)
                     .Include(s => s.Player)
                     .Include(s => s.SellerClub)
                     .ToListAsync();

            return PartialView("CompletedTransfers", completedTransfers);
        }



        public async Task<IActionResult> TransferList()
        {
            var transferLists = await _context.Transfer
                .Include(s => s.SellerClub)
                 .Include(s => s.CustomerClub)
                .Include(s => s.CreatedBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.Player)
                .ToListAsync();

            return PartialView(transferLists);
        }

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
                    .Where(mo => mo.PlayerTransferMarketId == marketId)
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


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InitiatePlayerTransfer(InitiatePlayerTransferViewModel viewModel)
        {
            try
            {
                if (ModelState.IsValid)
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
                        TempData["Message"] = $"You can't initiate transfer communication for your own player! Please select other clubs' players";
                        return RedirectToAction(nameof(TransferMarket));
                    }

                    if (transferMarket.SaleStatus == SaleStatus.Unavailable)
                    {
                        TempData["Message"] = $"This player have signed a new contract with another club. Please try signing available players.";
                        return RedirectToAction(nameof(TransferMarket));
                    }

                    var newPlayerTransfer = new Transfer
                    {
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

                    await _context.SaveChangesAsync();

                    _context.Transfer.Add(newPlayerTransfer);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = $"You have successfully initiated communication with {viewModel.ClubName} for {viewModel.FirstName} {viewModel.LastName} transfer";
                    return RedirectToAction(nameof(TransferMarket));
                }
                else
                {
                    var modelStateErrors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    string errorMessage = "Model state is invalid. Errors: " + string.Join("; ", modelStateErrors);
                    return RedirectToAction("ErrorPage", "Home", new { errorMessage = errorMessage });
                }
            }
            catch (Exception ex)
            {
                string errorMessage = "An error occurred while processing your request: " + ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " See the inner exception for details: " + ex.InnerException.Message + $" MarketId: {viewModel.MarketId}";
                }
                return RedirectToAction("ErrorPage", "Home", new { errorMessage = errorMessage });
            }
        }



        [HttpPost]
        public async Task<IActionResult> AcceptPlayerTransfer(int transferId)
        {

            var loggedInUser = await _userManager.GetUserAsync(User);

            var transfer = await _context.Transfer
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


            string message = $"You have successfully accepted player transfer for ";
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPlayerTransfer(int transferId)
        {
            var loggedInUser = await _userManager.GetUserAsync(User);

            var transfer = await _context.Transfer
              .Include(t => t.Player)
              .Include(t => t.CustomerClub)
              .FirstOrDefaultAsync(t => t.TransferId == transferId);

            var transferReport = await _context.TransfersReports
               .Where(t => t.Season.IsCurrent)
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

            transferReport.DeclinedTransfersCount++;

            _context.Update(transfer);
            await _context.SaveChangesAsync();


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


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Transfer == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfer
                .Include(t => t.Approved_Declined_By)
                .Include(t => t.CreatedBy)
                .Include(t => t.CustomerClub)
                .Include(t => t.ModifiedBy)
                .Include(t => t.Player)
                .Include(t => t.SellerClub)
                .FirstOrDefaultAsync(m => m.TransferId == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TransferId,PlayerId,CustomerClubId,SellerClubId,CreatedDateTime,ModifiedDateTime,CreatedById,ModifiedById,Approved_Declined_ById,Status")] Transfer transfer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transfer);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transfer);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Transfer == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfer.FindAsync(id);
            if (transfer == null)
            {
                return NotFound();
            }
            ViewData["Approved_Declined_ById"] = new SelectList(_context.SystemUsers, "Id", "Id", transfer.Approved_Declined_ById);
            ViewData["CreatedById"] = new SelectList(_context.SystemUsers, "Id", "Id", transfer.CreatedById);
            ViewData["CustomerClubId"] = new SelectList(_context.Club, "ClubId", "ClubLocation", transfer.CustomerClubId);
            ViewData["ModifiedById"] = new SelectList(_context.SystemUsers, "Id", "Id", transfer.ModifiedById);
            ViewData["PlayerId"] = new SelectList(_context.Player, "Id", "Id", transfer.PlayerId);
            ViewData["SellerClubId"] = new SelectList(_context.Club, "ClubId", "ClubLocation", transfer.SellerClubId);
            return View(transfer);
        }

        // POST: Transfers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TransferId,PlayerId,CustomerClubId,SellerClubId,CreatedDateTime,ModifiedDateTime,CreatedById,ModifiedById,Approved_Declined_ById,Status")] Transfer transfer)
        {
            if (id != transfer.TransferId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transfer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransferExists(transfer.TransferId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Approved_Declined_ById"] = new SelectList(_context.SystemUsers, "Id", "Id", transfer.Approved_Declined_ById);
            ViewData["CreatedById"] = new SelectList(_context.SystemUsers, "Id", "Id", transfer.CreatedById);
            ViewData["CustomerClubId"] = new SelectList(_context.Club, "ClubId", "ClubLocation", transfer.CustomerClubId);
            ViewData["ModifiedById"] = new SelectList(_context.SystemUsers, "Id", "Id", transfer.ModifiedById);
            ViewData["PlayerId"] = new SelectList(_context.Player, "Id", "Id", transfer.PlayerId);
            ViewData["SellerClubId"] = new SelectList(_context.Club, "ClubId", "ClubLocation", transfer.SellerClubId);
            return View(transfer);
        }

        // GET: Transfers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Transfer == null)
            {
                return NotFound();
            }

            var transfer = await _context.Transfer
                .Include(t => t.Approved_Declined_By)
                .Include(t => t.CreatedBy)
                .Include(t => t.CustomerClub)
                .Include(t => t.ModifiedBy)
                .Include(t => t.Player)
                .Include(t => t.SellerClub)
                .FirstOrDefaultAsync(m => m.TransferId == id);
            if (transfer == null)
            {
                return NotFound();
            }

            return View(transfer);
        }

        // POST: Transfers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transfer == null)
            {
                return Problem("Entity set 'Ksans_SportsDbContext.Transfer'  is null.");
            }
            var transfer = await _context.Transfer.FindAsync(id);
            if (transfer != null)
            {
                _context.Transfer.Remove(transfer);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransferExists(int id)
        {
            return (_context.Transfer?.Any(e => e.TransferId == id)).GetValueOrDefault();
        }
    }
}
