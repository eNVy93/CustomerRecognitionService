namespace CustomerRecognitionService.Entities
{
    public class PendingMerge
    {
        public int Id { get; set; }
        public int Customer1Id { get; set; }
        public int Customer2Id { get; set; }
        public DateTime DetectedAt { get; set; }
        public bool IsProcessed { get; set; }
    }
}
