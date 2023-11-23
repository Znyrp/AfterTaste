using System.ComponentModel.DataAnnotations.Schema;

namespace AfterTaste.Models
{
    public class UserReview
    {

        public int reviewId { get; set; }
        [ForeignKey("Username")]
        public string username { get; set; }
        public int rating { get; set; }
        public DateTime review_Date { get; set; }

    }
}