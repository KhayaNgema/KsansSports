using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyField.Models
{



    public class Live
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int LiveId { get; set; }

        public int FixtureId { get; set; }

        public virtual Fixture Fixture { get; set; }

        public int LeagueId { get; set; }

        public virtual League League { get; set; }

        public int LiveTime { get; set; }

        public int HomeTeamScore { get; set; }

        public int AwayTeamScore { get; set; }

        public bool IsLive { get; set; }

        public bool IsHalfTime { get; set; }

        public bool ISEnded { get; set; }

        public bool WentToHalfTime { get; set; }

        public int? AddedTime { get; set; }

        public string? HalfTimeScore { get; set; }
    }

    public class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EventId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public int LeagueId { get; set; }

        public virtual League League { get; set; }

        public DateTime RecordedTime { get; set; }
    }


    public class LiveGoal : Event
    {
        public string ScoreById { get; set; }

        public virtual Player ScoreBy { get; set; }

        public string ScoredTime { get; set; }
    }

    public class LiveAssist : Event
    {
        public string AssistedById { get; set; }

        public virtual Player AssistedBy { get; set; }
    }

    public class YellowCard : Event
    {
        public string YellowCommitedById { get; set; }

        public virtual Player YellowCommitedBy{ get; set; }

        public string YellowCardTime { get; set; }
    }

    public class RedCard  : Event
    {
        public string RedCommitedById { get; set; }

        public virtual Player RedCommitedBy { get; set; }
        public string RedCardTime { get; set; }

    }

    public class Penalty : Event
    {
        public string PenaltyTime { get; set; }

        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public PenaltyType Type { get; set; }
    }

    public enum PenaltyType
    {
        [Display(Name ="Dangerous play")]
        Dangerous_Play,

        [Display(Name = "Disrespect")]
        Disrespect,

        [Display(Name = "Impending progress")]
        Impending_Progress,

        [Display(Name = "Handball")]
        Touching_The_Ball,

        [Display(Name = "Technical penalty")]
        Technical_Penalty
    }


    public class Substitute : Event
    {
        public string OutPlayerId { get; set; }

        public virtual Player OutPlayer { get; set; }

        public  string InPlayerId { get; set; }

        public virtual Player InPlayer { get; set;}

        public string SubTime { get; set; }
    }

    public class LiveGoalHolder : Event
    {
        public string ScoredById { get; set; }

        public virtual Player ScoredBy { get; set; }

        public string ScoredTime { get; set; }
    }

    public class LiveAssistHolder : Event
    {

        public string AssistedById { get; set; }

        public virtual Player AssistedBy { get; set; }
    }


    public class LiveYellowCardHolder : Event
    {
        public string YellowCommitedById { get; set; }

        public virtual Player YellowCommitedBy { get; set; }

        public string YellowCardTime { get; set; }
    }

    public class LiveRedCardHolder : Event  
    {
        public string RedCommitedById { get; set; }

        public virtual Player RedCommitedBy { get; set; }

        public string RedCardTime { get; set; }
    }
}
