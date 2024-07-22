using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;

namespace MyField.Controllers
{
    public class SportNewsController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly FileUploadService _fileUploadService;
        private readonly IActivityLogger _activityLogger;
        private readonly EmailService _emailService;

        public SportNewsController(Ksans_SportsDbContext context, 
            UserManager<UserBaseModel> userManager, 
            FileUploadService fileUploadService,
            IActivityLogger activityLogger,
            EmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
            _activityLogger = activityLogger;
            _emailService = emailService;
        }

        public async Task<IActionResult> SportNewsIndex()
        {
            return PartialView("NewsPartial");
        }


        [Authorize(Roles =("News Administrator"))]
        public async Task<IActionResult> NewsReview(int? newsId)
        {
            if (newsId == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var overallNewsReports = await _context.OverallNewsReports
                .FirstOrDefaultAsync();

            var individualNewsReports = await _context.IndividualNewsReports
                .Where(i => i.SportNewsId == newsId)
                .Include(i => i.SportNews)
                .FirstOrDefaultAsync();

            overallNewsReports.NewsReadersCount++;
            individualNewsReports.ReadersCount++;

            await _context.SaveChangesAsync();

            var sportNews = await _context.SportNew
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .Include(s => s.RejectedBy)
                .FirstOrDefaultAsync(m => m.NewsId == newsId);
            if (sportNews == null)
            {
                return NotFound();
            }

            return View(sportNews);
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> ToBeModifiedSportNews()
        {
            var newsToBeModified = await _context.SportNew
                .Where(n => n.NewsStatus == NewsStatus.ToBeModified)
                .Include( n=> n.AuthoredBy)
                .Include(n => n.ModifiedBy)
                .OrderByDescending(n => n.ModifiedDateTime)
                .ToListAsync();

            return View(newsToBeModified);
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task <IActionResult> PublishedSportNewsBackOffice()
        {

            var sportsNews = await _context.SportNew
                  .Where(s => s.NewsStatus == NewsStatus.Approved)
                  .Include(s => s.AuthoredBy)
                  .Include(s => s.ModifiedBy)
                  .Include(s => s.PublishedBy)
                  .OrderByDescending(n => n.PublishedDate)
                  .ToListAsync();

            return View(sportsNews);
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> PublishedSportNews()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Approved && 
                 s.AuthoredById == userId)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return PartialView("_PublishedSportNewsPartial",sportsNews);
        }

        [Authorize(Roles = ("News Updator"))]
        public async Task<IActionResult> SportNewsList()
        {
            return View();
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> ApprovedSportNewsAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Approved && 
                 s.AuthoredById == userId)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.ModifiedDateTime)
                .ToListAsync();

            return PartialView("_ApprovedSportNewsPartial", sportsNews);
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> ApprovedSportNews()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Approved)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                 .OrderByDescending(n => n.ModifiedDateTime)
                .ToListAsync();

