using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
                .Include(m => m.ModifiedBy)
                .Where(m => m.MeetingStatus == MeetingStatus.Upcoming || 
                m.MeetingStatus == MeetingStatus.Postponed);

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
            else if (User.IsInRole("News Administrator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.News_Administrators
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Fans Administrator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Fans_Administrators
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Personnel Administrator"))
            {
                meetingsQuery = meetingsQuery
                    .Where(m => m.MeetingAttendees == MeetingAttendees.Personnel_Administrators
                             || m.MeetingAttendees == MeetingAttendees.Everyone);
            }
            else if (User.IsInRole("Sport Manager"))
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

                newMeeting.MeetingStatus = MeetingStatus.Upcoming;

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

        [HttpGet]

        public async Task<IActionResult> UpdateMeeting(int? meetingId)
        {
            if (meetingId == null)
            {
                return NotFound();
            }

            var existingMeeting = await _context.Meeting
                .Where(e => e.MeetingId == meetingId)
                .FirstOrDefaultAsync();

            var viewModel = new UpdateMeetingViewModel
            {
                MeetingTitle = existingMeeting.MeetingTitle,
                MeetingDescription = existingMeeting.MeetingDescription,
                AdditionalComments = existingMeeting.AdditionalComments,
                MeetingDate = existingMeeting.MeetingDate,
                MeetingTime = existingMeeting.MeetingTime,
                Venue = existingMeeting.Venue,
                MeetingAttendees = existingMeeting.MeetingAttendees
            };

            return View(viewModel);
        }


        [HttpPost]

        public async Task<IActionResult> UpdateMeeting(int? meetingId, UpdateMeetingViewModel viewModel)
        {
            if(meetingId != viewModel.MeetingId)
            {
                return NotFound();
            }


            if(ValidateUpdatedProperties(viewModel))
            {
                var user = await _userManager.GetUserAsync(User);


                var existingMeeting = await _context.Meeting
                     .Where(e => e.MeetingId == meetingId)
                      .FirstOrDefaultAsync();


                existingMeeting.MeetingTitle = viewModel.MeetingTitle;
                existingMeeting.MeetingDescription = viewModel.MeetingDescription;
                existingMeeting.Venue = viewModel.Venue;
                existingMeeting.MeetingDate = viewModel.MeetingDate;
                existingMeeting.MeetingTime = viewModel.MeetingTime;
                existingMeeting.AdditionalComments = viewModel.AdditionalComments;
                existingMeeting.ModifiedById = user.Id;
                existingMeeting.ModifiedDateTime = user.ModifiedDateTime;


                _context.Update(existingMeeting);
                await _context.SaveChangesAsync();


                TempData["Message"] = $"You have successfully updated a meeting with title {existingMeeting.MeetingTitle}";
                await _activityLogger.Log($"Updated a meeting with title {existingMeeting.MeetingTitle}", user.Id);


                return RedirectToAction(nameof(Meetings));
            }

            return View(viewModel);
        }

        private bool ValidateUpdatedProperties(UpdateMeetingViewModel viewModel)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(viewModel.MeetingDate, new ValidationContext(viewModel, null, null) { MemberName = "MeetingDate" }, validationResults);
            Validator.TryValidateProperty(viewModel.MeetingTitle, new ValidationContext(viewModel, null, null) { MemberName = "MeetingTitle" }, validationResults);
            Validator.TryValidateProperty(viewModel.MeetingTime, new ValidationContext(viewModel, null, null) { MemberName = "MeetingTime" }, validationResults);
            Validator.TryValidateProperty(viewModel.MeetingDescription, new ValidationContext(viewModel, null, null) { MemberName = "MeetingDescription" }, validationResults);
            Validator.TryValidateProperty(viewModel.AdditionalComments, new ValidationContext(viewModel, null, null) { MemberName = "AdditionalComments" }, validationResults);
            Validator.TryValidateProperty(viewModel.Venue, new ValidationContext(viewModel, null, null) { MemberName = "Venue" }, validationResults);
            return validationResults.Count == 0;
        }


        [HttpGet]
        public async Task<IActionResult> MeetingDetails(int? meetingId)
        {
            if(meetingId == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meeting
                .Where(m => m.MeetingId == meetingId)
                .FirstOrDefaultAsync();


            var viewModel = new MeetingDetailsViewModel
            {
                MeetingAttendees = meeting.MeetingAttendees,
                MeetingTitle = meeting.MeetingTitle,
                MeetingDescription = meeting.MeetingDescription,
                Venue = meeting.Venue,
                MeetingDate = meeting.MeetingDate,
                MeetingTime = meeting.MeetingTime,
                AdditionalComments = meeting.AdditionalComments,
                MeetingStatus = meeting.MeetingStatus,
            };


            return View(viewModel);
        }


        public async Task<IActionResult> PostponeMeeting(int? meetingId)
        {

            if (meetingId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);


            var meeting = await  _context.Meeting
                .Where(m => m.MeetingId == meetingId)
                .FirstOrDefaultAsync();


            meeting.MeetingStatus = MeetingStatus.Postponed;
            meeting.ModifiedById = user.Id;
            meeting.ModifiedDateTime = user.ModifiedDateTime;

            _context.Update(meeting);
            await _context.SaveChangesAsync();

            TempData["Message"] = $"You have successfully postponed a meeting with title {meeting.MeetingTitle}";
            await _activityLogger.Log($"Postponed a meeting with title {meeting.MeetingTitle}", user.Id);

            return RedirectToAction(nameof(Meetings));
        }

        public async Task<IActionResult> CancelMeeting(int? meetingId)
        {

            if (meetingId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var meeting = await _context.Meeting
                .Where(m => m.MeetingId == meetingId)
                .FirstOrDefaultAsync();


            meeting.MeetingStatus = MeetingStatus.Cancelled;
            meeting.ModifiedById = user.Id;
            meeting.ModifiedDateTime = user.ModifiedDateTime;

            _context.Update(meeting);
            await _context.SaveChangesAsync();


            TempData["Message"] = $"You have cancelled a meeting with title {meeting.MeetingTitle}";
            await _activityLogger.Log($"Cancelled a meeting with title {meeting.MeetingTitle}", user.Id);

            return RedirectToAction(nameof(Meetings));
        }
    }
}
