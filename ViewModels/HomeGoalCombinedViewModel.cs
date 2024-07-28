using MyField.Models;

namespace MyField.ViewModels
{
    public class HomeGoalCombinedViewModel
    {
        public HomeGoalViewModel HomeGoalViewModel { get; set; }
        public IEnumerable<dynamic> Players { get; set; }

        public int FixtureId { get; set; }
    }

}
