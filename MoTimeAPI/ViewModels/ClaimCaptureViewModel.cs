namespace MoTimeAPI.ViewModels
{
    public class ClaimCaptureViewModel
    {
        public int? ClaimItemId { get; set; }
        public int? TimecardId { get; set; }

        public decimal? Amount { get; set; }

        public string? UploadProof { get; set; }

    }
}
