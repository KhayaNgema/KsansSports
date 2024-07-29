namespace MyField.ViewModels
{
    public class HomeRedViewModel
    {
        public int FixtureId { get; set; }

        public IEnumerable<dynamic> Players { get; set; }

        public string HomeTeam { get; set; }
    }
}
