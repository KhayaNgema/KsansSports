namespace MyField.ViewModels
{
    public class AwayYellowViewModel
    {
        public int FixtureId { get; set; }

        public IEnumerable<dynamic> Players { get; set; }

        public string AwayTeam { get; set; }
    }
}
