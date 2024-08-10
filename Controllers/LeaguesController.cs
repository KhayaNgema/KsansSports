using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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
    public class LeaguesController : Controller
    {
        private readonly Ksans_SportsDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<UserBaseModel> _userManager;
        private readonly IActivityLogger _activityLogger;
        private readonly EmailService _emailService;

        public LeaguesController(Ksans_SportsDbContext context,
            IMapper mapper,
            UserManager<UserBaseModel> userManager,
            IActivityLogger activityLogger,
            EmailService emailService)
        {
            _mapper = mapper;
            _context = context;
            _userManager = userManager;
            _activityLogger = activityLogger;
            _emailService = emailService;
        }

        [Authorize(Roles = ("Sport Administrator"))]

        public async Task<IActionResult> LeagueCode()
        {
            var currentSeason = await _context.League
                .Where(c => c.IsCurrent)
                .FirstOrDefaultAsync();

            return PartialView("_LeagueCodePartial", currentSeason);
        }


        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<IActionResult> ClubCodes()
        {
            var clubs = await _context.Club
                .Where(c => c.League.IsCurrent)
                .ToListAsync();

            return PartialView("_ClubCodesPartial", clubs);
        }

        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<IActionResult> Leagues()
        {
            var leagues = _context.League
                .OrderByDescending(l => l.CreatedDateTime)
                .Include(s =>s.CreatedBy)
                .Include(s => s.ModifiedBy)
                .ToList();
            return View(leagues);
        }

        [Authorize(Roles = ("Sport Administrator"))]
        public async Task<IActionResult> SecretCodes()
        {
            return View();
        }

        [Authorize(Roles = ("Sport Administrator"))]
        public IActionResult StartLeague()
        {
            return View();
        }

        [Authorize(Roles = ("Sport Administrator"))]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartLeague(LeagueViewModel viewModel)
        {
            try
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

                    if (oldTransferPeriod != null)
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
                        var oldMatchFormations = await _context.MatchFormation.ToListAsync();
                        var oldMatchReports = await _context.MatchReports.Where(m => m.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldMatchResultsReports = await _context.MatchResultsReports.Where(r => r.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldTransfersReports = await _context.TransfersReports.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldClubTransfersReports = await _context.ClubTransferReports.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldClubPerformanceReports = await _context.ClubPerformanceReports.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldPlayerPerformanceReports = await _context.PlayerPerformanceReports.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldLives = await _context.Live.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldTopScores = await _context.TopScores.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldTopAssists = await _context.TopAssists.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldLiveGoals = await _context.LiveGoals.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldSubstitutes = await _context.Substitutes.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldYellowCards = await _context.YellowCards.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldRedCards = await _context.RedCards.Where(t => t.LeagueId == oldLeague.LeagueId).ToListAsync();
                        var oldLineUps = await _context.LineUp.Where(t => t.Fixture.League.IsCurrent).Include(t => t.Fixture).ToListAsync();
                        var oldLineUpXI = await _context.LineUpXI.Where(t => t.Fixture.League.IsCurrent).Include(t => t.ClubPlayer).Include(t => t.Fixture).ToListAsync();
                        var oldLineUpSubs = await _context.LineUpSubstitutes.Where(t => t.Fixture.League.IsCurrent).Include(t => t.ClubPlayer).Include(t => t.Fixture).ToListAsync();
                        var oldOwnGoals = await _context.LiveOwnGoalHolders.Where(t => t.League.IsCurrent).Include(t => t.OwnGoalScoredBy).ToListAsync();

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

                        if (oldMatchFormations != null)
                        {
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
                        }

                        foreach (var s in oldLineUps)
                        {
                            var archivedLineUpArchives = new LineUps_Archive
                            {
                                FixtureId = s.FixtureId,
                                CreatedDateTime = s.CreatedDateTime,
                                ModifiedDateTime = s.CreatedDateTime,
                                LineUpSubstitutes = s.LineUpSubstitutes,
                                ClubId = s.ClubId,
                                CreatedById = s.CreatedById,
                                LineUpXI = s.LineUpXI,
                                ModifiedById = s.ModifiedById
                            };

                            _context.LineUps_Archives.Add(archivedLineUpArchives);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var s in oldLineUpXI)
                        {
                            var archivedLineUpXI = new LineUpXI_Archive
                            {
                                FixtureId = s.FixtureId,
                                CreatedDateTime = s.CreatedDateTime,
                                ModifiedDateTime = s.CreatedDateTime,
                                ClubId = s.ClubId,
                                CreatedById = s.CreatedById,
                                ModifiedById = s.ModifiedById,
                                PlayerId = s.ClubPlayer.Id
                            };

                            _context.LineUpXI_Archives.Add(archivedLineUpXI);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var s in oldLineUpSubs)
                        {
                            var archivedLineUpSubs = new LineUpSubstitutes_Archive
                            {
                                FixtureId = s.FixtureId,
                                CreatedDateTime = s.CreatedDateTime,
                                ModifiedDateTime = s.CreatedDateTime,
                                ClubId = s.ClubId,
                                CreatedById = s.CreatedById,
                                ModifiedById = s.ModifiedById,
                                PlayerId = s.ClubPlayer.Id
                            };

                            _context.LineUpSubstitutes_Archives.Add(archivedLineUpSubs);
                            await _context.SaveChangesAsync();
                        }


                        foreach (var f in oldFixtures)
                        {
                            var archivedFixture = new Fixtures_Archive
                            {
                                HomeTeamId = f.HomeTeamId,
                                AwayTeamId = f.AwayTeamId,
                                KickOffDate = f.KickOffDate,
                                KickOffTime = f.KickOffTime,
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

                        foreach (var m in oldMatchReports)
                        {
                            var archivedMatchReports = new MatchReports_Archive
                            {
                                LeagueId = m.LeagueId,
                                MatchesToBePlayedCount = m.MatchesToBePlayedCount,
                                PlayedMatchesCounts = m.PlayedMatchesCounts,
                                InterruptedMatchesCount = m.InterruptedMatchesCount,
                                PostponedMatchesCount = m.PostponedMatchesCount,
                                FixturedMatchesCount = m.FixturedMatchesCount,
                                FixturedMatchesRate = m.FixturedMatchesRate,
                                UnfixturedMatchesRate = m.UnfixturedMatchesRate,
                                InterruptedMatchesRate = m.InterruptedMatchesRate,
                                PlayedMatchesRate = m.PlayedMatchesRate,
                                PostponedMatchesRate = m.PostponedMatchesRate,
                                UnreleasedFixturesCount = m.UnreleasedFixturesCount
                            };

                            _context.MatchReportsArchive.Add(archivedMatchReports);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var r in oldMatchResultsReports)
                        {
                            var archivedMatchResultsReports = new MatchResultsReports_Archive
                            {
                                LeagueId = r.LeagueId,
                                ExpectedResultsCount = r.ExpectedResultsCount,
                                ReleasedResultsCount = r.ReleasedResultsCount,
                                WinsCount = r.WinsCount,
                                LosesCount = r.LosesCount,
                                DrawsCount = r.DrawsCount,
                                ReleasedResultsRate = r.ReleasedResultsRate,
                                UnreleasedMatchesRate = r.UnreleasedMatchesRate,
                                WinningRate = r.WinningRate,
                                LosingRate = r.LosingRate,
                                DrawingRate = r.DrawingRate,
                                UnreleasedResultsCount = r.UnreleasedResultsCount
                            };

                            _context.MatchResultsReports_Archive.Add(archivedMatchResultsReports);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var t in oldTransfersReports)
                        {
                            var archivedTransfersReports = new TransfersReports_Archive
                            {
                                LeagueId = t.LeagueId,
                                TransferPeriodId = t.TransferPeriodId,
                                TransferMarketCount = t.TransferMarketCount,
                                PurchasedPlayersCount = t.PurchasedPlayersCount,
                                DeclinedTransfersCount = t.DeclinedTransfersCount,
                                TranferAmount = t.TranferAmount,
                                ClubsCut = t.ClubsCut,
                                AssociationCut = t.AssociationCut,
                                SuccessfulTranferRate = t.SuccessfulTranferRate,
                                UnsuccessfulTranferRate = t.UnsuccessfulTranferRate,
                                NotStartedTransferRate = t.NotStartedTransferRate,
                            };

                            _context.TransfersReports_Archive.Add(archivedTransfersReports);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var c in oldClubTransfersReports)
                        {
                            var archivedClubTransfersReports = new ClubTransferReports_Archive
                            {
                                ClubId = c.ClubId,
                                LeagueId = c.LeagueId,
                                OverallTransfersCount = c.OverallTransfersCount,
                                IncomingTransfersCount = c.IncomingTransfersCount,
                                OutgoingTransfersCount = c.OutgoingTransfersCount,
                                SuccessfulIncomingTransfersCount = c.SuccessfulIncomingTransfersCount,
                                SuccessfulOutgoingTransfersCount = c.SuccessfulOutgoingTransfersCount,
                                RejectedIncomingTransfersCount = c.RejectedIncomingTransfersCount,
                                RejectedOutgoingTransfersCount = c.RejectedOutgoingTransfersCount,
                                IncomingTransferRate = c.IncomingTransferRate,
                                OutgoingTransferRate = c.OutgoingTransferRate,
                                SuccessfullIncomingTransferRate = c.SuccessfullIncomingTransferRate,
                                SuccessfullOutgoingTransferRate = c.SuccessfullOutgoingTransferRate,
                                RejectedIncomingTransferRate = c.RejectedIncomingTransferRate,
                                RejectedOutgoingTransferRate = c.RejectedOutgoingTransferRate
                            };

                            _context.ClubTransferReports_Archive.Add(archivedClubTransfersReports);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var c in oldPlayerPerformanceReports)
                        {
                            var archivedPlayerPerformanceReports = new PlayerPerformanceReports_Archive
                            {
                                PlayerId = c.PlayerId,
                                LeagueId = c.LeagueId,
                                AppearancesCount = c.AppearancesCount,
                                GoalsScoredCount = c.GoalsScoredCount,
                                AssistsCount = c.AssistsCount,
                                YellowCardCount = c.YellowCardCount,
                                RedCardCount = c.RedCardCount
                            };

                            _context.PlayerPerformanceReports_Archives.Add(archivedPlayerPerformanceReports);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var c in oldClubPerformanceReports)
                        {
                            var archivedClubPerformanceReports = new ClubPerformanceReports_Archive
                            {
                                ClubId = c.ClubId,
                                LeagueId = c.LeagueId,
                                GamesToPlayCount = c.GamesToPlayCount,
                                GamesPlayedCount = c.GamesPlayedCount,
                                GamesNotPlayedCount = c.GamesNotPlayedCount,
                                GamesWinCount = c.GamesWinCount,
                                GamesLoseCount = c.GamesLoseCount,
                                GamesDrawCount = c.GamesDrawCount,
                                GamesPlayedRate = c.GamesPlayedRate,
                                GamesNotPlayedRate = c.GamesNotPlayedRate,
                                GamesWinRate = c.GamesWinRate,
                                GamesLoseRate = c.GamesLoseRate,
                                GamesDrawRate = c.GamesDrawRate
                            };

                            _context.ClubPerformanceReports_Archive.Add(archivedClubPerformanceReports);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var l in oldLives)
                        {
                            var archivedLives = new Live_Archive
                            {
                                LeagueId = l.LeagueId,
                                FixtureId = l.FixtureId,
                                HomeTeamScore = l.HomeTeamScore,
                                AwayTeamScore = l.AwayTeamScore,
                                AddedTime = l.AddedTime,
                                ISEnded = l.ISEnded,
                                IsHalfTime = l.IsHalfTime,
                                IsLive = l.IsLive,
                                LiveTime = l.LiveTime,
                                WentToHalfTime = l.WentToHalfTime
                            };

                            _context.Live_Archives.Add(archivedLives);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var l in oldTopScores)
                        {
                            var archivedTopScores = new TopScores_Archive
                            {
                                LeagueId = l.LeagueId,
                                PlayerId = l.PlayerId,
                                NumberOfGoals = l.NumberOfGoals,
                            };

                            _context.TopScores_Archives.Add(archivedTopScores);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var l in oldTopAssists)
                        {
                            var archivedTopAssists = new TopAssists_Archive
                            {
                                LeagueId = l.LeagueId,
                                PlayerId = l.PlayerId,
                                NumberOfAssists = l.NumberOfAssists,

                            };

                            _context.TopAssists_Archives.Add(archivedTopAssists);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var g in oldLiveGoals)
                        {
                            var archivedLiveGoals = new LiveGoals_Archive
                            {
                                LeagueId = g.LeagueId,
                                ScoreById = g.ScoreById,
                                RecordedTime = g.RecordedTime,
                                LiveId = g.LiveId
                            };

                            _context.LiveGoals_Archives.Add(archivedLiveGoals);
                            await _context.SaveChangesAsync();
                        }


                        foreach (var s in oldSubstitutes)
                        {
                            var archivedSubstitutes = new Substitute_Archive
                            {
                                LiveId = s.LiveId,
                                InPlayerId = s.InPlayerId,
                                OutPlayerId = s.OutPlayerId,
                                RecordedTime = s.RecordedTime,
                                LeagueId = s.LeagueId,
                                SubTime = s.SubTime
                            };

                            _context.Substitute_Archives.Add(archivedSubstitutes);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var y in oldYellowCards)
                        {
                            var archivedYellowCards = new LiveYellowCard_Archive
                            {
                                LeagueId = y.LeagueId,
                                YellowCardTime = y.YellowCommitedById,
                                RecordedTime = y.RecordedTime,
                                LiveId = y.LiveId,
                                YellowCommitedById = y.YellowCommitedById,
                            };

                            _context.YellowCard_Archives.Add(archivedYellowCards);
                            await _context.SaveChangesAsync();
                        }

                        foreach (var r in oldRedCards)
                        {
                            var archivedRedCards = new LiveRedCard_Archive
                            {
                                LeagueId = r.LeagueId,
                                RedCardTime = r.RedCardTime,
                                RecordedTime = r.RecordedTime,
                                LiveId = r.LiveId,
                                RedCommitedById= r.RedCommitedById,
                            };

                            _context.LiveRedCard_Archives.Add(archivedRedCards);
                            await _context.SaveChangesAsync();
                        }


                        foreach (var r in oldOwnGoals)
                        {
                            var archivedOwnGoals = new OwnGoals_Archive
                            {
                                LeagueId = r.LeagueId,
                                RecordedTime = r.RecordedTime,
                                OwnGoalScoredById = r.OwnGoalScoredById,
                                OwnGoalTime = r.OwnGoalTime
                            };

                            _context.OwnGoals_Archives.Add(archivedOwnGoals);
                            await _context.SaveChangesAsync();
                        }

                        _context.LiveOwnGoalHolders.RemoveRange(oldOwnGoals);
                        _context.YellowCards.RemoveRange(oldYellowCards);
                        _context.RedCards.RemoveRange(oldRedCards);
                        _context.Substitutes.RemoveRange(oldSubstitutes);
                        _context.LiveGoals.RemoveRange(oldLiveGoals);
                        _context.LiveGoals.RemoveRange(oldLiveGoals);
                        _context.TopScores.RemoveRange(oldTopScores);
                        _context.PlayerPerformanceReports.RemoveRange(oldPlayerPerformanceReports);
                        _context.Live.RemoveRange(oldLives);
                        _context.ClubPerformanceReports.RemoveRange(oldClubPerformanceReports);
                        _context.ClubTransferReports.RemoveRange(oldClubTransfersReports);
                        _context.MatchReports.RemoveRange(oldMatchReports);
                        _context.MatchResultsReports.RemoveRange(oldMatchResultsReports);
                        _context.TransfersReports.RemoveRange(oldTransfersReports);
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


                    var matchReports = new MatchReports
                    {
                        LeagueId = newSeason.LeagueId,
                        MatchesToBePlayedCount = 0,
                        FixturedMatchesCount = 0,
                        UnreleasedFixturesCount = 0,
                        InterruptedMatchesCount = 0,
                        FixturedMatchesRate = 0,
                        UnfixturedMatchesRate = 0,
                        PlayedMatchesRate = 0,
                        InterruptedMatchesRate = 0,
                        PostponedMatchesRate = 0,
                        PlayedMatchesCounts = 0,
                        PostponedMatchesCount = 0,
                    };

                    _context.Add(matchReports);
                    await _context.SaveChangesAsync();

                    var newTransferReports = new TransfersReports
                    {
                        LeagueId = newSeason.LeagueId,
                        TransferMarketCount = 0,
                        PurchasedPlayersCount = 0,
                        DeclinedTransfersCount = 0,
                        TransferPeriodId = transferPeriod.TransferPeriodId,
                        TranferAmount = 0,
                        AssociationCut = 0,
                        ClubsCut = 0,
                        SuccessfulTranferRate = 0,
                        UnsuccessfulTranferRate = 0,
                        NotStartedTransferRate = 0
                    };

                    _context.Add(newTransferReports);
                    await _context.SaveChangesAsync();

                    var newMatchResultsReports = new MatchResultsReports
                    {
                        LeagueId = newSeason.LeagueId,
                        ExpectedResultsCount = 0,
                        ReleasedResultsCount = 0,
                        UnreleasedResultsCount = 0,
                        WinsCount = 0,
                        LosesCount = 0,
                        DrawsCount = 0,
                        ReleasedResultsRate = 0,
                        UnreleasedMatchesRate = 0,
                        WinningRate = 0,
                        LosingRate = 0,
                        DrawingRate = 0,
                    };

                    _context.Add(newMatchResultsReports);
                    await _context.SaveChangesAsync();

                    var allPlayers = await _context.Player
                        .Where(a => !a.IsDeleted)
                        .ToListAsync();

                    var newPlayerPerformanceReports = new List<PlayerPerformanceReport>();

                    var newPlayerScores = new List<TopScore>();

                    var newPlayerAssists = new List<TopAssist>();

                    foreach (var p in allPlayers)
                    {
                        var newPlayerPerformanceReport = new PlayerPerformanceReport
                        {
                            LeagueId = newSeason.LeagueId,
                            PlayerId = p.Id,
                            AppearancesCount = 0,
                            GoalsScoredCount = 0,
                            AssistsCount = 0,
                            YellowCardCount = 0,
                            RedCardCount = 0,
                            OwnGoalsScoredCount = 0,
                        };

                        newPlayerPerformanceReports.Add(newPlayerPerformanceReport);

                        var newPlayerScore = new TopScore
                        {
                            PlayerId = p.Id,
                            LeagueId = newLeague.LeagueId,
                            NumberOfGoals = 0,
                        };

                        newPlayerScores.Add(newPlayerScore);

                        var newPlayerAssist = new TopAssist
                        {
                            PlayerId = p.Id,
                            LeagueId = newLeague.LeagueId,
                            NumberOfAssists = 0,
                        };

                        newPlayerAssists.Add(newPlayerAssist);
                    }

                    

                    _context.PlayerPerformanceReports.AddRange(newPlayerPerformanceReports);
                    _context.TopScores.AddRange(newPlayerScores);
                    _context.TopAssists.AddRange(newPlayerAssists);
                    await _context.SaveChangesAsync();

                    TempData["Message"] = $"{newLeague.LeagueYears} has been started successfully and all running data will be related to it. This league will be referred to as the current league.";

                    await _activityLogger.Log($"Started season {newSeason.LeagueYears}", user.Id);
                    return RedirectToAction(nameof(Leagues));
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = "Failed to redirect to payfast: " + ex.Message,
                    errorDetails = new
                    {
                        InnerException = ex.InnerException?.Message,
                        StackTrace = ex.StackTrace
                    }
                });
            }
           
            return View(viewModel);
        }

        [Authorize(Roles = ("Sport Administrator"))]
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
