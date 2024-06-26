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
            var ksans_SportsDbContext = _context.Fines
                .Where(mo => mo.ClubId != null)
                .Include(f => f.Club)
                .Include(f => f.CreatedBy)
                .Include(f => f.ModifiedBy)
                .ToListAsync();

            return View(await ksans_SportsDbContext);
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


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Fines == null)
            {
                return NotFound();
            }

            var fine = await _context.Fines.FindAsync(id);
            if (fine == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubLocation", fine.ClubId);
            ViewData["CreatedById"] = new SelectList(_context.SportMember, "Id", "Id", fine.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.SportMember, "Id", "Id", fine.ModifiedById);
            return View(fine);
        }

        // POST: Fines/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FineId,FineDetails,ClubId,CreatedById,CreatedDateTime,ModifiedById,ModifiedDateTime,FineAmount,FineDuDate")] Fine fine)
        {
            if (id != fine.FineId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(fine);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FineExists(fine.FineId))
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
            ViewData["ClubId"] = new SelectList(_context.Club, "ClubId", "ClubLocation", fine.ClubId);
            ViewData["CreatedById"] = new SelectList(_context.SportMember, "Id", "Id", fine.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.SportMember, "Id", "Id", fine.ModifiedById);
            return View(fine);
        }

        // GET: Fines/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

        // POST: Fines/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Fines == null)
            {
                return Problem("Entity set 'Ksans_SportsDbContext.Fines'  is null.");
            }
            var fine = await _context.Fines.FindAsync(id);
            if (fine != null)
            {
                _context.Fines.Remove(fine);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FineExists(int id)
        {
            return (_context.Fines?.Any(e => e.FineId == id)).GetValueOrDefault();
        }
    }
}
