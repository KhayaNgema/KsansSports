namespace MyField.Models
{
    public class EventDto
    {
        public int EventId { get; set; }
        public string EventType { get; set; }
        public string ScoreBy { get; set; }
        public string ScoredTime { get; set; }
        public string AssistedBy { get; set; }
        public string YellowCardTime { get; set; }
        public string YellowCommitedBy { get; set; }
        public string RedCardTime { get; set; }
        public string RedCommitedBy { get; set; }
        public string PenaltyTime { get; set; }
        public string Player { get; set; }
        public string OutPlayer { get; set; }
        public string InPlayer { get; set; }
        public string SubTime { get; set; }
    }
}
