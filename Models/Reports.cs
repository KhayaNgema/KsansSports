using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyField.Models
{
    public class Reports
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReportId { get; set; }
    }


    public class MatchReports : Reports
    {
        public int LeagueId { get; set; }

        public virtual League Season { get; set; }
        public int MatchesToBePlayedCount { get; set; }
        public int FixturedMatchesCount { get; set; }

        public int UnreleasedFixturesCount { get; set; }

        public int PlayedMatchesCounts { get; set; }

        public int PostponedMatchesCount { get; set; }

        public int InterruptedMatchesCount { get; set; }

        public decimal FixturedMatchesRate { get; set; }

        public decimal UnfixturedMatchesRate { get; set; }

        public decimal PlayedMatchesRate { get; set; }

        public decimal PostponedMatchesRate { get; set; }

        public decimal InterruptedMatchesRate { get; set; }
    }

    public class MatchResultsReports : Reports
    {
        public int LeagueId { get; set; }

        public virtual League Season { get; set; }

        public int ExpectedResultsCount { get; set; }

        public int ReleasedResultsCount { get; set; }

        public int UnreleasedResultsCount { get; set; }

        public int WinsCount { get; set; }

        public int LosesCount { get; set; }

        public int DrawsCount { get; set; }

        public decimal ReleasedResultsRate { get; set; }

        public decimal UnreleasedMatchesRate { get; set; }

        public decimal WinningRate { get; set; }


        public decimal LosingRate { get; set; }

        public decimal DrawingRate { get; set; }
    }

    public class TransfersReports : Reports
    {
        public int LeagueId { get; set; }

        public virtual League Season { get; set; }

        public int TransferPeriodId { get; set; }

        public virtual TransferPeriod TransferPeriod { get; set; }

        public int TransferMarketCount { get; set; }

        public int PurchasedPlayersCount { get; set; }

        public int DeclinedTransfersCount { get; set; }

        public decimal TranferAmount { get; set; }

        public decimal AssociationCut { get; set; }

        public decimal ClubsCut { get; set; }

        public decimal SuccessfulTranferRate { get; set; }

        public decimal UnsuccessfulTranferRate { get; set; }

        public decimal NotStartedTransferRate { get; set; }

    }

    public class ClubPerformanceReport : Reports
    {
        public int LeagueId { get; set; }

        public virtual League League { get; set; }
        public int ClubId { get; set; }

        public virtual Club Club { get; set; }

        public int GamesToPlayCount { get; set; }

        public int GamesPlayedCount { get; set; }

        public int GamesNotPlayedCount { get; set; }

        public int GamesWinCount { get; set; }

        public int GamesLoseCount { get; set; }

        public int GamesDrawCount { get; set; }

        public decimal GamesPlayedRate { get; set; }

        public decimal GamesNotPlayedRate { get; set; }

        public decimal GamesWinRate { get; set; }

        public decimal GamesLoseRate { get; set; }

        public decimal GamesDrawRate { get; set; }

        public int StandingId { get; set; }

        public virtual Standing ClubStanding { get; set; }
    }

    public class ClubTransferReport : Reports
    {
        public int LeagueId { get; set; }

        public virtual League League { get; set; }
        public int ClubId { get; set; }

        public virtual Club Club { get; set; }
        public int OverallTransfersCount { get; set; }

        public int OutgoingTransfersCount { get; set; }

        public int IncomingTransfersCount { get; set; }

        public decimal OutgoingTransferRate { get; set; }

        public decimal IncomingTransferRate { get; set; }

        public int SuccessfulOutgoingTransfersCount { get; set; }

        public int SuccessfulIncomingTransfersCount { get; set; }

        public int RejectedOutgoingTransfersCount {  get; set; }

        public int RejectedIncomingTransfersCount { get; set; }

        public int NotActionedIncomingTransferCount { get; set; }

        public int NotActionedOutgoigTransferCount { get; set; }

        public decimal SuccessfullIncomingTransferRate { get; set; }

        public decimal RejectedOutgoingTransferRate { get; set; }

        public decimal SuccessfullOutgoingTransferRate { get; set; }

        public decimal RejectedIncomingTransferRate { get; set; }

        public decimal NotActionedIncomingTransferRate { get; set; }

        public decimal NotActionedOutgoingTransferRate { get; set; }
    }
}
