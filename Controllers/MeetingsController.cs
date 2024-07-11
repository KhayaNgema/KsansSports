using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class MeetingsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;

        public MeetingsController(Ksans_SportsDbContext context,
              UserManager<UserBaseModel> userManager,
              IActivityLogger activityLogger)
        {
            _userManager = userManager;
            _context = context;
            _activityLogger = activityLogger;
        }

        public async Task<IActionResult> Meetings()
        {
            IQueryable<Meeting> meetingsQuery = _context.Meeting
                .Include(m => m.CreatedBy)
                .Include(m => m.ModifiedBy);

            if (User.IsInRole("Sport Coordinator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Sport_Coordinators
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Club Administrator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Club_Administrators
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Club Manager"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Club_Managers
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Player"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Players
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Sport Administrator"))
            {
                meetingsQuery = meetingsQuery;
            }
            else if (User.IsInRole("News Updator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.News_Updaters
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Official"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Officials
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("News administrator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.News_Administrators
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Fans administrator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Fans_Administrators
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Personnel administrator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Personnel_Administrators
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Sport manager"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Sport_Managers
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }

            var meetings = await meetingsQuery.ToListAsync();

            return View(meetings);
        }


        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Meeting == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting
                .Include(m => m.CreatedBy)
                .Include(m => m.ModifiedBy)
                .FirstOrDefaultAsync(m => m.MeetingId == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        public IActionResult Create()
        {
            ViewData["MeetingsAttendes"] = Enum.GetValues(typeof(MeetingAttendees))
                     .Cast<MeetingAttendees>()
                     .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() });

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(MeetingViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var newMeeting = new Meeting
                {
                    MeetingTitle = viewModel.MeetingTitle,
                    MeetingDescription = viewModel.MeetingDescription,
                    AdditionalComments = viewModel.AdditionalComments,
                    Venue = viewModel.Venue,
                    MeetingDate = viewModel.MeetingDate,    
                    MeetingTime = viewModel.MeetingTime,
                    CreatedById = userId,
                    CreatedDateTime = DateTime.Now,
                    ModifiedById = userId,
                    ModifiedDateTime = DateTime.Now,
                    MeetingAttendees = viewModel.MeetingAttendes
                };


                _context.Add(newMeeting);
                await _context.SaveChangesAsync();
                await _activityLogger.Log($"", user.Id);

                var savedMeeting = await _context.Meeting
                    .Where( s => s.Equals(newMeeting))
                    .FirstOrDefaultAsync();

                TempData["Message"] = $"You have successfully scheduled a meeting that will take place at {viewModel.Venue}. on {viewModel.MeetingDate} at {viewModel.MeetingTime}. You invited {viewModel.MeetingAttendes} to join the meeting.";
                await _activityLogger.Log($"Scheduled a meeting that will take place as follows; Date:{savedMeeting.MeetingDate.ToString("ddd, dd MMM yyyy")}, Time:{savedMeeting.MeetingTime.ToString("HH:mm")}", user.Id);
                return RedirectToAction(nameof(Meetings));
            }

            ViewData["MeetingsAttendes"] = Enum.GetValues(typeof(MeetingAttendees))
                    .Cast<MeetingAttendees>()
                    .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() });

            return View(viewModel);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Meeting == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting.FindAsync(id);
            if (meeting == null)
            {
                return NotFound();
            }
            ViewData["CreatedById"] = new SelectList(_context.SystemUsers, "Id", "Id", meeting.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.SystemUsers, "Id", "Id", meeting.ModifiedById);
            return View(meeting);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MeetingId,MeetingTitle,MeetingDescription,Venue,MeetingDate,MeetingTime,AdditionalComments,CreatedDateTime,ModifiedDateTime,CreatedById,ModifiedById")] Meeting meeting)
        {
            if (id != meeting.MeetingId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MeetingExists(meeting.MeetingId))
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
            ViewData["CreatedById"] = new SelectList(_context.SystemUsers, "Id", "Id", meeting.CreatedById);
            ViewData["ModifiedById"] = new SelectList(_context.SystemUsers, "Id", "Id", meeting.ModifiedById);
            return View(meeting);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Meeting == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting
                .Include(m => m.CreatedBy)
                .Include(m => m.ModifiedBy)
                .FirstOrDefaultAsync(m => m.MeetingId == id);
            if (meeting == null)
            {
                return NotFound();
            }

            return View(meeting);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Meeting == null)
            {
                return Problem("Entity set 'Ksans_SportsDbContext.Meeting'  is null.");
            }
            var meeting = await _context.Meeting.FindAsync(id);
            if (meeting != null)
            {
                _context.Meeting.Remove(meeting);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MeetingExists(int id)
        {
          return (_context.Meeting?.Any(e => e.MeetingId == id)).GetValueOrDefault();
        }
    }
}
