using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
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

        public SportNewsController(Ksans_SportsDbContext context, 
            UserManager<UserBaseModel> userManager, 
            FileUploadService fileUploadService,
            IActivityLogger activityLogger)
        {
            _context = context;
            _userManager = userManager;
            _fileUploadService = fileUploadService;
            _activityLogger = activityLogger;   
        }

        public async Task<IActionResult> SportNewsIndex()
        {
            return PartialView("NewsPartial");
        }

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


        public async Task<IActionResult> PublishedSportNews()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Approved)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.PublishedDate)
                .ToListAsync();

            return PartialView("_PublishedSportNewsPartial",sportsNews);
        }


        public async Task<IActionResult> SportNewsList()
        {
            return View();
        }

        public async Task<IActionResult> ApprovedSportNewsAdmin()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Approved)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.ModifiedDateTime)
                .ToListAsync();

            return PartialView("_ApprovedSportNewsPartial", sportsNews);
        }




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


        public async Task<IActionResult> AwaitingApprovalSportNewsAdmin()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Awaiting_Approval)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.ModifiedDateTime)
                .ToListAsync();

            return PartialView("_PendingSportNewsPartial",sportsNews);
        }

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

        public async Task<IActionResult> RejectedSportNewsAdmin()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Rejected)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .OrderByDescending(n => n.RejectedDateTime)
                .ToListAsync();

            return PartialView("_RejectedSportNewsPartial",sportsNews);
        }

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

        public IActionResult Create()
        {
            return View();
        }

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
                NewsBody = existingNews.NewsBody
            };

            return View(viewModel);
        }


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
            }

            _context.Update(existingNews);
            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Updated sport news with heading {existingNews.NewsHeading}", user.Id);

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

        // GET: SportNews/Approve/5
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

        public async Task<IActionResult> AskReEditNews(int? newsId)
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


            sportNews.NewsStatus = NewsStatus.ToBeModified;
            sportNews.ModifiedDateTime = DateTime.Now;
            sportNews.ModifiedById = userId;

            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Sent news with heading {sportNews.NewsHeading} back to author for modification",  user.Id);

            TempData["Message"] = $"You have successfully sent news back to the author for re-edit the sport news with heading {sportNews.NewsHeading}.";
            return RedirectToAction(nameof(AwaitingApprovalSportNews));
        }

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
