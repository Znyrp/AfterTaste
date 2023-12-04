using AfterTaste.Models;

namespace AfterTaste.ViewModels
{
    public class ProfileViewModel
    {
        public List<Recipe> CreatedRecipes { get; set; }
        public List<Recipe> FavoriteRecipes { get; set; }
    }
}