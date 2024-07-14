using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Models;
using MyField.ViewModels;
using Microsoft.AspNetCore.Identity;
using MyField.Interfaces;
using System.ComponentModel.DataAnnotations;
using Microsoft.CodeAnalysis.Elfie.Serialization;

namespace MyField.Controllers
{
    public class FinesController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;


        public FinesController(Ksans_SportsDbContext context,
              UserManager<UserBaseModel> userManager,
              IActivityLogger activityLogger)
        {
            _userManager = userManager;
            _context = context;
            _activityLogger = activityLogger;
        }


        public async Task<IActionResult> _PendingClubFines()
        {
            var _pendingFines = await _context.Fines
                .Where(p => p.PaymentStatus == PaymentStatus.Pending && p.Club != null)
                .Include(p => p.Club)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .OrderByDescending(p=> p.CreatedDateTime)
                .ToListAsync();

            return PartialView("_PendingClubFinesPartial", _pendingFines);
        }

        public async Task<IActionResult> PaidClubFines()
        {
            var paidFines = await _context.Fines
                .Where(p => p.PaymentStatus == PaymentStatus.Paid && p.Club != null)
                .Include(p => p.Club)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .OrderByDescending(p => p.CreatedDateTime)
                .ToListAsync();

            return PartialView("_PaidClubFinesPartial", paidFines);
        }

        public async Task<IActionResult> OverdueClubFines()
        {
            var overdueFines = await _context.Fines
                .Where(p => p.PaymentStatus == PaymentStatus.Overdue && p.Club != null)
                .Include(p => p.Club)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .OrderByDescending(p => p.CreatedDateTime)
                .ToListAsync();

            return PartialView("_OverdueClubFinesPartial", overdueFines);
        }

        public async Task<IActionResult> PendingClubFines ()
        {
            var pendingFines = await _context.Fines
                .Where(p => p.PaymentStatus == PaymentStatus.Pending && p.Club != null)
                .Include(p => p.Club)
                .Include(p => p.CreatedBy)
                .Include(p => p.ModifiedBy)
                .ToListAsync();

            return View(pendingFines);
        }

        public async Task<IActionResult> ClubFines()
        {
            return View();
        }

        public async Task<IActionResult> IndividualFines()
        {
            var ksans_SportsDbContext = _context.Fines
                .Where(mo => mo.OffenderId != null) 
                .Include(f => f.Offender)
                .Include(f => f.CreatedBy)
                .Include(f => f.ModifiedBy)
                .ToListAsync();

            return View(await ksans_SportsDbContext);
        }

