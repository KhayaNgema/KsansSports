using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Models;
using MyField.ViewModels;
using MyField.Services;
using Microsoft.AspNetCore.Identity;
using MyField.Migrations;
using System.ComponentModel.DataAnnotations;

namespace MyField.Controllers
{
    public class UsersController : Controller
    {
        public readonly Ksans_SportsDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<UserBaseModel> _userManager;

        public UsersController(Ksans_SportsDbContext context,
           FileUploadService fileUploadService,
           UserManager<UserBaseModel> userManager)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
        }

        public async Task<IActionResult> SystemUsers()
        {
            var systemUsers = await _context.Users
                .ToListAsync();

            return View(systemUsers);
        }

        public async Task<IActionResult> SportMembers()
        {
            var sportMembers = await _context.SportMember
                .ToListAsync();

            return View(sportMembers);
        }

        public async Task<IActionResult> MyClubPlayers()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return RedirectToAction("Error", "Home");
            }

            var clubPlayers = await _context.Player
                .Where(p => p.ClubId == clubAdministrator.ClubId)
                .Include(s => s.Club)
                .ToListAsync();

            ViewBag.ClubName = clubAdministrator?.Club?.ClubName;

            return View(clubPlayers);
        }

        public async Task<IActionResult> MyClubManagers()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return RedirectToAction("Error", "Home");
            }

            var clubManagers = await _context.ClubManager
                .Where(p => p.ClubId == clubAdministrator.ClubId)
                .Include(s => s.Club)
                .ToListAsync();

            ViewBag.ClubName = clubAdministrator?.Club?.ClubName;

            return PartialView(clubManagers);
        }

        public async Task<IActionResult> ClubPlayers()
        {
            var clubPlayers = await _context.Player
                .Include(s => s.Club)
                .ToListAsync();

            return View(clubPlayers);
        }

        public async Task<IActionResult> MyClubAdministrators()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return RedirectToAction("Error", "Home");
            }

            var clubAdministrators = await _context.ClubAdministrator
                .Where(p => p.ClubId == clubAdministrator.ClubId)
                .Include(s => s.Club)
                .ToListAsync();

            ViewBag.ClubName = clubAdministrator?.Club?.ClubName;

            return View(clubAdministrators);
        }

        public async Task<IActionResult> Officials()
        {
            var officials = await _context.Officials
                .ToListAsync();

            return View(officials);
        }

        public async Task<IActionResult> DeActivateUser()
        {
            return View();
        }

        public async Task<IActionResult> ActivateUser()
        {
            return View();
        }

        public async Task<IActionResult> RemoveUser()
        {
            return View();
        }

        public async Task<IActionResult> ChangeUserRole()
        {
            return View();
        }


        /*Update all users methods*/

        [HttpGet]
        public async Task<IActionResult> UpdateClubManager(string id)
        {
            var logMessages = new List<string>();

            if (id == null || _context.ClubManager == null)
            {
                logMessages.Add($"UpdateClubManager GET request failed: id is null or ClubManager context is null");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubManager = await _context.ClubManager.FindAsync(id);
            if (clubManager == null)
            {
                logMessages.Add($"ClubManager with id {id} not found during GET request");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubManagerViewModel = new UpdateClubManagerViewModel
            {
                Id = clubManager.Id,
                FirstName = clubManager.FirstName,
                LastName = clubManager.LastName,
                PhoneNumber = clubManager.PhoneNumber,
                ProfilePicture = clubManager.ProfilePicture,
                Email = clubManager.Email,
            };

            TempData["LogMessages"] = logMessages;
            return View(clubManagerViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClubManager(string id, UpdateClubManagerViewModel viewModel, IFormFile ProfilePictureFile)
        {
            var logMessages = new List<string>();

            if (id != viewModel.Id)
            {
                logMessages.Add($"UpdateClubManager POST request failed: id mismatch: {id} != {viewModel.Id}");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var clubManager = await _context.ClubManager.FindAsync(id);

            if (ValidateUpdatedProperties(viewModel))
            {

                try
                {

                    if (clubManager == null)
                    {
                        logMessages.Add($"ClubManager with id {id} not found during POST request");
                        TempData["LogMessages"] = logMessages;
                        return NotFound();
                    }

                    clubManager.FirstName = viewModel.FirstName;
                    clubManager.LastName = viewModel.LastName;
                    clubManager.PhoneNumber = viewModel.PhoneNumber;
                    clubManager.Email = viewModel.Email;
                    clubManager.UserName = viewModel.Email;
                    clubManager.NormalizedEmail = viewModel.Email;
                    clubManager.NormalizedUserName = viewModel.Email;
                    clubManager.ModifiedBy = userId;
                    clubManager.ModifiedDateTime = DateTime.Now;

                    if (ProfilePictureFile != null && ProfilePictureFile.Length > 0)
                    {
                        var uploadedImagePath = await _fileUploadService.UploadFileAsync(ProfilePictureFile);
                        clubManager.ProfilePicture = uploadedImagePath;
                    }

                    _context.Update(clubManager);
                    await _context.SaveChangesAsync();
                    logMessages.Add($"ClubManager with id {id} successfully updated");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    logMessages.Add($"DbUpdateConcurrencyException occurred: {ex.Message}");
                    TempData["LogMessages"] = logMessages;
                    throw;
                }

                TempData["Message"] = $"{clubManager.FirstName} {clubManager.LastName}  information has been updated successfully.";
                return RedirectToAction(nameof(MyClubManagers));
            }

            logMessages.Add($"Model state invalid for ClubManager with id {id}");

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    var errorMessage = $"Error in {state.Key}: {error.ErrorMessage}";
                    logMessages.Add(errorMessage);
                }
            }

            viewModel.ProfilePicture = _context.ClubManager.Find(id)?.ProfilePicture;

            TempData["LogMessages"] = logMessages;
            viewModel.ProfilePicture = clubManager.ProfilePicture;
            return View(viewModel);
        }

        private bool ValidateUpdatedProperties(UpdateClubManagerViewModel viewModel)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(viewModel.FirstName, new ValidationContext(viewModel, null, null) { MemberName = "FirstName" }, validationResults);
            Validator.TryValidateProperty(viewModel.LastName, new ValidationContext(viewModel, null, null) { MemberName = "LastName" }, validationResults);
            Validator.TryValidateProperty(viewModel.PhoneNumber, new ValidationContext(viewModel, null, null) { MemberName = "PhoneNumber" }, validationResults);
            Validator.TryValidateProperty(viewModel.Email, new ValidationContext(viewModel, null, null) { MemberName = "Email" }, validationResults);
            return validationResults.Count == 0;
        }




        [HttpGet]
        public async Task<IActionResult> UpdateClubPlayer(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var clubPlayer = await _context.Player.FindAsync(id);
            if (clubPlayer == null)
            {
                return NotFound();
            }

            var viewModel = new UpdateClubPlayerViewModel
            {
                Id = clubPlayer.Id,
                FirstName = clubPlayer.FirstName,
                LastName = clubPlayer.LastName,
                PhoneNumber = clubPlayer.PhoneNumber,
                Email = clubPlayer.Email,
                Position = clubPlayer.Position,
                JerseyNumber = clubPlayer.JerseyNumber,
                ProfilePicture = clubPlayer.ProfilePicture,
                MarketValue = clubPlayer.MarketValue
            };

            ViewBag.Positions = Enum.GetValues(typeof(Position))
                                     .Cast<Position>()
                                     .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() });

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClubPlayer(string id, UpdateClubPlayerViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (ValidateUpdatedProperties(viewModel))
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);
                    var userId = user.Id;

                    var clubPlayer = await _context.Player.FindAsync(id);
                    if (clubPlayer == null)
                    {
                        return NotFound();
                    }

                    clubPlayer.FirstName = viewModel.FirstName;
                    clubPlayer.LastName = viewModel.LastName;
                    clubPlayer.PhoneNumber = viewModel.PhoneNumber;
                    clubPlayer.Email = viewModel.Email;
                    clubPlayer.Position = viewModel.Position;
                    clubPlayer.JerseyNumber = viewModel.JerseyNumber;
                    clubPlayer.UserName = viewModel.Email;
                    clubPlayer.NormalizedEmail = viewModel.Email;
                    clubPlayer.NormalizedUserName = viewModel.Email;
                    clubPlayer.ModifiedBy = userId;
                    clubPlayer.ModifiedDateTime = DateTime.Now;
                    clubPlayer.MarketValue =  viewModel.MarketValue;    

                    if (viewModel.ProfilePictureFile != null && viewModel.ProfilePictureFile.Length > 0)
                    {
                        var playerProfilePicturePath = await _fileUploadService.UploadFileAsync(viewModel.ProfilePictureFile);
                        clubPlayer.ProfilePicture = playerProfilePicturePath;
                    }

                    _context.Update(clubPlayer);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = $"{clubPlayer.FirstName} {clubPlayer.LastName}  information has been updated successfully.";
                    return RedirectToAction("MyClubPlayers");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubPlayerExists(viewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            ViewBag.Positions = Enum.GetValues(typeof(Position))
                                     .Cast<Position>()
                                     .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() });

            return View(viewModel);
        }

        private bool ValidateUpdatedProperties(UpdateClubPlayerViewModel viewModel)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(viewModel.FirstName, new ValidationContext(viewModel, null, null) { MemberName = "FirstName" }, validationResults);
            Validator.TryValidateProperty(viewModel.LastName, new ValidationContext(viewModel, null, null) { MemberName = "LastName" }, validationResults);
            Validator.TryValidateProperty(viewModel.PhoneNumber, new ValidationContext(viewModel, null, null) { MemberName = "PhoneNumber" }, validationResults);
            Validator.TryValidateProperty(viewModel.Email, new ValidationContext(viewModel, null, null) { MemberName = "Email" }, validationResults);
            Validator.TryValidateProperty(viewModel.Position, new ValidationContext(viewModel, null, null) { MemberName = "Position" }, validationResults);
            Validator.TryValidateProperty(viewModel.JerseyNumber, new ValidationContext(viewModel, null, null) { MemberName = "JerseyNumber" }, validationResults);
            Validator.TryValidateProperty(viewModel.MarketValue, new ValidationContext(viewModel, null, null) { MemberName = "MarketValue" }, validationResults);
            return validationResults.Count == 0;
        }

        private bool ClubPlayerExists(string id)
        {
            return _context.Player.Any(e => e.Id == id);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateClubAdministrator(string id)
        {
            var logMessages = new List<string>();

            if (id == null || _context.ClubAdministrator == null)
            {
                logMessages.Add($"UpdateClubManager GET request failed: id is null or ClubManager context is null");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubAdministrator = await _context.ClubAdministrator.FindAsync(id);
            if (clubAdministrator == null)
            {
                logMessages.Add($"ClubManager with id {id} not found during GET request");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubManagerViewModel = new UpdateClubAdministratorViewModel
            {
                Id = clubAdministrator.Id,
                FirstName = clubAdministrator.FirstName,
                LastName = clubAdministrator.LastName,
                PhoneNumber = clubAdministrator.PhoneNumber,
                ProfilePicture = clubAdministrator.ProfilePicture,
                Email = clubAdministrator.Email,
            };

            TempData["LogMessages"] = logMessages;
            return View(clubManagerViewModel);
        }




        [HttpPost]
        public async Task<IActionResult> UpdateClubAdministrator(string id, UpdateClubAdministratorViewModel viewModel, IFormFile ProfilePictureFile)
        {
            var logMessages = new List<string>();

            if (id != viewModel.Id)
            {
                logMessages.Add($"UpdateClubManager POST request failed: id mismatch: {id} != {viewModel.Id}");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var clubAdministrator = await _context.ClubAdministrator.FindAsync(id);

            if (ValidateUpdatedProperties(viewModel))
            {

                try
                {

                    if (clubAdministrator == null)
                    {
                        logMessages.Add($"ClubManager with id {id} not found during POST request");
                        TempData["LogMessages"] = logMessages;
                        return NotFound();
                    }

                    clubAdministrator.FirstName = viewModel.FirstName;
                    clubAdministrator.LastName = viewModel.LastName;
                    clubAdministrator.PhoneNumber = viewModel.PhoneNumber;
                    clubAdministrator.Email = viewModel.Email;
                    clubAdministrator.UserName = viewModel.Email;
                    clubAdministrator.NormalizedUserName = viewModel.Email;
                    clubAdministrator.NormalizedEmail = viewModel.Email;
                    clubAdministrator.ModifiedBy = userId;
                    clubAdministrator.ModifiedDateTime = DateTime.Now;

                    if (ProfilePictureFile != null && ProfilePictureFile.Length > 0)
                    {
                        var uploadedImagePath = await _fileUploadService.UploadFileAsync(ProfilePictureFile);
                        clubAdministrator.ProfilePicture = uploadedImagePath;
                    }

                    _context.Update(clubAdministrator);
                    await _context.SaveChangesAsync();
                    logMessages.Add($"ClubManager with id {id} successfully updated");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    logMessages.Add($"DbUpdateConcurrencyException occurred: {ex.Message}");
                    TempData["LogMessages"] = logMessages;
                    throw;
                }

                TempData["LogMessages"] = logMessages;
                TempData["Message"] = $"{clubAdministrator.FirstName} {clubAdministrator.LastName}  information has been updated successfully.";
                return RedirectToAction(nameof(MyClubAdministrators));
            }

            logMessages.Add($"Model state invalid for ClubManager with id {id}");

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    var errorMessage = $"Error in {state.Key}: {error.ErrorMessage}";
                    logMessages.Add(errorMessage);
                }
            }

            viewModel.ProfilePicture = _context.ClubAdministrator.Find(id)?.ProfilePicture;

            TempData["LogMessages"] = logMessages;
            viewModel.ProfilePicture = clubAdministrator.ProfilePicture;
            return View(viewModel);
        }

        private bool ClubAdministratorExists(string id)
        {
            return _context.Player.Any(e => e.Id == id);
        }

        private bool ValidateUpdatedProperties(UpdateClubAdministratorViewModel viewModel)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(viewModel.FirstName, new ValidationContext(viewModel, null, null) { MemberName = "FirstName" }, validationResults);
            Validator.TryValidateProperty(viewModel.LastName, new ValidationContext(viewModel, null, null) { MemberName = "LastName" }, validationResults);
            Validator.TryValidateProperty(viewModel.PhoneNumber, new ValidationContext(viewModel, null, null) { MemberName = "PhoneNumber" }, validationResults);
            Validator.TryValidateProperty(viewModel.Email, new ValidationContext(viewModel, null, null) { MemberName = "Email" }, validationResults);
            return validationResults.Count == 0;
        }


    }
}
