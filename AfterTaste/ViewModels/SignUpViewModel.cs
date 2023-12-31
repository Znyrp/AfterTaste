﻿using System.ComponentModel.DataAnnotations;

namespace AfterTaste.ViewModels
{
    public class SignUpViewModel
    {
        public int userId { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string? username { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        public string? email { get; set; }

        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string? userPassword { get; set; }

        [Display(Name = "Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(userPassword), ErrorMessage = "Password do not match")]
        [Required(ErrorMessage = "You must confirm your password")]
        public string? ConfirmPassword { get; set; }

        [Display(Name = "Firstname")]
        [Required(ErrorMessage = "Firstname is required")]
        public string? firstName { get; set; }

        [Display(Name = "Lastname")]
        [Required(ErrorMessage = "Lastname is required")]
        public string? lastName { get; set; }

        [Display(Name = "Profile Picture")]
      
        [DataType(DataType.Upload)]
        [RegularExpression(@"^.*\.(jpg|jpeg|png)$", ErrorMessage = "Only JPG, JPEG, and PNG files are allowed.")]
        public byte[]? profilePicture { get; set; }

        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Contact Number is required")]
        [RegularExpression("[0-9]{4}-[0-9]{3}-[0-9]{4}", ErrorMessage = "Please follow Phone Number format 09**-***-****!")]
        public string? contactNumber { get; set; }
        public DateTime? birthdate { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is required")]
        public string? address { get; set; }
    }
}
