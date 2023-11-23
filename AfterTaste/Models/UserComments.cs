using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterTaste.Models
{
    public class UserComments
    {
        [Required]
        public int commentId { get; set; }
        [ForeignKey("userId")]
        public int userId { get; set; }
        [Required]
        public string commentText { get; set; }
        public DateTime commentDate { get; set; }
    }
}