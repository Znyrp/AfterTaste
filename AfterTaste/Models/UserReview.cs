using AfterTaste.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterTaste.Models
{
    public class UserReview
    {
        [Key]
        public int reviewId { get; set; }

        [ForeignKey("userId")]
        public string? userId { get; set; }
        public User? User { get; set; } // This property references the User model
        [Required(ErrorMessage = "A Rating is Required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1-5 only")]
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public string? comment { get; set; }
        public int RecipeId { get; set; }
        public Recipe? Recipe { get; set; }
    }
}