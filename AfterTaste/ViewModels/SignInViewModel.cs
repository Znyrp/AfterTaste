using System.ComponentModel.DataAnnotations;

namespace AfterTaste.ViewModels
{
    public class SignInViewModel
    {
        
        [Display(Name = "Username")]
        [Required(ErrorMessage = " a username is required")]
        public string? UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = " a password is required")]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

    }
}
