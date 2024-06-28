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

    public class MatchResultsReports: Reports
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

        public decimal AssociationCut {  get; set; }

        public decimal ClubsCut { get; set; }

        public decimal SuccessfulTranferRate { get; set; }

        public decimal UnsuccessfulTranferRate { get; set; }

        public decimal NotStartedTransferRate { get; set; }

    }
}
