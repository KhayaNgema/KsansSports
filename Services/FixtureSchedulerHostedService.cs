namespace MyField.Services
{
    public class FixtureSchedulerHostedService : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private Timer _timer;

        public FixtureSchedulerHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var nextSundayAt5Pm = GetNextSundayAt5Pm();
            var timeToGo = nextSundayAt5Pm - DateTime.Now;

            _timer = new Timer(ScheduleFixtures, null, timeToGo, TimeSpan.FromDays(7));
            return Task.CompletedTask;
        }

        private async void ScheduleFixtures(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var fixtureService = scope.ServiceProvider.GetRequiredService<FixtureService>();
                await fixtureService.ScheduleFixturesAsync();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private DateTime GetNextSundayAt5Pm()
        {
            DateTime today = DateTime.Now;
            int daysUntilSunday = ((int)DayOfWeek.Sunday - (int)today.DayOfWeek + 7) % 7;
            DateTime nextSunday = today.AddDays(daysUntilSunday).Date.AddHours(17);
            return nextSunday;
        }
    }

}
