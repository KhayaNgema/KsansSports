using MyField.Models;

namespace MyField.ViewModels
{
    public class ModifyFixtureViewModel
    {
        public int FixtureId { get; set; }
        public int HomeTeamId { get; set; }

        public int AwayTeamId { get; set; }

        public DateTime KickOff { get; set; }

        public string RefereeId { get; set; }

        public string AssistantOneId { get; set; }

        public string AssistantTwoId { get; set; }

        public string Stadium { get; set; }

        public FixtureStatus FixtureStatus { get; set; }
    }
}
