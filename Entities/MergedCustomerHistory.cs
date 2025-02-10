namespace CustomerRecognitionService.Entities
{
    public class MergedCustomerHistory
    {
        public int Id { get; set; }
        public int OriginalCustomerId { get; set; }
        public int MergedCustomerId { get; set; }
        public DateTime MergeDate { get; set; }
    }
}
