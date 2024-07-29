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

        public int LiveTime { get; set; }

        public int HomeTeamScore { get; set; }

        public int AwayTeamScore { get; set; }

        public bool IsLive { get; set; }

        public bool IsHalfTime { get; set; }

        public bool ISEnded { get; set; }

        public bool WentToHalfTime { get; set; }
    }

    public class LiveGoal
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalsId { get; set; }
        public int LiveId { get; set; }
        public virtual Live Live { get; set; }
        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }
    }

    public class LiveAssist
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssistId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }
    }

    public class YellowCard
    {
        public int YellowCardId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public string CardTime { get; set; }
    }

    public class RedCard
    {
        public int RedCardId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }
        public string CardTime { get; set; }

    }

    public class Penalty
    {
        public int PenaltyId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

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


    public class Substitute
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubstituteId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public string OutPlayerId { get; set; }

        public virtual Player OutPlayer { get; set; }

        public  string InPlayerId { get; set; }

        public virtual Player InPlayer { get; set;}

        public string SubTime { get; set; }
    }


    public class LiveGoalHolder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalsId { get; set; }
        public int LiveId { get; set; }
        public virtual Live Live { get; set; }
        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public string ScoredTime { get; set; }
    }

    public class LiveAssistHolder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AssistId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }
    }


    public class LiveYellowCardHolder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int YellowCardId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public string CardTime { get; set; }
    }

    public class LiveRedCardHolder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RedCardId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }

        public string CardTime { get; set; }
    }
}
