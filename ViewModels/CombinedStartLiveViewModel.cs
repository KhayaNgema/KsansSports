namespace MyField.ViewModels
{
    public class CombinedStartLiveViewModel
    {
        public StartLiveViewModel StartLiveViewModel { get; set; }
        public HomeGoalCombinedViewModel HomeGoalCombinedViewModel { get; set; }

        public AwayGoalCombinedViewModel AwayGoalCombinedViewModel { get; set; }

        public AwayYellowViewModel AwayYellowViewModel { get; set; }

        public HomeYellowViewModel HomeYellowViewModel { get; set; }

        public AwayRedViewModel AwayRedViewModel { get; set; }

        public HomeRedViewModel HomeRedViewModel { get; set; }

        public AwayPenaltyViewModel AwayPenaltyViewModel { get; set; }

        public HomePenaltyViewModel HomePenaltyViewModel { get; set; }
    }
}

