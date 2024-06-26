using System;
using System.Collections.Generic;
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


        public async Task<IActionResult> PublishedSportNews()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Approved)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
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
                .ToListAsync();

            return View(sportsNews);
        }

        // GET: AwaitingApprovalSportNews
        public async Task<IActionResult> AwaitingApprovalSportNewsAdmin()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Awaiting_Approval)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .ToListAsync();

            return PartialView("_PendingSportNewsPartial",sportsNews);
        }

        // GET: AwaitingApprovalSportNews
        public async Task<IActionResult> AwaitingApprovalSportNews()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Awaiting_Approval)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .ToListAsync();

            return View(sportsNews);
        }

        // GET: RejectedSportNews
        public async Task<IActionResult> RejectedSportNewsAdmin()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Rejected)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .ToListAsync();

            return PartialView("_RejectedSportNewsPartial",sportsNews);
        }

        // GET: RejectedSportNews
        public async Task<IActionResult> RejectedSportNews()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Rejected)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .ToListAsync();

            return View(sportsNews);
        }

        // GET: SportNews
        public async Task<IActionResult> SportNewsBackOffice()
        {
            var sportsNews = await _context.SportNew
                 .Where(s => s.NewsStatus == NewsStatus.Approved)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .ToListAsync();

            return View(sportsNews);
        }

        // GET: SportNews
        public async Task<IActionResult> Index()
        {
            var sportsNews = await _context.SportNew
                .Where(s => s.NewsStatus == NewsStatus.Approved)
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .ToListAsync();

            return PartialView("_SportsNewsPartial", sportsNews);
        }


        // GET: SportNews
        public async Task<IActionResult> SportNews()
        {
            var sportsNews = await _context.SportNew
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


        // GET: SportNews/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SportNews/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SportNewsViewModel viewModel, IFormFile NewsImages)
        {
            if (ModelState.IsValid)
            {
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
                    // Upload the club badge using the FileUploadService
                    var uploadedImagePath = await _fileUploadService.UploadFileAsync(NewsImages);

                    // Assuming you want to save the image path in your Club entity
                    sportNews.NewsImage = uploadedImagePath;
                }

                sportNews.NewsStatus = NewsStatus.Awaiting_Approval;
                _context.Add(sportNews);
                await _context.SaveChangesAsync();

                var savedNews = await _context.SportNew
                    .Where(s => s.Equals(sportNews))
                    .FirstOrDefaultAsync();

                await _activityLogger.Log($"Authored sport news with heading {savedNews.NewsHeading}", user.Id);
                return RedirectToAction(nameof(AwaitingApprovalSportNews));
            }
            return View(viewModel);
        }

        // GET: SportNews/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var sportNews = await _context.SportNew.FindAsync(id);
            if (sportNews == null)
            {
                return NotFound();
            }
            ViewData["AuthoredById"] = new SelectList(_context.Users, "Id", "Id", sportNews.AuthoredById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", sportNews.ModifiedById);
            ViewData["PublishedById"] = new SelectList(_context.Users, "Id", "Id", sportNews.PublishedById);
            return View(sportNews);
        }

        // POST: SportNews/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("NewsId,NewsHeading,PublishedDate,ModifiedDateTime,ModifiedById,AuthoredById,PublishedById,NewsBody,NewsImage")] SportNews sportNews)
        {
            if (id != sportNews.NewsId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sportNews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SportNewsExists(sportNews.NewsId))
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
            ViewData["AuthoredById"] = new SelectList(_context.Users, "Id", "Id", sportNews.AuthoredById);
            ViewData["ModifiedById"] = new SelectList(_context.Users, "Id", "Id", sportNews.ModifiedById);
            ViewData["PublishedById"] = new SelectList(_context.Users, "Id", "Id", sportNews.PublishedById);
            return View(sportNews);
        }

        // GET: SportNews/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var sportNews = await _context.SportNew
                .Include(s => s.AuthoredBy)
                .Include(s => s.ModifiedBy)
                .Include(s => s.PublishedBy)
                .FirstOrDefaultAsync(m => m.NewsId == id);
            if (sportNews == null)
            {
                return NotFound();
            }

            return View(sportNews);
        }

        // POST: SportNews/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SportNew == null)
            {
                return Problem("Entity set 'Sports_ManagerDbContext.SportNew'  is null.");
            }
            var sportNews = await _context.SportNew.FindAsync(id);
            if (sportNews != null)
            {
                _context.SportNew.Remove(sportNews);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
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

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportNews = await _context.SportNew.FindAsync(newsId);
            if (sportNews == null)
            {
                return NotFound();
            }

            sportNews.NewsStatus = NewsStatus.Approved;
            sportNews.PublishedDate = DateTime.UtcNow;
            sportNews.PublishedById = userId;

            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Approved sportnews with heading {sportNews.NewsHeading}", user.Id);
            return RedirectToAction(nameof(AwaitingApprovalSportNews));
        }

        // GET: SportNews/Approve/5
        public async Task<IActionResult> DeclineNews(int? id)
        {
            if (id == null || _context.SportNew == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);
            var userId = user.Id;

            var sportNews = await _context.SportNew.FindAsync(id);
            if (sportNews == null)
            {
                return NotFound();
            }

            sportNews.NewsStatus = NewsStatus.Rejected;
            sportNews.RejectedDateTime = DateTime.UtcNow;
            sportNews.RejectedById = userId;   

            await _context.SaveChangesAsync();

            await _activityLogger.Log($"Declined sportnews with heading {sportNews.NewsHeading}", user.Id);
            return RedirectToAction(nameof(AwaitingApprovalSportNews));
        }

    }
}
