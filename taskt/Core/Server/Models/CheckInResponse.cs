namespace taskt.Core.Server.Models
{
    public class CheckInResponse
    {
        public Task ScheduledTask { get; set; }
        public PublishedScript PublishedScript { get; set; }
        public Worker Worker { get; set; }
    }
}