        public async Task<IActionResult> MyPendingClubFines()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return RedirectToAction("Error", "Home");
            }

            var myPendingClubFines = await _context.Fines
                .Where(mo => mo.ClubId == clubAdministrator.ClubId && mo.PaymentStatus.Equals(PaymentStatus.Pending))
                .Include(s => s.Club)
                .ToListAsync();

            return PartialView("_MyPendingClubFinesPartial", myPendingClubFines);
        }

        public async Task<IActionResult> MyPaidClubFines()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return RedirectToAction("Error", "Home");
            }

            var myPaidClubFines = await _context.Fines
                .Where(mo => mo.ClubId == clubAdministrator.ClubId && mo.PaymentStatus.Equals(PaymentStatus.Paid))
                .Include(s => s.Club)
                .ToListAsync();

            return PartialView("_MyPaidClubFinesPartial", myPaidClubFines);
        }

        public async Task<IActionResult> MyOverDueClubFines()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            if (user == null || !(user is ClubAdministrator clubAdministrator))
            {
                return RedirectToAction("Error", "Home");
            }

            var myOverDueClubFines = await _context.Fines
                .Where(mo => mo.ClubId == clubAdministrator.ClubId && mo.PaymentStatus.Equals(PaymentStatus.Overdue))
                .Include(s => s.Club)
                .ToListAsync();

            return PartialView("_MyOverDueClubFinesPartial", myOverDueClubFines);
        }

        public async Task<IActionResult> MyClubFines()
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

        public async Task<IActionResult> MyIndividualFines()
        {
            return View();
        }

        public async Task<IActionResult> MyPendingFines()
        {

            var user = await _userManager.GetUserAsync(User);

            var myFines = await _context.Fines
                .Where(f => f.OffenderId == user.Id && f.PaymentStatus == PaymentStatus.Pending)
                .Include(f => f.Offender)
                .ToListAsync();

            return PartialView("_MyPendingFinesPartial", myFines);
        }

        public async Task<IActionResult> MyPaidFines()
        {
            var user = await _userManager.GetUserAsync(User);

            var myFines = await _context.Fines
                .Where(f => f.OffenderId == user.Id && f.PaymentStatus == PaymentStatus.Paid)
                .Include(f => f.Offender)
                .ToListAsync();

            return PartialView("_MyPaidFinesPartial", myFines);
        }

        public async Task<IActionResult> MyOverDueFines()
        {
            var user = await _userManager.GetUserAsync(User);

            var myFines = await _context.Fines
                .Where(f => f.OffenderId == user.Id && f.PaymentStatus == PaymentStatus.Overdue)
                .Include(f => f.Offender)
                .ToListAsync();

            return PartialView("_MyOverDueFinesPartial", myFines);
        }


        public async Task<IActionResult> ClubFineDetails(int? id)
        {
            if (id == null || _context.Fines == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines
                .Include(f => f.Club)
                .Include(f => f.CreatedBy)
                .Include(f => f.ModifiedBy)
                .FirstOrDefaultAsync(m => m.FineId == id);
            if (fine == null)
            {
                return NotFound();
            }

            return View(fine);
        }

        public async Task<IActionResult> IndividualFineDetails(int? id)
        {
            if (id == null || _context.Fines == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines
                .Include(f => f.Club)
                .Include(f => f.CreatedBy)
                .Include(f => f.ModifiedBy)
                .FirstOrDefaultAsync(m => m.FineId == id);
            if (fine == null)
            {
                return NotFound();
            }

            return View(fine);
        }


        [HttpGet]
        public async Task<IActionResult> CreateIndividualFine()
        {
            ViewBag.SystemUsers = new SelectList(_context.SystemUsers.Select(u => new
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName
            }), "Id", "FullName");


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateIndividualFine(CreateIndividualFineViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var personnelFinancialReport = await _context.PersonnelFinancialReports
                    .FirstOrDefaultAsync();

                var newIndividualFine = new Fine
                {
                    FineDetails = viewModel.FineDetails,
                    RuleViolated = viewModel.RuleViolated,
                    FineAmount = viewModel.FineAmount,
                    FineDuDate = viewModel.FineDuDate,
                    OffenderId = viewModel.OffenderId,
                    CreatedById = userId,
                    CreatedDateTime = DateTime.Now,
                    ModifiedById = userId,
                    ModifiedDateTime = DateTime.Now,
                };

                personnelFinancialReport.ExpectedRepayableAmount = personnelFinancialReport.ExpectedRepayableAmount + viewModel.FineAmount;
                personnelFinancialReport.RepayableFinesCount++;

                newIndividualFine.PaymentStatus = PaymentStatus.Pending;

                _context.Add(newIndividualFine);
                await _context.SaveChangesAsync();

                var newSavedFine = await _context.Fines
                    .Where(mo => mo.Equals(newIndividualFine))
                    .Include(f => f.Offender)
                    .FirstOrDefaultAsync();

                TempData["Message"] = $"You have successfully filed a fine/offence against {newSavedFine.Offender.FirstName} {newSavedFine.Offender.LastName}. The offence is required to pay an amount of {newSavedFine.FineAmount} before {newSavedFine.FineDuDate}.The email concerning this charge has been sent to the relevant individual.";
                await _activityLogger.Log($"Charged {newSavedFine.Offender.FirstName} {newSavedFine.Offender.FirstName} R{newSavedFine.FineAmount} for {newSavedFine.RuleViolated}", user.Id);
                return RedirectToAction(nameof(IndividualFines));
            }

            ViewBag.SystemUsers = new SelectList(_context.SystemUsers.Select(u => new
            {
                Id = u.Id,
                FullName = u.FirstName + " " + u.LastName
            }), "Id", "FullName");

            return View(viewModel);
        }



        public IActionResult CreateCLubFine()
        {
            ViewBag.Clubs = new SelectList(_context.Club, "ClubId", "ClubName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCLubFine(CreateClubFineViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var newClubFine = new Fine
                {
                    RuleViolated = viewModel.RuleViolated,
                    FineDetails = viewModel.FineDetails,
                    FineAmount = viewModel.FineAmount,
                    FineDuDate = viewModel.FineDuDate,
                    ClubId = viewModel.ClubId,
                    CreatedById = userId,
                    CreatedDateTime = DateTime.Now,
                    ModifiedById = userId,
                    ModifiedDateTime = DateTime.Now,
                };

                newClubFine.PaymentStatus = PaymentStatus.Pending;

                _context.Add(newClubFine);
                await _context.SaveChangesAsync();

                var newSavedFine = await _context.Fines
                    .Where(mo => mo.Equals(newClubFine))
                    .Include(f => f.Club)
                    .Include(f => f.Offender)
                    .FirstOrDefaultAsync();

                TempData["Message"] = $"You have successfully filed a fine/offence against {newSavedFine.Club.ClubName}. The club is required to pay an amount of {newSavedFine.FineAmount} before {newSavedFine.FineDuDate}. The email concerning this charge has been sent to the relevant club.";
                await _activityLogger.Log($"Charged {newSavedFine.Club.ClubName} R{newSavedFine.FineAmount} for {newSavedFine.RuleViolated}", user.Id);
                return RedirectToAction(nameof(ClubFines));
            }

            ViewBag.Clubs = new SelectList(_context.Club, "ClubId", "ClubName", viewModel.ClubId);

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateClubFine (int? fineId)
        {

            var clubFine = await _context.Fines
                .Where(e => e.ClubId != null && e.OffenderId == null && e.FineId == fineId)
                .Include(e => e.Club)
                .FirstOrDefaultAsync();

            var club = clubFine.Club;


            var viewModel = new UpdateClubFineViewModel
            {
                FineId = fineId,
                ClubName = club.ClubName,
                RuleViolated = clubFine.RuleViolated,
                FineDetails = clubFine.FineDetails,
                FineAmount = clubFine.FineAmount,
                FineDueDate = clubFine.FineDuDate
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateClubFine(int? fineId, UpdateClubFineViewModel viewModel)
        {
            if (fineId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var existingClubFine = await _context.Fines
                .Where(e => e.ClubId != null && e.OffenderId == null && e.FineId == viewModel.FineId)
                .Include(e => e.Club)
                .FirstOrDefaultAsync();

            if (ValidateClubUpdatedProperties(viewModel))
            {

                existingClubFine.RuleViolated = viewModel.RuleViolated;
                existingClubFine.FineDetails = viewModel.FineDetails;
                existingClubFine.FineAmount = viewModel.FineAmount;
                existingClubFine.FineDuDate = viewModel.FineDueDate;
                existingClubFine.ModifiedById = user.Id;
                existingClubFine.ModifiedDateTime = DateTime.Now;

                _context.Update(existingClubFine);
                await _context.SaveChangesAsync();
            }

            TempData["Message"] = $"You have successfully updated {existingClubFine.Club.ClubName} charges";

            await _activityLogger.Log($"Updated {existingClubFine.Club.ClubName} fines", user.Id);
            return RedirectToAction(nameof(ClubFines));
        }

        private bool ValidateClubUpdatedProperties(UpdateClubFineViewModel viewModel)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(viewModel.RuleViolated, new ValidationContext(viewModel, null, null) { MemberName = "RuleViolated" }, validationResults);
            Validator.TryValidateProperty(viewModel.FineDetails, new ValidationContext(viewModel, null, null) { MemberName = "FineDetails" }, validationResults);
            Validator.TryValidateProperty(viewModel.FineAmount, new ValidationContext(viewModel, null, null) { MemberName = "FineAmount" }, validationResults);
            Validator.TryValidateProperty(viewModel.FineDueDate, new ValidationContext(viewModel, null, null) { MemberName = "FineDueDate" }, validationResults);
            return validationResults.Count == 0;
        }


        public async Task<IActionResult> MarkOverDueClubFine (int? fineId)
        {
            if(fineId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var existingClubFine = await _context.Fines
                .Where(e => e.ClubId != null && e.OffenderId == null && e.FineId == fineId)
                .Include(e => e.Club)
                .FirstOrDefaultAsync();

            existingClubFine.PaymentStatus = PaymentStatus.Overdue;
            existingClubFine.ModifiedById = user.Id;
            existingClubFine.ModifiedDateTime = DateTime.Now;

            _context.Update(existingClubFine);

            await _context.SaveChangesAsync();


            TempData["Message"] = $"You have successfully markerd {existingClubFine.Club.ClubName} charges as overdue payment";

            await _activityLogger.Log($"Marked {existingClubFine.Club.ClubName} fines as overdue", user.Id);

            return RedirectToAction(nameof(ClubFines));
        }

        public async Task<IActionResult> DropClubFine (int? fineId)
        {
            if (fineId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var existingClubFine = await _context.Fines
                .Where(e => e.ClubId != null && e.OffenderId == null && e.FineId == fineId)
                .Include(e => e.Club)
                .FirstOrDefaultAsync();

            TempData["Message"] = $"You have deleted {existingClubFine.Club.ClubName} fines of {existingClubFine.RuleViolated}";

            await _activityLogger.Log($"Dropped {existingClubFine.Club.ClubName} fines of {existingClubFine.RuleViolated}", user.Id);

            _context.Remove(existingClubFine);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(ClubFines));
        }



        private bool FineExists(int id)
        {
            return (_context.Fines?.Any(e => e.FineId == id)).GetValueOrDefault();
        }
    }
}
