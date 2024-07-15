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
using MyField.Interfaces;

namespace MyField.Controllers
{
    public class UsersController : Controller
    {
        public readonly Ksans_SportsDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;

        public UsersController(Ksans_SportsDbContext context,
           FileUploadService fileUploadService,
           UserManager<UserBaseModel> userManager,
           IActivityLogger activityLogger)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
            _activityLogger = activityLogger;
        }

        [HttpGet]
        public async Task<IActionResult> UserProfile(string? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserBaseModel
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var viewModel = new ProfileViewModel
            {
                UserId = userId,
                Names = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                Phone = userProfile.PhoneNumber,
                ProfilePicture = userProfile.ProfilePicture,
                DateOfBirth = userProfile.DateOfBirth
            };

            var userRole = await _context.UserRoles
                 .Where(ur => ur.UserId == userId)
                 .Join(_context.Roles,
                 ur => ur.RoleId,
                 r => r.Id,
                 (ur, r) => r.Name)
                 .FirstOrDefaultAsync();

            ViewBag.UserRole = userRole;

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PlayerProfile(string? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var userProfile = await _context.Player
                .Where(u => u.Id == userId)
                .Include(u => u.Club)
                .FirstOrDefaultAsync();

            var viewModel = new PlayerProfileViewModel
            {
                UserId = userProfile.Id,
                Names = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                Phone = userProfile.PhoneNumber,
                ProfilePicture = userProfile.ProfilePicture,
                JerseyNumber = userProfile.JerseyNumber,
                Position = userProfile.Position,
                DateOfBirth = userProfile.DateOfBirth,
            };

            var userRole = await _context.UserRoles
                 .Where(ur => ur.UserId == userId)
                 .Join(_context.Roles,
                 ur => ur.RoleId,
                 r => r.Id,
                 (ur, r) => r.Name)
                 .FirstOrDefaultAsync();

            var club = userProfile.Club;

            ViewBag.Club = club.ClubName;    

            ViewBag.UserRole = userRole;

            return View(viewModel);
        }


        [HttpGet]
        public async Task<IActionResult> Profile(string? userId)
        {
            if(userId == null)
            {
                return NotFound();
            }

            var userProfile = await _context.UserBaseModel
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var viewModel = new ProfileViewModel
            {
                UserId = userId,
                Names = userProfile.FirstName,
                LastName = userProfile.LastName,
                Email = userProfile.Email,
                Phone  = userProfile.PhoneNumber,
                ProfilePicture = userProfile.ProfilePicture,
                DateOfBirth = userProfile.DateOfBirth
            };

            var userRole = await _context.UserRoles
                 .Where(ur => ur.UserId == userId)
                 .Join(_context.Roles,
                 ur => ur.RoleId,
                 r => r.Id,
                 (ur, r) => r.Name)
                 .FirstOrDefaultAsync();

            if (userRole == "Club Administrator")
            {
                var clubAdministrator = _context.ClubAdministrator
                .Include(c => c.Club)
                .Where(c => c.Id == userId)
                .FirstOrDefault();
                ViewBag.Club = clubAdministrator?.Club?.ClubName;
            }
            else if (userRole == "Club Manager")
            {
                var clubManager = _context.ClubManager
               .Include(p => p.Club)
               .Where(c => c.Id == userId)
               .FirstOrDefault();
                ViewBag.Club = clubManager?.Club?.ClubName;
            }

            ViewBag.UserRole = userRole;

            return View(viewModel);
        }

        public async Task<IActionResult> DivisionFans()
        {
            var divisionFans = await _context.Fans
                .ToListAsync();

            return View(divisionFans);
        }

        public async Task<IActionResult> SportAdministrators()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Sport Administrator");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var sportAdministrators = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) &&
                u.IsDeleted == false)
                .ToListAsync();

