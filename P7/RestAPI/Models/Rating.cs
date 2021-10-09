namespace RestAPI.Models
{
    public class Rating
    {
        public int Id { get; set; }
        public string MoodyRating { get; set; }
        public string SandRating { get; set; }
        public string FitchRating { get; set; }
        public int OrderNumber { get; set; }
    }
}