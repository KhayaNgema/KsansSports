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
    }

    public class RedCard
    {
        public int RedCardId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }

        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }
    }

    public class Penalty
    {
        public int PenaltyId { get; set; }

        public int LiveId { get; set; }

        public virtual Live Live { get; set; }
    }


    public class LiveGoalHolder
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GoalsId { get; set; }
        public int LiveId { get; set; }
        public virtual Live Live { get; set; }
        public string PlayerId { get; set; }

        public virtual Player Player { get; set; }
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
}