            return View(sportsNews);
        }


        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> AwaitingApprovalSportNewsAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Awaiting_Approval && 
                 s.AuthoredById == userId)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.ModifiedDateTime)
                .ToListAsync();

            return PartialView("_PendingSportNewsPartial",sportsNews);
        }

        [Authorize(Roles = ("News Administrator"))]
        public async Task<IActionResult> AwaitingApprovalSportNews()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Awaiting_Approval)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.ModifiedDateTime)
                .ToListAsync();

            return View(sportsNews);
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> RejectedSportNewsAdmin()
        {
            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Rejected && 
                 s.AuthoredById == userId)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.RejectedDateTime)
                .ToListAsync();

            return PartialView("_RejectedSportNewsPartial",sportsNews);
        }

        [Authorize(Roles = ("News Administrator"))]
        public async Task<IActionResult> RejectedSportNews()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Rejected)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                 .OrderByDescending(n => n.RejectedDateTime)
                .ToListAsync();

            return View(sportsNews);
        }

        [Authorize(Roles = ("News Administrator, News Updator"))]
        public async Task<IActionResult> SportNewsBackOffice()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Approved)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                 .OrderByDescending(n => n.ModifiedDateTime)
                .ToListAsync();

            return View(sportsNews);
        }


        public async Task<IActionResult> Index()
        {
            var sportsNews = await _context.SportNew
                .Where(s => s.NewsStatus == NewsStatus.Approved)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return PartialView("_SportsNewsPartial", sportsNews);
        }


        [Authorize]
        public async Task<IActionResult> SportNews()
        {
            var sportsNews = await _context.SportNew
                .Where(s => s.NewsStatus == NewsStatus.Approved)
                .OrderByDescending(s => s.PublishedDate)
                .ToListAsync();

            return View( sportsNews);
        }

        [Authorize]
        public async Task<IActionResult> Details(int? newsId)
        {
            if (newsId == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var overallNewsReports = await _context.OverallNewsReports
                .FirstOrDefaultAsync();

            var individualNewsReports = await _context.IndividualNewsReports
                .Where(i => i.SportNewsId == newsId)
                .Include(i => i.SportNews)
                .FirstOrDefaultAsync();

            if(User.IsInRole("News Administrator") || (User.IsInRole("News Updator")))
            {

            }
            else
            {
                overallNewsReports.NewsReadersCount++;
                individualNewsReports.ReadersCount++;
            }

            await _context.SaveChangesAsync();

            var sportNews = await _context.SportNew
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .Include(s => s.RejectedBy)
                .FirstOrDefaultAsync(m => m.NewsId == newsId);
            if (sportNews == null)
            {
                return NotFound();
            }

            return PartialView("_SportNewsDetailsPartial", sportNews);
        }


        [Authorize(Roles = ("News Updator"))]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = ("News Updator"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SportNewsViewModel viewModel, IFormFile NewsImages)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var overallNewsReports = await _context.OverallNewsReports
                        .FirstOrDefaultAsync();

                    var user = await _userManager.GetUserAsync(User);
                    var userId = user.Id;

                    var sportNews = new SportNews
                    {
                        NewsHeading = viewModel.NewsHeading,
                        NewsBody = viewModel.NewsBody,
                        AuthoredById = userId,
                        ModifiedById = userId,
                        PublishedById = userId,
                        ModifiedDateTime = DateTime.UtcNow,
                        RejectedById = userId,
                        RejectedDateTime = DateTime.UtcNow
                    };

                    if (NewsImages != null && NewsImages.Length > 0)
                    {
                        var uploadedImagePath = await _fileUploadService.UploadFileAsync(NewsImages);
                        sportNews.NewsImage = uploadedImagePath;
                    }

                    overallNewsReports.AuthoredNewsCount++;
                    sportNews.NewsStatus = NewsStatus.Awaiting_Approval;

                    _context.SportNew.Add(sportNews);
                    await _context.SaveChangesAsync();

                    var savedNews = await _context.SportNew
                        .Where(s => s.Equals(sportNews))
                        .FirstOrDefaultAsync();

                    var newIndividualNews = new IndividualNewsReport
                    {
                        SportNewsId = sportNews.NewsId,
                        ReadersCount = 0,
                    };

                    _context.IndividualNewsReports.Add(newIndividualNews);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = $"You have successfully authored the sport news with heading {savedNews.NewsHeading}.";
                    await _activityLogger.Log($"Authored sport news with heading {savedNews.NewsHeading}", user.Id);

                    // Email sending logic
                    var newsAdminRoleName = "News Administrator";
                    var subject = "Pending News Approval";
                    var emailBodyTemplate = $@"
                Dear {{0}},<br/><br/>
                A new sport news item with the heading <b>{savedNews.NewsHeading}</b> is awaiting your approval.<br/><br/>
                Please review and take the necessary action as soon as possible.<br/><br/>
                If you have any questions, please contact us at support@ksfoundation.com.<br/><br/>
                Regards,<br/>
                K&S Foundation Support Team
                    ";

                    var newsAdminUsers = await _userManager.GetUsersInRoleAsync(newsAdminRoleName);

                    foreach (var adminUser in newsAdminUsers)
                    {
                        var personalizedEmailBody = string.Format(emailBodyTemplate, $"{adminUser.FirstName} {adminUser.LastName}");
                        BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(adminUser.Email, subject, personalizedEmailBody));
                    }

                    return RedirectToAction(nameof(SportNewsList));
                }
                return View(viewModel);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to create news: " + ex.Message,
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    }
                });
            }
        }

        [Authorize(Roles = ("News Updator"))]
        [HttpGet]
        public async Task<IActionResult> ReEditNews(int? newsId)
        {
            if (newsId == null)
            {
                return NotFound();
            }

            var existingNews = await _context.SportNew
                .Where(e => e.NewsId == newsId)
                .FirstOrDefaultAsync();

            if (existingNews == null)
            {
                return NotFound();
            }

            var viewModel = new ReEditNewsViewModel
            {
                NewsId = existingNews.NewsId,
                NewsImage = existingNews.NewsImage,
                NewsTitle = existingNews.NewsHeading,
                NewsBody = existingNews.NewsBody,
                ReasonForReEdit = existingNews.ReasonForReEdit
            };

            return View(viewModel);
        }

        [Authorize(Roles = ("News Updator"))]
        [HttpPost]
        public async Task<IActionResult> ReEditNews(int? newsId, ReEditNewsViewModel viewModel, IFormFile NewsImages)
        {
            if (newsId == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var existingNews = await _context.SportNew
                .Where(e => e.NewsId == viewModel.NewsId)
                .FirstOrDefaultAsync();

            if (existingNews == null)
            {
                return NotFound();
            }

            var oldNewsStatus = existingNews.NewsStatus;

            if (ValidateUpdatedProperties(viewModel))
            {
                existingNews.NewsHeading = viewModel.NewsTitle;
                existingNews.NewsBody = viewModel.NewsBody;
                existingNews.NewsStatus = NewsStatus.Awaiting_Approval;

                if (NewsImages != null && NewsImages.Length > 0)
                {
                    var uploadedImagePath = await _fileUploadService.UploadFileAsync(NewsImages);
                    existingNews.NewsImage = uploadedImagePath;
                }
                else
                {
                    existingNews.NewsImage = viewModel.NewsImage;
                }

                _context.Update(existingNews);
                await _context.SaveChangesAsync();

                await _activityLogger.Log($"Re-edited sport news with heading {existingNews.NewsHeading}", user.Id);

                var newsAdminRoleName = "News Administrator";
                var subject = "News Re-Edited and Awaiting Approval";
                var emailBodyTemplate = $@"
            Dear {{0}},<br/><br/>
            The news item with the heading <b>{existingNews.NewsHeading}</b> has been re-edited and is now awaiting your review and approval.<br/><br/>
            Please review the updated news and take the necessary action.<br/><br/>
            If you have any questions, please contact us at support@ksfoundation.com.<br/><br/>
            Regards,<br/>
            K&S Foundation Support Team
                ";

                var newsAdminUsers = await _userManager.GetUsersInRoleAsync(newsAdminRoleName);

                foreach (var adminUser in newsAdminUsers)
                {
                    var personalizedEmailBody = string.Format(emailBodyTemplate, $"{adminUser.FirstName} {adminUser.LastName}");
                    BackgroundJob.Enqueue(() => _emailService.SendEmailAsync(adminUser.Email, subject, personalizedEmailBody));
                }

                if (oldNewsStatus == NewsStatus.Awaiting_Approval)
                {
                    TempData["Message"] = $"You have successfully updated the sport news with heading {existingNews.NewsHeading}.";
                    return RedirectToAction(nameof(SportNewsList));
                }
                else if (oldNewsStatus == NewsStatus.ToBeModified)
                {
                    TempData["Message"] = $"You have successfully updated the sport news with heading {existingNews.NewsHeading}.";
                    return RedirectToAction(nameof(ToBeModifiedSportNews));
                }
                else
                {
                    TempData["Message"] = $"You have successfully updated the sport news with heading {existingNews.NewsHeading}.";
                    return RedirectToAction(nameof(SportNewsList));
                }
            }

            return View(viewModel);
        }


        private bool ValidateUpdatedProperties(ReEditNewsViewModel viewModel)
        {
            var validationResults = new List<ValidationResult>();
            Validator.TryValidateProperty(viewModel.NewsTitle, new ValidationContext(viewModel, null, null) { MemberName = "NewsTitle" }, validationResults);
            Validator.TryValidateProperty(viewModel.NewsBody, new ValidationContext(viewModel, null, null) { MemberName = "NewsBody" }, validationResults);
            return validationResults.Count == 0;
        }


        private bool SportNewsExists(int id)
        {

            return (_context.SportNew?.Any(e => e.NewsId == id)).GetValueOrDefault();
        }

        [Authorize(Roles = ("News Administrator"))]
        public async Task<IActionResult> ApproveNews(int? newsId)
        {
            if (newsId == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var overallNewsReports = await _context.OverallNewsReports
               .FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportNews = await _context.SportNew.FindAsync(newsId);
            if (sportNews == null)
            {
                return NotFound();
            }

            overallNewsReports.ApprovedNewsCount++;
            overallNewsReports.PublishedNewsCount++;

            sportNews.NewsStatus = NewsStatus.Approved;
            sportNews.PublishedDate = DateTime.Now;
            sportNews.PublishedById = userId;

            await _context.SaveChangesAsync();

            TempData["Message"] = $"You have successfully approved the sport news with heading {sportNews.NewsHeading}.";

            await _activityLogger.Log($"Approved sportnews with heading {sportNews.NewsHeading}", user.Id);
            return RedirectToAction(nameof(AwaitingApprovalSportNews));
        }

        [Authorize(Roles = ("News Administrator"))]
        [HttpGet]
        public async Task<IActionResult> AskReEditNews(int? newsId)
        {
            if (newsId == null)
            {
                return NotFound();
            }

            var sportNews = await _context.SportNew
                .Where(s => s.NewsId == newsId)
                .Include( s => s.AuthoredBy)
                .FirstOrDefaultAsync();

            var viewModel = new AskForReEditNewsViewModel
            {
                NewsId = sportNews.NewsId,
                NewsTitle = sportNews.NewsHeading,
                NewsImage = sportNews.NewsImage,
                NewsBody = sportNews.NewsBody,
            };

            return View(viewModel);
        }

        [Authorize(Roles = ("News Administrator"))]
        [HttpPost]
        public async Task<IActionResult> AskReEditNews(int? newsId, AskForReEditNewsViewModel viewModel)
        {
            if (newsId == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportNews = await _context.SportNew.FindAsync(newsId);
            if (sportNews == null)
            {
                return NotFound();
            }

            var author = await _userManager.FindByIdAsync(sportNews.AuthoredById);
            if (author == null)
            {
                return NotFound();
            }

            sportNews.NewsStatus = NewsStatus.ToBeModified;
            sportNews.ModifiedDateTime = DateTime.Now;
            sportNews.ModifiedById = userId;
            sportNews.ReasonForReEdit = viewModel.ReasonForReEdit;

            _context.Update(sportNews);
            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Sent news with heading {sportNews.NewsHeading} back to author for modification", user.Id);


            var subject = "Request for News Re-Editing";
            var emailBodyTemplate = $@"
        Dear {author.FirstName} {author.LastName},<br/><br/>
        The news item with the heading <b>{sportNews.NewsHeading}</b> has been sent back for re-editing.<br/><br/>
        Please make the necessary modifications and resubmit the news.<br/><br/>
        If you have any questions or need further clarification, please contact us at support@ksfoundation.com.<br/><br/>
        Regards,<br/>
        K&S Foundation Support Team
            ";

            await _emailService.SendEmailAsync(author.Email, subject, emailBodyTemplate);

            TempData["Message"] = $"You have successfully sent the news back to the author for re-editing: {sportNews.NewsHeading}.";
            return RedirectToAction(nameof(AwaitingApprovalSportNews));
        }

        [Authorize(Roles = ("News Administrator"))]
        public async Task<IActionResult> DeclineNews(int? newsId)
        {
            if (newsId == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var overallNewsReports = await _context.OverallNewsReports
                 .FirstOrDefaultAsync();

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportNews = await _context.SportNew.FindAsync(newsId);
            if (sportNews == null)
            {
                return NotFound();
            }

            overallNewsReports.RejectedNewsCount++;
            sportNews.NewsStatus = NewsStatus.Rejected;
            sportNews.RejectedDateTime = DateTime.Now;
            sportNews.RejectedById = userId;   

            await _context.SaveChangesAsync();
            TempData["Message"] = $"You have declined the sport news with heading {sportNews.NewsHeading}.";
            await _activityLogger.Log($"Declined sportnews with heading {sportNews.NewsHeading}", user.Id);
            return RedirectToAction(nameof(AwaitingApprovalSportNews));
        }

        [Authorize(Roles = ("News Updator"))]
        public async Task<IActionResult> DeleteSportNews(int? newsId)
        {
            if (newsId == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            var sportNew = await _context.SportNew.FindAsync(newsId);

            await _activityLogger.Log($"Deleted news with heading {sportNew.NewsHeading}", user.Id);

            TempData["Message"] = $"You have deleted sport news with heading {sportNew.NewsHeading}.";

            _context.Remove(sportNew);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(SportNewsList));
        }

    }
}
