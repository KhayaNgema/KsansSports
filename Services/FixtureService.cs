using Microsoft.EntityFrameworkCore;
using MyField.Data;
using MyField.Models;

namespace MyField.Services
{
    public class FixtureService
    {
        private readonly Ksans_SportsDbContext _context;

        public FixtureService(Ksans_SportsDbContext context)
        {
            _context = context;
        }

        public async Task ScheduleFixturesAsync()
        {
            var clubs = await _context.Club.ToListAsync();

            DateTime today = DateTime.Today;
            int daysUntilSaturday = ((int)DayOfWeek.Saturday - (int)today.DayOfWeek + 7) % 7;
            DateTime nextSaturday = today.AddDays(daysUntilSaturday);

            nextSaturday = nextSaturday.Date;

            var weekendFixtures = await _context.Fixture
                .Where(f => f.KickOffDate >= nextSaturday && f.KickOffDate < nextSaturday.AddDays(2))
                .ToListAsync();

            _context.Fixture.RemoveRange(weekendFixtures);
            await _context.SaveChangesAsync();

            var random = new Random();
            clubs = clubs.OrderBy(x => random.Next()).ToList();


            await ScheduleFixturesForDay(nextSaturday, clubs);
            await ScheduleFixturesForDay(nextSaturday.AddDays(1), clubs);
        }

        private async Task ScheduleFixturesForDay(DateTime day, List<Club> clubs)
        {
            int fixturesPerDay = 4;
            TimeSpan startTime = new TimeSpan(9, 0, 0); 
            TimeSpan interval = new TimeSpan(2, 0, 0); 

            for (int i = 0; i < fixturesPerDay && i < clubs.Count / 2; i++)
            {
                var currentSeason = await _context.League
                    .Where(c => c.IsCurrent)
                    .FirstOrDefaultAsync();

                var homeTeam = clubs[i * 2];
                var awayTeam = clubs[i * 2 + 1];

                var kickOffTime = day + startTime.Add(interval * i);

                var fixture = new Fixture
                {
                    HomeTeamId = homeTeam.ClubId,
                    AwayTeamId = awayTeam.ClubId,
                    KickOffDate = kickOffTime.Date,
                    KickOffTime = kickOffTime, 
                    Location = "Nkungwini Sports Ground", 
                    CreatedDateTime = DateTime.Now,
                    ModifiedDateTime = DateTime.Now,
                    CreatedById = "system", 
                    ModifiedById = "system",
                    FixtureStatus = FixtureStatus.Upcoming,
                    LeagueId = currentSeason.LeagueId
                };

                _context.Fixture.Add(fixture);
            }

            await _context.SaveChangesAsync();
        }
    }


}