            return View(sportAdministrators);
        }

        public async Task<IActionResult> SportManagers()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Sport Manager");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var sportManagers = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) &&
                u.IsDeleted == false)
                .ToListAsync();

            return View(sportManagers);
        }

        public async Task<IActionResult> SportCoordinators()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Sport Coordinator");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var sportCoordinators = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) &&
                 u.IsDeleted == false)
                .ToListAsync();

            return View(sportCoordinators);
        }

        public async Task<IActionResult> Officials()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Official");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var officials = await _context.Officials
                .Where(u => userIds.Contains(u.Id) &&
                u.IsDeleted == false)
                .ToListAsync();

            return View(officials);
        }

        public async Task<IActionResult> ClubAdministrators()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Club administrator");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var clubAdministrators = await _context.ClubAdministrator
                .Where(u => userIds.Contains(u.Id) &&
                 u.IsDeleted == false)
                .Include( u => u.Club)
                .ToListAsync();

            return View(clubAdministrators);
        }

        public async Task<IActionResult> ClubManagers()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Club manager");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var clubManagers = await _context.ClubManager
                .Where(u => userIds.Contains(u.Id) &&
                u.IsDeleted == false)
                .Include(u => u.Club)
                .ToListAsync();

            return View(clubManagers);
        }

        public async Task<IActionResult> DivisionPlayers()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Player");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var divisionPlayers = await _context.Player
                .Where(u => userIds.Contains(u.Id) &&
                u.IsDeleted == false)
                .Include(u => u.Club)
                .ToListAsync();

            return View(divisionPlayers);
        }


        public async Task<IActionResult> NewsAdministrators()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "News administrator");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var newsAdministrators = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) &&
                u.IsDeleted == false)
                .ToListAsync();

            return View(newsAdministrators);
        }

        public async Task<IActionResult> NewsUpdaters()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "News updator");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var newsUpdaters = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) &&
                u.IsDeleted == false)
                .ToListAsync();

            return View(newsUpdaters);
        }

        public async Task<IActionResult> FansAdministrators()
        {
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.Name == "Fans administrator");
            var userIds = await _context.UserRoles.Where(ur => ur.RoleId == role.Id).Select(ur => ur.UserId).ToListAsync();

            var fansAdministrators = await _context.SportMember
                .Where(u => userIds.Contains(u.Id) &&
                u.IsDeleted == false)
                .ToListAsync();

            return View(fansAdministrators);
        }


        public async Task<IActionResult> SystemUsers()
        {
            var systemUsers = await _context.Users
                .ToListAsync();

            return View(systemUsers);
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
                .Where(p => p.ClubId == clubAdministrator.ClubId &&
                p.IsDeleted == false)
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
                .Where(p => p.ClubId == clubAdministrator.ClubId &&
                p.IsDeleted == false)
                .Include(s => s.Club)
                .ToListAsync();

            ViewBag.ClubName = clubAdministrator?.Club?.ClubName;

            return PartialView(clubManagers);
        }

        public async Task<IActionResult> ClubPlayers()
        {
            var clubPlayers = await _context.Player
                .Where(s => s.IsDeleted == false)
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
                .Where(p => p.ClubId == clubAdministrator.ClubId && 
                p.IsDeleted == false)
                .Include(s => s.Club)
                .ToListAsync();

            ViewBag.ClubName = clubAdministrator?.Club?.ClubName;

            return View(clubAdministrators);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateProfile(string? userId)
        {
            var logMessages = new List<string>();

            if (userId == null || _context.UserBaseModel == null)
            {
                logMessages.Add($"UpdateClubManager GET request failed: id is null or ClubManager context is null");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var userProfile = await _context.UserBaseModel.FindAsync(userId);

            if (userProfile == null)
            {
                logMessages.Add($"ClubManager with id {userId} not found during GET request");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubManagerViewModel = new UpdateProfileViewModel
            {
                Id = userProfile.Id,
                FirstName = userProfile.FirstName,
                LastName = userProfile.LastName,
                PhoneNumber = userProfile.PhoneNumber,
                ProfilePicture = userProfile.ProfilePicture,
                Email = userProfile.Email,
            };

            TempData["LogMessages"] = logMessages;
            return View(clubManagerViewModel);
        }




        [HttpPost]
        public async Task<IActionResult> UpdateProfile(string userId, UpdateProfileViewModel viewModel, IFormFile ProfilePictureFile)
        {
            var logMessages = new List<string>();

            if (userId != viewModel.Id)
            {
                logMessages.Add($"UpdateClubManager POST request failed: id mismatch: {userId} != {viewModel.Id}");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var userProfile = await _context.UserBaseModel.FindAsync(userId);


            var userRole = await _context.UserRoles
                  .Where(ur => ur.UserId == viewModel.Id)
                  .Join(_context.Roles,
                   ur => ur.RoleId,
                   r => r.Id,
                   (ur, r) => r.Name)
                  .FirstOrDefaultAsync();

            if (ValidateProfileUpdatedProperties(viewModel))
            {

                try
                {

                    if (userProfile == null)
                    {
                        logMessages.Add($"ClubManager with id {userId} not found during POST request");
                        TempData["LogMessages"] = logMessages;
                        return NotFound();
                    }

                    userProfile.FirstName = viewModel.FirstName;
                    userProfile.LastName = viewModel.LastName;
                    userProfile.PhoneNumber = viewModel.PhoneNumber;
                    userProfile.Email = viewModel.Email;
                    userProfile.UserName = viewModel.Email;
                    userProfile.NormalizedUserName = viewModel.Email;
                    userProfile.NormalizedEmail = viewModel.Email;
                    userProfile.ModifiedBy = user.Id;
                    userProfile.ModifiedDateTime = DateTime.Now;

                    if (ProfilePictureFile != null && ProfilePictureFile.Length > 0)
                    {
                        var uploadedImagePath = await _fileUploadService.UploadFileAsync(ProfilePictureFile);
                        userProfile.ProfilePicture = uploadedImagePath;
                    }

                    _context.Update(userProfile);
                    await _context.SaveChangesAsync();
                    logMessages.Add($"ClubManager with id {userId} successfully updated");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    logMessages.Add($"DbUpdateConcurrencyException occurred: {ex.Message}");
                    TempData["LogMessages"] = logMessages;
                    throw;
                }

                TempData["LogMessages"] = logMessages;

                await _activityLogger.Log($"Updated {userProfile.FirstName} {userProfile.LastName} profile information", user.Id);

                TempData["Message"] = $"{userProfile.FirstName} {userProfile.LastName}  information has been updated successfully.";

                if (userRole == "Club Administrator")
                {
                    return RedirectToAction(nameof(ClubAdministrators));
                }
                else if (userRole == "Player")
                {
                    return RedirectToAction(nameof(DivisionPlayers));
                }
                else if (userRole == "Sport Administrator")
                {
                    return RedirectToAction(nameof(SportAdministrators));
                }
                else if (userRole == "Sport manager")
                {
                    return RedirectToAction(nameof(SportManagers));
                }
                else if (userRole == "Sport Coordinator")
                {
                    return RedirectToAction(nameof(SportCoordinators));
                }
                else if (userRole == "Official")
                {
                    return RedirectToAction(nameof(Officials));
                }
                else if (userRole == "Club Manager")
                {
                    return RedirectToAction(nameof(ClubManagers));
                }
                else if (userRole == "News Administrator")
                {
                    return RedirectToAction(nameof(NewsAdministrators));
                }
                else if (userRole == "News Updator")
                {
                    return RedirectToAction(nameof(NewsUpdaters));
                }
                else if (userRole == "Fans Administrator")
                {
                    return RedirectToAction(nameof(FansAdministrators));
                }
                else
                {
                    return RedirectToAction(nameof(DivisionFans));
                }
            }

            logMessages.Add($"Model state invalid for ClubManager with id {userId}");

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    var errorMessage = $"Error in {state.Key}: {error.ErrorMessage}";
                    logMessages.Add(errorMessage);
                }
            }

            viewModel.ProfilePicture = _context.ClubAdministrator.Find(userId)?.ProfilePicture;

            TempData["LogMessages"] = logMessages;
            viewModel.ProfilePicture = userProfile.ProfilePicture;
            return View(viewModel);
        }

        private bool ClubAdministratorExists(string userId)
        {
            return _context.Player.Any(e => e.Id == userId);
        }

        private bool ValidateProfileUpdatedProperties(UpdateProfileViewModel viewModel)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(viewModel.FirstName, new ValidationContext(viewModel, null, null) { MemberName = "FirstName" }, validationResults);
            Validator.TryValidateProperty(viewModel.LastName, new ValidationContext(viewModel, null, null) { MemberName = "LastName" }, validationResults);
            Validator.TryValidateProperty(viewModel.PhoneNumber, new ValidationContext(viewModel, null, null) { MemberName = "PhoneNumber" }, validationResults);
            Validator.TryValidateProperty(viewModel.Email, new ValidationContext(viewModel, null, null) { MemberName = "Email" }, validationResults);
            return validationResults.Count == 0;
        }


        public async Task<IActionResult> UpdateUserProfile(string? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            return View();
        }


        public async Task<IActionResult> Deactivate(string? userId)
        {
            if(userId == null)
            {
                return NotFound();
            }

            var loggedInUser = await _userManager.GetUserAsync(User);

            var user = await _context.UserBaseModel
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var userRole = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_context.Roles,
                      ur => ur.RoleId,
                      r => r.Id,
                      (ur, r) => r.Name)
                .FirstOrDefaultAsync();


            user.IsActive = false;
            user.ModifiedBy = loggedInUser.Id;
            user.ModifiedDateTime = DateTime.Now;

            await _context.SaveChangesAsync();


            await _activityLogger.Log($"Deactivated {user.FirstName} {user.LastName} profile.", user.Id);

            TempData["Message"] = $"You have deactivated {user.FirstName} {user.LastName} and they won't be able to use some system features until you activate them.";

            if(userRole == "Club Administrator")
            {
                return RedirectToAction(nameof(ClubAdministrators));
            }
            else if(userRole == "Player")
            {
                return RedirectToAction(nameof(DivisionPlayers));
            }
            else if (userRole == "Sport Administrator")
            {
                return RedirectToAction(nameof(SportAdministrators));
            }
            else if (userRole == "Sport manager")
            {
                return RedirectToAction(nameof(SportManagers));
            }
            else if (userRole == "Sport Coordinator")
            {
                return RedirectToAction(nameof(SportCoordinators));
            }
            else if (userRole == "Official")
            {
                return RedirectToAction(nameof(Officials));
            }
            else if (userRole == "Club Manager")
            {
                return RedirectToAction(nameof(ClubManagers));
            }
            else if (userRole == "News Administrator")
            {
                return RedirectToAction(nameof(NewsAdministrators));
            }
            else if (userRole == "News Updator")
            {
                return RedirectToAction(nameof(NewsUpdaters));
            }
            else if (userRole == "Fans Administrator")
            {
                return RedirectToAction(nameof(FansAdministrators));
            }
            else
            {
                return RedirectToAction(nameof(DivisionFans));
            }

        }

        public async Task<IActionResult> Activate(string? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var loggedInUser = await _userManager.GetUserAsync(User);

            var user = await _context.UserBaseModel
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var userRole = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_context.Roles,
                      ur => ur.RoleId,
                      r => r.Id,
                      (ur, r) => r.Name)
                .FirstOrDefaultAsync();


            user.IsActive = true;
            user.ModifiedBy = loggedInUser.Id;
            user.ModifiedDateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Activated {user.FirstName} {user.LastName} profile.", user.Id);

            TempData["Message"] = $"You have successfully activated {user.FirstName} {user.LastName} and now they have access into this system features.";

            if (userRole == "Club Administrator")
            {
                return RedirectToAction(nameof(ClubAdministrators));
            }
            else if (userRole == "Player")
            {
                return RedirectToAction(nameof(DivisionPlayers));
            }
            else if (userRole == "Sport Administrator")
            {
                return RedirectToAction(nameof(SportAdministrators));
            }
            else if (userRole == "Sport manager")
            {
                return RedirectToAction(nameof(SportManagers));
            }
            else if (userRole == "Sport Coordinator")
            {
                return RedirectToAction(nameof(SportCoordinators));
            }
            else if (userRole == "Official")
            {
                return RedirectToAction(nameof(Officials));
            }
            else if (userRole == "Club Manager")
            {
                return RedirectToAction(nameof(ClubManagers));
            }
            else if (userRole == "News Administrator")
            {
                return RedirectToAction(nameof(NewsAdministrators));
            }
            else if (userRole == "News Updator")
            {
                return RedirectToAction(nameof(NewsUpdaters));
            }
            else if (userRole == "Fans Administrator")
            {
                return RedirectToAction(nameof(FansAdministrators));
            }
            else
            {
                return RedirectToAction(nameof(DivisionFans));
            }

        }

        public async Task<IActionResult> Suspend(string? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var loggedInUser = await _userManager.GetUserAsync(User);

            var user = await _context.UserBaseModel
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var userRole = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_context.Roles,
                      ur => ur.RoleId,
                      r => r.Id,
                      (ur, r) => r.Name)
                .FirstOrDefaultAsync();


            user.IsSuspended = true;
            user.ModifiedBy = loggedInUser.Id;
            user.ModifiedDateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Suspended {user.FirstName} {user.LastName} profile.", user.Id);

            TempData["Message"] = $"You have suspended {user.FirstName} {user.LastName} and they won't be able to use the entire system until you unsuspend them.";

            if (userRole == "Club Administrator")
            {
                return RedirectToAction(nameof(ClubAdministrators));
            }
            else if (userRole == "Player")
            {
                return RedirectToAction(nameof(DivisionPlayers));
            }
            else if (userRole == "Sport Administrator")
            {
                return RedirectToAction(nameof(SportAdministrators));
            }
            else if (userRole == "Sport manager")
            {
                return RedirectToAction(nameof(SportManagers));
            }
            else if (userRole == "Sport Coordinator")
            {
                return RedirectToAction(nameof(SportCoordinators));
            }
            else if (userRole == "Official")
            {
                return RedirectToAction(nameof(Officials));
            }
            else if (userRole == "Club Manager")
            {
                return RedirectToAction(nameof(ClubManagers));
            }
            else if (userRole == "News Administrator")
            {
                return RedirectToAction(nameof(NewsAdministrators));
            }
            else if (userRole == "News Updator")
            {
                return RedirectToAction(nameof(NewsUpdaters));
            }
            else if (userRole == "Fans Administrator")
            {
                return RedirectToAction(nameof(FansAdministrators));
            }
            else
            {
                return RedirectToAction(nameof(DivisionFans));
            }

        }

        public async Task<IActionResult> Unsuspend(string? userId)
        {
            if (userId == null)
            {
                return NotFound();
            }
            var loggedInUser = await _userManager.GetUserAsync(User);


            var user = await _context.UserBaseModel
                .Where(u => u.Id == userId)
                .FirstOrDefaultAsync();

            var userRole = await _context.UserRoles
                .Where(ur => ur.UserId == userId)
                .Join(_context.Roles,
                      ur => ur.RoleId,
                      r => r.Id,
                      (ur, r) => r.Name)
                .FirstOrDefaultAsync();


            user.IsSuspended = false;
            user.ModifiedBy = loggedInUser.Id;
            user.ModifiedDateTime = DateTime.Now;

            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Unsuspended {user.FirstName} {user.LastName} profile.", user.Id);

            TempData["Message"] = $"You have successfully unsuspended {user.FirstName} {user.LastName} and now they have access into features of this system.";

            if (userRole == "Club  Administrator")
            {
                return RedirectToAction(nameof(ClubAdministrators));
            }
            else if (userRole == "Player")
            {
                return RedirectToAction(nameof(DivisionPlayers));
            }
            else if (userRole == "Sport Administrator")
            {
                return RedirectToAction(nameof(SportAdministrators));
            }
            else if (userRole == "Sport manager")
            {
                return RedirectToAction(nameof(SportManagers));
            }
            else if (userRole == "Sport Coordinator")
            {
                return RedirectToAction(nameof(SportCoordinators));
            }
            else if (userRole == "Official")
            {
                return RedirectToAction(nameof(Officials));
            }
            else if (userRole == "Club Manager")
            {
                return RedirectToAction(nameof(ClubManagers));
            }
            else if (userRole == "News Administrator")
            {
                return RedirectToAction(nameof(NewsAdministrators));
            }
            else if (userRole == "News Updator")
            {
                return RedirectToAction(nameof(NewsUpdaters));
            }
            else if (userRole == "Fans Administrator")
            {
                return RedirectToAction(nameof(FansAdministrators));
            }
            else
            {
                return RedirectToAction(nameof(DivisionFans));
            }

        }

        public async Task<IActionResult> Delete(string? userId)
        {
            if(userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var existingUser = await _context.UserBaseModel
                .Where(e => e.Id == userId)
                .FirstOrDefaultAsync();

            var userRole = await _context.UserRoles
                      .Where(ur => ur.UserId == userId)
                      .Join(_context.Roles,
                       ur => ur.RoleId,
                       r => r.Id,
                       (ur, r) => r.Name)
                      .FirstOrDefaultAsync();



            existingUser.IsDeleted = true;
            existingUser.IsSuspended = true;
            existingUser.IsActive = true;
            existingUser.ModifiedBy = user.Id;
            existingUser.ModifiedDateTime = DateTime.Now;


            _context.Update(existingUser);
            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Deleted {existingUser.FirstName} {existingUser.LastName} profile from the system.", user.Id);

            TempData["Message"] = $"You have deleted {existingUser.FirstName} {existingUser.LastName} and now they don't longer exist in this system.";

            if (userRole == "Club  Administrator")
            {
                return RedirectToAction(nameof(ClubAdministrators));
            }
            else if (userRole == "Player")
            {
                return RedirectToAction(nameof(DivisionPlayers));
            }
            else if (userRole == "Sport Administrator")
            {
                return RedirectToAction(nameof(SportAdministrators));
            }
            else if (userRole == "Sport manager")
            {
                return RedirectToAction(nameof(SportManagers));
            }
            else if (userRole == "Sport Coordinator")
            {
                return RedirectToAction(nameof(SportCoordinators));
            }
            else if (userRole == "Official")
            {
                return RedirectToAction(nameof(Officials));
            }
            else if (userRole == "Club Manager")
            {
                return RedirectToAction(nameof(ClubManagers));
            }
            else if (userRole == "News Administrator")
            {
                return RedirectToAction(nameof(NewsAdministrators));
            }
            else if (userRole == "News Updator")
            {
                return RedirectToAction(nameof(NewsUpdaters));
            }
            else if (userRole == "Fans Administrator")
            {
                return RedirectToAction(nameof(FansAdministrators));
            }
            else
            {
                return RedirectToAction(nameof(DivisionFans));
            }
        }

        public async Task<IActionResult> ChangeUserRole(string? userId)
        {
            return View();
        }


        /*Update all users methods*/

        [HttpGet]
        public async Task<IActionResult> UpdateClubManager(string userId)
        {
            var logMessages = new List<string>();

            if (userId == null || _context.ClubManager == null)
            {
                logMessages.Add($"UpdateClubManager GET request failed: id is null or ClubManager context is null");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubManager = await _context.ClubManager.FindAsync(userId);
            if (clubManager == null)
            {
                logMessages.Add($"ClubManager with id {userId} not found during GET request");
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
        public async Task<IActionResult> UpdateClubManager(string userId, UpdateClubManagerViewModel viewModel, IFormFile ProfilePictureFile)
        {
            var logMessages = new List<string>();

            if (userId != viewModel.Id)
            {
                logMessages.Add($"UpdateClubManager POST request failed: id mismatch: {userId} != {viewModel.Id}");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var clubManager = await _context.ClubManager.FindAsync(userId);

            if (ValidateUpdatedProperties(viewModel))
            {

                try
                {

                    if (clubManager == null)
                    {
                        logMessages.Add($"ClubManager with id {userId} not found during POST request");
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
                    clubManager.ModifiedBy = user.Id;
                    clubManager.ModifiedDateTime = DateTime.Now;

                    if (ProfilePictureFile != null && ProfilePictureFile.Length > 0)
                    {
                        var uploadedImagePath = await _fileUploadService.UploadFileAsync(ProfilePictureFile);
                        clubManager.ProfilePicture = uploadedImagePath;
                    }

                    _context.Update(clubManager);
                    await _context.SaveChangesAsync();
                    logMessages.Add($"ClubManager with id {userId} successfully updated");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    logMessages.Add($"DbUpdateConcurrencyException occurred: {ex.Message}");
                    TempData["LogMessages"] = logMessages;
                    throw;
                }

                await _activityLogger.Log($"Updated {clubManager.FirstName} {clubManager.LastName} profile.", user.Id);


                TempData["Message"] = $"{clubManager.FirstName} {clubManager.LastName}  information has been updated successfully.";
                return RedirectToAction(nameof(MyClubManagers));
            }

            logMessages.Add($"Model state invalid for ClubManager with id {userId}");

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    var errorMessage = $"Error in {state.Key}: {error.ErrorMessage}";
                    logMessages.Add(errorMessage);
                }
            }

            viewModel.ProfilePicture = _context.ClubManager.Find(userId)?.ProfilePicture;

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
        public async Task<IActionResult> UpdatePlayerProfile(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return NotFound();
            }

            var clubPlayer = await _context.Player.FindAsync(userId);
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
        public async Task<IActionResult> UpdatePlayerProfile(string userId, UpdateClubPlayerViewModel viewModel)
        {
            if (userId != viewModel.Id)
            {
                return NotFound();
            }

            if (ValidateUpdatedProperties(viewModel))
            {
                try
                {
                    var user = await _userManager.GetUserAsync(User);

                    var clubPlayer = await _context.Player.FindAsync(userId);
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
                    clubPlayer.ModifiedBy = user.Id;
                    clubPlayer.ModifiedDateTime = DateTime.Now;
                    clubPlayer.MarketValue =  viewModel.MarketValue;    

                    if (viewModel.ProfilePictureFile != null && viewModel.ProfilePictureFile.Length > 0)
                    {
                        var playerProfilePicturePath = await _fileUploadService.UploadFileAsync(viewModel.ProfilePictureFile);
                        clubPlayer.ProfilePicture = playerProfilePicturePath;
                    }

                    _context.Update(clubPlayer);
                    await _context.SaveChangesAsync();

                    await _activityLogger.Log($"Updated {clubPlayer.FirstName} {clubPlayer.LastName} profile.", user.Id);

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

        private bool ClubPlayerExists(string userId)
        {
            return _context.Player.Any(e => e.Id == userId);
        }


        [HttpGet]
        public async Task<IActionResult> UpdateClubAdministrator(string? userId)
        {
            var logMessages = new List<string>();

            if (userId == null || _context.ClubAdministrator == null)
            {
                logMessages.Add($"UpdateClubManager GET request failed: id is null or ClubManager context is null");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var clubAdministrator = await _context.ClubAdministrator.FindAsync(userId);
            if (clubAdministrator == null)
            {
                logMessages.Add($"ClubManager with id {userId} not found during GET request");
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
        public async Task<IActionResult> UpdateClubAdministrator(string userId, UpdateClubAdministratorViewModel viewModel, IFormFile ProfilePictureFile)
        {
            var logMessages = new List<string>();

            if (userId != viewModel.Id)
            {
                logMessages.Add($"UpdateClubManager POST request failed: id mismatch: {userId} != {viewModel.Id}");
                TempData["LogMessages"] = logMessages;
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var clubAdministrator = await _context.ClubAdministrator.FindAsync(userId);

            if (ValidateUpdatedProperties(viewModel))
            {

                try
                {

                    if (clubAdministrator == null)
                    {
                        logMessages.Add($"ClubManager with id {userId} not found during POST request");
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
                    clubAdministrator.ModifiedBy = user.Id;
                    clubAdministrator.ModifiedDateTime = DateTime.Now;

                    if (ProfilePictureFile != null && ProfilePictureFile.Length > 0)
                    {
                        var uploadedImagePath = await _fileUploadService.UploadFileAsync(ProfilePictureFile);
                        clubAdministrator.ProfilePicture = uploadedImagePath;
                    }

                    _context.Update(clubAdministrator);
                    await _context.SaveChangesAsync();
                    logMessages.Add($"ClubManager with id {userId} successfully updated");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    logMessages.Add($"DbUpdateConcurrencyException occurred: {ex.Message}");
                    TempData["LogMessages"] = logMessages;
                    throw;
                }

                TempData["LogMessages"] = logMessages;

                await _activityLogger.Log($"Updated {clubAdministrator.FirstName} {clubAdministrator.LastName} profile.", user.Id);

                TempData["Message"] = $"{clubAdministrator.FirstName} {clubAdministrator.LastName}  information has been updated successfully.";
                return RedirectToAction(nameof(MyClubAdministrators));
            }

            logMessages.Add($"Model state invalid for ClubManager with id {userId}");

            foreach (var state in ModelState)
            {
                foreach (var error in state.Value.Errors)
                {
                    var errorMessage = $"Error in {state.Key}: {error.ErrorMessage}";
                    logMessages.Add(errorMessage);
                }
            }

            viewModel.ProfilePicture = _context.ClubAdministrator.Find(userId)?.ProfilePicture;

            TempData["LogMessages"] = logMessages;
            viewModel.ProfilePicture = clubAdministrator.ProfilePicture;
            return View(viewModel);
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


        [HttpGet]
        public async Task<IActionResult> EndClubManagerContract(string userId)
        {
            if(userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return RedirectToAction("Error", "Home");
            }

            var clubManager = await _context.ClubManager
                .Where(c => c.Id == userId && c.ClubId == clubAdministrator.ClubId)
                .FirstOrDefaultAsync();

            var viewModel = new EndClubManagerContractViewModel
            {
                FullNames = $"{clubManager.FirstName} {clubManager.LastName}",
                ProfilePicture = clubManager.ProfilePicture,
                PhoneNumber = clubManager.PhoneNumber,
                Email = clubManager.Email
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EndClubManagerContract(string userId, EndClubManagerContractViewModel viewModel)
        {
            if (userId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return RedirectToAction("Error", "Home");
            }

            var clubManager = await _context.ClubManager
                .Where(c => c.Id == userId && c.ClubId == clubAdministrator.ClubId)
                .Include( c => c.Club)
                .FirstOrDefaultAsync();

            clubManager.IsActive = false;
            clubManager.IsSuspended = true;
            clubManager.IsContractEnded = true;

            _context.Update(clubManager);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"You have ended {viewModel.FullNames}'s contract with {clubManager.Club.ClubName}.";
            return RedirectToAction(nameof(MyClubManagers));
        }
    }
}
