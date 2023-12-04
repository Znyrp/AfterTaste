using AfterTaste.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterTaste.Models
{
    public class FavoriteRecipe
    {
        [Key]
        public int FavoriteId { get; set; }
        [ForeignKey("Id")]
        public string userId { get; set; }
        public User User { get; set; }
        [ForeignKey("recipeId")]
        public int recipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
