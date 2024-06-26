using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    public class LeaguesController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;

        public LeaguesController(Ksans_SportsDbContext context,
            IMapper mapper,
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _activityLogger = activityLogger;
        }

        public async Task<IActionResult> Leagues()
        {
            var leagues = _context.League
                .OrderByDescending(l => l.CreatedDateTime)
                .Include(s =>s.CreatedBy)
                .Include(s => s.ModifiedBy)
                .ToList();
            return View(leagues);
        }

        public IActionResult StartLeague()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartLeague(LeagueViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = user.Id;

                var oldLeague = await _context.League.FirstOrDefaultAsync(l => l.IsCurrent);

                var oldTransferPeriod = await _context.TransferPeriod.FirstOrDefaultAsync(l => l.IsCurrent);

                if (oldLeague != null && oldLeague.LeagueCode != viewModel.OldLeagueCode)
                {
                    TempData["Message"] = "League code does not match the league you are trying to end.";
                    return View(viewModel);
                }

                if(oldTransferPeriod != null)
                {
                    await _activityLogger.Log($"Ended season {oldLeague.LeagueYears}", user.Id);
                    oldTransferPeriod.IsCurrent = false;
                    _context.Update(oldLeague);
                    await _context.SaveChangesAsync();
                }

                if (oldLeague != null)
                {
                    oldLeague.IsCurrent = false;
                    _context.Update(oldLeague);
                    await _context.SaveChangesAsync();

                    var oldStandings = await _context.Standing.Where(s => s.LeagueId == oldLeague.LeagueId).ToListAsync();
                    var oldMatchResults = await _context.MatchResult.Where(m => m.LeagueId == oldLeague.LeagueId).ToListAsync();
                    var oldFixtures = await _context.Fixture.Where(f => f.LeagueId == oldLeague.LeagueId).ToListAsync();
                    var oldClubs = await _context.Club.Where(c => c.LeagueId == oldLeague.LeagueId).ToListAsync();
                    var oldMatchFormations= await _context.MatchFormation.ToListAsync();

                    foreach (var s in oldStandings)
                    {
                        var archivedStanding = new Standings_Archive
                        {
                            ClubId = s.ClubId,
                            Position = s.Position,
                            MatchPlayed = s.MatchPlayed,
                            Points = s.Points,
                            Wins = s.Wins,
                            Lose = s.Lose,
                            GoalsScored = s.GoalsScored,
                            GoalsConceded = s.GoalsConceded,
                            GoalDifference = s.GoalDifference,
                            Draw = s.Draw,
                            CreatedDateTime = s.CreatedDateTime,
                            ModifiedDateTime = s.ModifiedDateTime,
                            CreatedById = s.CreatedById,
                            ModifiedById = s.ModifiedById,
                            Last5Games = s.Last5Games,
                            LeagueId = s.LeagueId
                        };
                        _context.Standings_Archive.Add(archivedStanding);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var m in oldMatchResults)
                    {
                        var archivedMatchResult = new MatchResults_Archive
                        {
                            FixtureId = m.FixtureId,
                            AwayTeamId = m.AwayTeamId,
                            HomeTeamId = m.HomeTeamId,
                            HomeTeamScore = m.HomeTeamScore,
                            AwayTeamScore = m.AwayTeamScore,
                            MatchDate = m.MatchDate,
                            CreatedDateTime = m.CreatedDateTime,
                            ModifiedDateTime = m.ModifiedDateTime,
                            CreatedById = m.CreatedById,
                            ModifiedById = m.ModifiedById,
                            Location = m.Location,
                            LeagueId = m.LeagueId
                        };
                        _context.MatchResults_Archive.Add(archivedMatchResult);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var m in oldMatchFormations)
                    {
                        var archivedMatchFormation = new MatchFormation_Archive
                        {
                            ClubId = m.ClubId,
                            FormationId = m.FormationId,
                            FixtureId = m.FixtureId,
                            CreatedDateTime = m.CreatedDateTime,
                            CreatedById = m.CreatedById,
                            ModifiedDateTime = m.ModifiedDateTime,
                            ModifiedById = m.ModifiedById,
                        };
                        _context.MatchFormation_Archive.Add(archivedMatchFormation);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var f in oldFixtures)
                    {
                        var archivedFixture = new Fixtures_Archive
                        {
                            HomeTeamId = f.HomeTeamId,
                            AwayTeamId = f.AwayTeamId,
                            KickOff = f.KickOff,
                            Location = f.Location,
                            CreatedDateTime = f.CreatedDateTime,
                            ModifiedDateTime = f.ModifiedDateTime,
                            CreatedById = f.CreatedById,
                            ModifiedById = f.ModifiedById,
                            FixtureStatus = f.FixtureStatus,
                            LeagueId = f.LeagueId
                        };
                        _context.Fixtures_Archive.Add(archivedFixture);
                        await _context.SaveChangesAsync();
                    }

                    foreach (var c in oldClubs)
                    {
                        var existingClub = await _context.Club.FindAsync(c.ClubId);

                        if (existingClub != null)
                        {
                            existingClub.IsActive = false;
                            existingClub.Status = ClubStatus.Previous_Season;

                            _context.Update(existingClub);
                            await _context.SaveChangesAsync();
                        }
                    }

                    _context.MatchFormation.RemoveRange(oldMatchFormations);
                    _context.Standing.RemoveRange(oldStandings);
                    _context.MatchResult.RemoveRange(oldMatchResults);
                    _context.Fixture.RemoveRange(oldFixtures);

                    await _context.SaveChangesAsync();
                }

                var newLeague = new League
                {
                    LeagueYears = viewModel.LeagueYears,
                    CreatedById = userId,
                    CreatedDateTime = DateTime.Now,
                    ModifiedById = userId,
                    ModifiedDateTime = DateTime.Now,
                    IsCurrent = true,
                    LeagueCode = GenerateLeagueCode()
                };

                _context.Add(newLeague);
                await _context.SaveChangesAsync();

                var newSeason = await _context.League
                       .Where(n => n.Equals(newLeague)).FirstOrDefaultAsync();

                var transferPeriod = new TransferPeriod
                {
                    LeagueId = newSeason.LeagueId,
                    PeriodOpenCount = 0,
                    IsOpened = false,
                    CreatedDateTime = DateTime.Now,
                    CreatedById = user.Id,
                    ModifiedDateTime = DateTime.Now,
                    ModifiedById = user.Id,
                    IsCurrent = true
                };

                _context.Add(transferPeriod);
                await _context.SaveChangesAsync();


                TempData["Message"] = $"{newLeague.LeagueYears} has been started successfully and all running data will be related to it. This league will be referred to as the current league.";

                await _activityLogger.Log($"Started season {newSeason.LeagueYears}", user.Id);
                return RedirectToAction(nameof(Leagues));
            }
            return View(viewModel);
        }



        private string GenerateLeagueCode()
        {
            var year = DateTime.Now.Year.ToString().Substring(2);
            var month = DateTime.Now.Month.ToString("D2");
            var day = DateTime.Now.Day.ToString("D2");

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var randomString = new string(Enumerable.Repeat(chars, 6)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            return $"{year}{month}{day}{randomString}";
        }

    }
}
