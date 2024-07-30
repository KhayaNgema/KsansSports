﻿using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using MyField.Data;
using MyField.Interfaces;
using MyField.Models;
using MyField.Services;
using MyField.ViewModels;
using System.Linq;


namespace MyField.Controllers
{
    public class LivesController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly FileUploadService _fileUploadService;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;
        private readonly IEncryptionService _encryptionService;
        private readonly IServiceProvider _serviceProvider;

        public LivesController(Ksans_SportsDbContext context,
            FileUploadService fileUploadService,
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger,
            IEncryptionService encryptionService,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _fileUploadService = fileUploadService;
            _userManager = userManager;
            _activityLogger = activityLogger;
            _encryptionService = encryptionService;
            _serviceProvider = serviceProvider;
        }

        [HttpGet]
        public async Task<IActionResult> MatchOverview(int fixtureId)
        {
            return PartialView("_OverviewPartial");
        }

        [HttpGet]
        public async Task<IActionResult> StartLive(string fixtureId)
        {
            var decryptedFixtureId = _encryptionService.DecryptToInt(fixtureId);

            var fixture = await _context.Fixture
                .Where(f => f.FixtureId == decryptedFixtureId)
                .Include(f => f.HomeTeam)
                .Include(f => f.AwayTeam)
                .FirstOrDefaultAsync();

            if (fixture == null)
            {
                return NotFound("Fixture not found.");
            }

            var startLiveViewModel = new StartLiveViewModel
            {
                FixtureId = decryptedFixtureId,
                FixturedClubs = $"{fixture.HomeTeam.ClubName} vs {fixture.AwayTeam.ClubName}",
                KickOffDate = fixture.KickOffDate,
                KickOffTime = fixture.KickOffTime,
                HomeTeamId = fixture.HomeTeamId,
                AwayTeamId = fixture.AwayTeamId,
                HomeTeamName = fixture.HomeTeam.ClubName,
                HomeTeamBadge = fixture.HomeTeam.ClubBadge,
                AwayTeamName = fixture.AwayTeam.ClubName,
                AwayTeamBadge = fixture.AwayTeam.ClubBadge,
                LiveTime = 0,
                HomeTeamScore = 0,
                AwayTeamScore = 0
            };

            var homePlayers = await _context.Player
                .Where(p => p.ClubId == fixture.HomeTeamId)
                .Select(p => new
                {
                    PlayerId = p.Id,
                    FullName = $"{p.FirstName} {p.LastName}",
                    JerseyNumber = p.JerseyNumber
                })
                .ToListAsync();


            var awayPlayers = await _context.Player
               .Where(p => p.ClubId == fixture.AwayTeamId)
               .Select(p => new
               {
               PlayerId = p.Id,
               FullName = $"{p.FirstName} {p.LastName}",
               JerseyNumber = p.JerseyNumber
               })
               .ToListAsync();

            var homeLineUpXI = await _context.LineUpXI
               .Where(l => l.FixtureId == decryptedFixtureId && l.ClubId == fixture.HomeTeam.ClubId)
               .Include(l => l.ClubPlayer)
               .Select(l => new
               {
               PlayerId = l.ClubPlayer.Id,
               FullName = $"{l.ClubPlayer.FirstName} {l.ClubPlayer.LastName}",
               JerseyNumber = l.ClubPlayer.JerseyNumber
               })
              .ToListAsync();

            var homeLineUpSubstitutes = await _context.LineUpSubstitutes
                .Where(l => l.FixtureId == decryptedFixtureId && l.ClubId == fixture.HomeTeam.ClubId)
                .Include(l => l.ClubPlayer)
                .Select(l => new
                {
                    PlayerId = l.ClubPlayer.Id,
                    FullName = $"{l.ClubPlayer.FirstName} {l.ClubPlayer.LastName}",
                    JerseyNumber = l.ClubPlayer.JerseyNumber
                })
                .ToListAsync();

            var awayLineUpXI = await _context.LineUpXI
                .Where(l => l.FixtureId == decryptedFixtureId && l.ClubId == fixture.AwayTeam.ClubId)
                .Include(l => l.ClubPlayer)
                .Select(l => new
                {
                    PlayerId = l.ClubPlayer.Id,
                    FullName = $"{l.ClubPlayer.FirstName} {l.ClubPlayer.LastName}",
                    JerseyNumber = l.ClubPlayer.JerseyNumber
                })
                .ToListAsync();

            var awayLineUpSubstitutes = await _context.LineUpSubstitutes
                .Where(l => l.FixtureId == decryptedFixtureId && l.ClubId == fixture.AwayTeam.ClubId)
                .Include(l => l.ClubPlayer)
                .Select(l => new
                {
                    PlayerId = l.ClubPlayer.Id,
                    FullName = $"{l.ClubPlayer.FirstName} {l.ClubPlayer.LastName}",
                    JerseyNumber = l.ClubPlayer.JerseyNumber
                })
                .ToListAsync();



            var homeGoalCombinedViewModel = new HomeGoalCombinedViewModel
            {
                Players = homePlayers.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                HomeTeam = fixture.HomeTeam.ClubName
            };

            var awayGoalCombinedViewModel = new AwayGoalCombinedViewModel
            {
                Players = awayPlayers.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                AwayTeam = fixture.AwayTeam.ClubName
            };

            var homeYellowViewModel = new HomeYellowViewModel
            {
                Players = homePlayers.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                FixtureId = fixture.FixtureId,

                HomeTeam = fixture.HomeTeam.ClubName
            };

            var awayYellowViewModel = new AwayYellowViewModel
            {
                Players = awayPlayers.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                FixtureId = fixture.FixtureId,

                AwayTeam = fixture.AwayTeam.ClubName
            };


            var homeRedViewModel = new HomeRedViewModel
            {
                Players = homePlayers.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                FixtureId = fixture.FixtureId,

                HomeTeam = fixture.HomeTeam.ClubName
            };

            var awayRedViewModel = new AwayRedViewModel
            {
                Players = awayPlayers.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                FixtureId = fixture.FixtureId,

                AwayTeam = fixture.AwayTeam.ClubName
            };


            var penaltyTypes = Enum.GetValues(typeof(PenaltyType))
                  .Cast<PenaltyType>()
                  .Select(p => new SelectListItem { Value = p.ToString(), Text = p.ToString() })
                  .ToList();

            var homePenaltyViewModel = new HomePenaltyViewModel
            {
                Players = awayPlayers.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                FixtureId = fixture.FixtureId,

                HomeTeam = fixture.HomeTeam.ClubName,

                PenaltyTypes = penaltyTypes
            };

            var awayPenaltyViewModel = new AwayPenaltyViewModel
            {
                Players = homePlayers.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                FixtureId = fixture.FixtureId,

                AwayTeam = fixture.AwayTeam.ClubName,

                PenaltyTypes = penaltyTypes
            };


            var homeSubViewModel = new HomeSubViewModel
            {
                StartingXi = homeLineUpXI.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                Substitutes = homeLineUpSubstitutes.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                FixtureId = fixture.FixtureId,

                HomeTeam = fixture.HomeTeam.ClubName
            };

            var awaySubViewModel = new AwaySubViewModel
            {
                StartingXi = awayLineUpXI.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                Substitutes = awayLineUpSubstitutes.Select(p => new
                {
                    p.PlayerId,
                    p.FullName,
                    p.JerseyNumber
                }).ToList(),

                FixtureId = fixture.FixtureId,

                AwayTeam = fixture.AwayTeam.ClubName,
            };


            var user = await _userManager.GetUserAsync(User);

            var userRole = await _context.UserRoles
                 .Where(ur => ur.UserId == user.Id)
                 .Join(_context.Roles,
                 ur => ur.RoleId,
                 r => r.Id,
                 (ur, r) => r.Name)
                .FirstOrDefaultAsync();

            var overviewViewModel = new OverviewViewModel
            {
                FixtureId = decryptedFixtureId
            };

            var combinedViewModel = new CombinedStartLiveViewModel
            {
                StartLiveViewModel = startLiveViewModel,
                HomeGoalCombinedViewModel = homeGoalCombinedViewModel,
                AwayGoalCombinedViewModel = awayGoalCombinedViewModel,
                HomeYellowViewModel = homeYellowViewModel,
                AwayYellowViewModel = awayYellowViewModel,
                HomeRedViewModel = homeRedViewModel,
                AwayRedViewModel = awayRedViewModel,
                HomePenaltyViewModel = homePenaltyViewModel,
                AwayPenaltyViewModel = awayPenaltyViewModel,
                HomeSubViewModel = homeSubViewModel,
                AwaySubViewModel = awaySubViewModel,
                AddedTime = 0,
                UserRole = userRole,
                OverviewViewModel = overviewViewModel,
            };

            return View(combinedViewModel);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartMatch([FromBody] StartLiveViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingLiveMatch = await _context.Live
                .Where(lm => lm.FixtureId == viewModel.FixtureId)
                .FirstOrDefaultAsync();

            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();

            if (existingLiveMatch != null)
            {
                if (existingLiveMatch.ISEnded)
                {
                    TempData["Message"] = "Cannot start match, it has already ended!";
                    return BadRequest(new { success = false, message = TempData["Message"] });
                }

                TempData["Message"] = "Match already started!";
                return BadRequest(new { success = false, message = TempData["Message"] });
            }

            var newLive = new Live
            {
                FixtureId = viewModel.FixtureId,
                LeagueId = currentSeason.LeagueId,
                HomeTeamScore = viewModel.HomeTeamScore,
                AwayTeamScore = viewModel.AwayTeamScore,
                ISEnded = false,
                IsHalfTime = false,
                IsLive = true,
                LiveTime = 0
            };

            _context.Live.Add(newLive);
            await _context.SaveChangesAsync();

            RecurringJob.AddOrUpdate(
                $"update-live-time-{newLive.LiveId}",
                () => UpdateLiveTime(newLive.LiveId),
                Cron.Minutely);

            TempData["Message"] = "Live match started successfully!";
            return Ok(new { success = true, liveId = newLive.LiveId, message = TempData["Message"] });
        }


        public async Task UpdateLiveTime(int liveId)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<Ksans_SportsDbContext>();

                var live = await context.Live.FindAsync(liveId);
                if (live == null || !live.IsLive) return;

                live.LiveTime++;
                context.Update(live);
                await context.SaveChangesAsync();
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetLiveMatchStatus(int fixtureId)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId)
                .FirstOrDefaultAsync();

