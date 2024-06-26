// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using MyField.Interfaces;
using MyField.Models;

namespace MyField.Areas.Identity.Pages.Account
{
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<UserBaseModel> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly IActivityLogger _activityLogger;
        private readonly UserManager<UserBaseModel> _userManager;

        public LogoutModel(SignInManager<UserBaseModel> signInManager, 
            ILogger<LogoutModel> logger,
            IActivityLogger activityLogger,
            UserManager<UserBaseModel> userManager)
        {
            _signInManager = signInManager;
            _logger = logger;
            _activityLogger = activityLogger;
            _userManager = userManager;
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                var user = await _userManager.GetUserAsync(User);
                await _activityLogger.Log("Logged out", user.Id);
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
