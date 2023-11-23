using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AfterTaste.Models
{
    public class TopRecipe
    {
        [Required]
        public int topRecipeId { get; set; }
        [ForeignKey("recipeId")]
        public int recipeRatings { get; set; }

    }
}