            if (liveMatch == null)
            {
                return NotFound();
            }

            var response = new
            {
                LiveTime = liveMatch.LiveTime,
                IsLive = liveMatch.IsLive,
                IsHalfTime = liveMatch.IsHalfTime,
                IsEnded = liveMatch.ISEnded,
                WentToHalfTime = liveMatch.WentToHalfTime,
                HomeTeamScore = liveMatch.HomeTeamScore,
                AwayTeamScore = liveMatch.AwayTeamScore,
                AddTime = liveMatch.AddedTime,
            };

            return Ok(response);
        }



        [HttpPost]
        public async Task<IActionResult> HalfTime(int fixtureId)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId)
                .FirstOrDefaultAsync();

            if (liveMatch != null)
            {
                liveMatch.IsLive = false;
                liveMatch.IsHalfTime = true;
                liveMatch.ISEnded = false;
                liveMatch.LiveTime = 45;
                liveMatch.WentToHalfTime = true;
                liveMatch.AddedTime = 0;

                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound("Live match not found.");
        }


        [HttpPost]
        public async Task<IActionResult> ResumeMatch(int fixtureId)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId)
                .FirstOrDefaultAsync();

            if (liveMatch != null)
            {
                liveMatch.IsLive = true;
                liveMatch.IsHalfTime = false;
                liveMatch.ISEnded = false;
                liveMatch.LiveTime = 45;

                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok();
            }

            return NotFound("Live match not found.");
        }

        [HttpGet]
        public async Task<IActionResult> HomeGoal()
        {
            return PartialView("_HomeGoalPartial");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> HomeGoal(int fixtureId, string goalScoredBy, string assistedBy, string scoredTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();


                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if(!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                liveMatch.HomeTeamScore++;

                var newGoal = new LiveGoalHolder
                {
                    LeagueId = liveMatch.LeagueId,
                    ScoredById= goalScoredBy,
                    LiveId = liveMatch.LiveId,
                    ScoredTime = scoredTime,
                    RecordedTime = DateTime.Now
                };

                var newAssist = new LiveAssistHolder
                {
                    AssistedById = assistedBy,
                    LeagueId = liveMatch.LeagueId,
                    LiveId = liveMatch.LiveId,
                    RecordedTime = DateTime.Now

                };

                _context.Add(newGoal);
                _context.Add(newAssist);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }





        [HttpGet]
        public async Task<IActionResult> AwayGoal()
        {
           
            return PartialView("_AwayGoalPartial");
        }

        [HttpPost]
        public async Task<IActionResult> AwayGoal(int fixtureId, string goalScoredBy, string assistedBy, string scoredTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                liveMatch.AwayTeamScore++;

                var newGoal = new LiveGoalHolder
                {
                    LeagueId = liveMatch.LeagueId,
                    ScoredById = goalScoredBy,
                    LiveId = liveMatch.LiveId,
                    ScoredTime = scoredTime,
                    RecordedTime = DateTime.Now
                };

                var newAssist = new LiveAssistHolder
                {
                    AssistedById = assistedBy,
                    LiveId = liveMatch.LiveId,
                    LeagueId = liveMatch.LeagueId,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newGoal);
                _context.Add(newAssist);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> HomeYellow()
        {
            return PartialView("_HomeYellowPartial");
        }

        [HttpPost]
        public async Task<IActionResult> HomeYellow(int fixtureId, string commitedBy, string cardTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                var newYellowCard = new LiveYellowCardHolder
                {
                    LeagueId = liveMatch.LeagueId,
                    YellowCommitedById = commitedBy,
                    LiveId = liveMatch.LiveId,
                    YellowCardTime  = cardTime,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newYellowCard);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> AwayYellow()
        {
            return PartialView("_AwayYellowPartial");
        }

        [HttpPost]
        public async Task<IActionResult> AwayYellow(int fixtureId, string commitedBy, string cardTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                var newYellowCard = new LiveYellowCardHolder
                {
                    LeagueId = liveMatch.LeagueId,
                    YellowCommitedById = commitedBy,
                    LiveId = liveMatch.LiveId,
                    YellowCardTime = cardTime,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newYellowCard);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> HomeRed()
        {
            return PartialView("_HomeRedPartial");
        }

        [HttpPost]
        public async Task<IActionResult> HomeRed(int fixtureId, string commitedBy, string cardTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                var newRedCard = new LiveRedCardHolder
                {
                    LeagueId = liveMatch.LeagueId,
                    RedCommitedById = commitedBy,
                    LiveId = liveMatch.LiveId,
                    RedCardTime = cardTime,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newRedCard);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> AwayRed()
        {
            return PartialView("_AwayRedPartial");
        }

        [HttpPost]
        public async Task<IActionResult> AwayRed(int fixtureId, string commitedBy, string cardTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                var newRedCard = new LiveRedCardHolder
                {
                    LeagueId = liveMatch.LeagueId,
                    RedCommitedById = commitedBy,
                    LiveId = liveMatch.LiveId,
                    RedCardTime = cardTime,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newRedCard);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> HomePenalty()
        {

            return PartialView("_HomePenaltyPartial");
        }

        [HttpPost]
        public async Task<IActionResult> HomePenalty(int fixtureId, string commitedBy, string penaltyTime, PenaltyType penaltyType, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                var newPenalty = new Penalty
                {
                    LeagueId = liveMatch.LeagueId,
                    PlayerId = commitedBy,
                    LiveId = liveMatch.LiveId,
                    PenaltyTime = penaltyTime,
                    Type = penaltyType,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newPenalty);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> AwayPenalty()
        {
            return PartialView("_AwayPenaltyPartial");
        }

        [HttpPost]
        public async Task<IActionResult> AwayPenalty(int fixtureId, string commitedBy, string penaltyTime, PenaltyType penaltyType, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                var newPenalty = new Penalty
                {
                    LeagueId = liveMatch.LeagueId,
                    PlayerId = commitedBy,
                    LiveId = liveMatch.LiveId,
                    PenaltyTime = penaltyTime,
                    Type = penaltyType,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newPenalty);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> HomeSub()
        {
            return PartialView("_HomeSubPartial");
        }

        [HttpPost]
        public async Task<IActionResult> HomeSub(int fixtureId, string outPlayer, string inPlayer, string subTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                var playerPerformanceReport = await _context.PlayerPerformanceReports
                    .Where(p => p.PlayerId == inPlayer)
                    .FirstOrDefaultAsync();

                playerPerformanceReport.AppearancesCount++;

                _context.Update(playerPerformanceReport);
                await _context.SaveChangesAsync();


                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                var newSub = new Substitute
                {
                    LeagueId = liveMatch.LeagueId,
                    InPlayerId = inPlayer,
                    LiveId = liveMatch.LiveId,
                    OutPlayerId = outPlayer,
                    SubTime = subTime,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newSub);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }


        [HttpGet]
        public async Task<IActionResult> AwaySub()
        {
            return PartialView("_AwaySubPartial");
        }

        [HttpPost]
        public async Task<IActionResult> AwaySub(int fixtureId, string outPlayer, string inPlayer, string subTime, StartLiveViewModel viewModel)
        {
            try
            {
                Console.WriteLine($"Received Fixture ID: {fixtureId}");

                var liveMatch = await _context.Live
                    .Where(l => l.FixtureId == fixtureId && l.IsLive)
                    .Include(l => l.League)
                    .FirstOrDefaultAsync();

                var playerPerformanceReport = await _context.PlayerPerformanceReports
                    .Where(p => p.PlayerId == inPlayer)
                    .FirstOrDefaultAsync();

                playerPerformanceReport.AppearancesCount++;

                _context.Update(playerPerformanceReport);
                await _context.SaveChangesAsync();

                if (liveMatch == null)
                {
                    Console.WriteLine("Live match not found.");
                    return BadRequest(new { success = false, message = "Live match not found." });
                }

                if (!liveMatch.IsLive)
                {

                    return View(viewModel);
                }

                var newSub = new Substitute
                {
                    LeagueId = liveMatch.LeagueId,
                    InPlayerId = inPlayer,
                    LiveId = liveMatch.LiveId,
                    OutPlayerId = outPlayer,
                    SubTime = subTime,
                    RecordedTime = DateTime.Now
                };

                _context.Add(newSub);
                _context.Update(liveMatch);
                await _context.SaveChangesAsync();

                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return StatusCode(500, new { success = false, message = "An error occurred while processing your request." });
            }
        }

        [HttpGet]
        public async Task<IActionResult> AddTime()
        {
            return PartialView("_AddTimePartial");
        }

        [HttpPost]
        public async Task<IActionResult> AddTime(int fixtureId, int addedTime)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId && l.IsLive)
                .FirstOrDefaultAsync();


            liveMatch.AddedTime = addedTime;

            _context.Update(liveMatch);
            await _context.SaveChangesAsync();

            return Ok(new {success = true});
        }


        [HttpGet]
        public async Task<IActionResult> GetEvents(int fixtureId)
        {
            var liveMatch = await _context.Live
                .Where(l => l.FixtureId == fixtureId && l.IsLive)
                .FirstOrDefaultAsync();

            if (liveMatch == null)
            {
                return NotFound("Live match not found.");
            }

            var liveGoals = await _context.LiveGoalHolders
                .Where(g => g.LiveId == liveMatch.LiveId)
                .Include(g => g.ScoredBy)
                .Select(g => new
                {
                    ScoreBy = g.ScoredBy != null
                        ? $"{g.ScoredBy.FirstName[0]}. {g.ScoredBy.LastName}"
                        : "Unknown",
                    ScoredTime = g.ScoredTime,
                    RecordedTime = g.RecordedTime,
                })
                .OrderByDescending(g => g.RecordedTime) // Sort by RecordedTime in descending order
                .ToListAsync();

            var liveAssists = await _context.LiveAssistHolders
                .Where(a => a.LiveId == liveMatch.LiveId)
                .Include(a => a.AssistedBy)
                .Select(a => new
                {
                    AssistedBy = a.AssistedBy != null
                        ? $"{a.AssistedBy.FirstName[0]}. {a.AssistedBy.LastName}"
                        : "Unknown",
                    RecordedTime = a.RecordedTime,
                })
                .OrderByDescending(a => a.RecordedTime) // Sort by RecordedTime in descending order
                .ToListAsync();

            var yellowCards = await _context.LiveYellowCardHolders
                .Where(y => y.LiveId == liveMatch.LiveId)
                .Include(y => y.YellowCommitedBy)
                .Select(y => new
                {
                    IssuedTime = y.YellowCardTime,
                    IssuedTo = y.YellowCommitedBy != null
                        ? $"{y.YellowCommitedBy.FirstName[0]}. {y.YellowCommitedBy.LastName}"
                        : "Unknown",
                    RecordedTime = y.RecordedTime,
                })
                .OrderByDescending(y => y.RecordedTime) // Sort by RecordedTime in descending order
                .ToListAsync();

            var redCards = await _context.LiveRedCardHolders
                .Where(r => r.LiveId == liveMatch.LiveId)
                .Include(r => r.RedCommitedBy)
                .Select(r => new
                {
                    IssuedTime = r.RedCardTime,
                    IssuedTo = r.RedCommitedBy != null
                        ? $"{r.RedCommitedBy.FirstName[0]}. {r.RedCommitedBy.LastName}"
                        : "Unknown",
                    RecordedTime = r.RecordedTime,
                })
                .OrderByDescending(r => r.RecordedTime) // Sort by RecordedTime in descending order
                .ToListAsync();

            var penalties = await _context.Penalties
                .Where(p => p.LiveId == liveMatch.LiveId)
                .Include(p => p.Player)
                .Select(p => new
                {
                    PenaltyTime = p.PenaltyTime,
                    TakenBy = p.Player != null
                        ? $"{p.Player.FirstName[0]}. {p.Player.LastName}"
                        : "Unknown",
                    PenaltyType = p.Type,
                    RecordedTime = p.RecordedTime,
                })
                .OrderByDescending(p => p.RecordedTime) // Sort by RecordedTime in descending order
                .ToListAsync();

            var substitutes = await _context.Substitutes
                .Where(s => s.LiveId == liveMatch.LiveId)
                .Include(s => s.InPlayer)
                .Include(s => s.OutPlayer)
                .Select(s => new
                {
                    PlayerIn = s.InPlayer != null
                        ? $"{s.InPlayer.FirstName[0]}. {s.InPlayer.LastName}"
                        : "Unknown",
                    PlayerOut = s.OutPlayer != null
                        ? $"{s.OutPlayer.FirstName[0]}. {s.OutPlayer.LastName}"
                        : "Unknown",
                    SubstitutionTime = s.SubTime,
                    RecordedTime = s.RecordedTime,
                })
                .OrderByDescending(s => s.RecordedTime) // Sort by RecordedTime in descending order
                .ToListAsync();

            // Return the sorted events grouped by type
            var events = new
            {
                liveGoals,
                liveAssists,
                yellowCards,
                redCards,
                penalties,
                substitutes
            };

            return Ok(events);
        }



    }


}
