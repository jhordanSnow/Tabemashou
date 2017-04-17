using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Tabemashou_User.Models
{
    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }
    }

    public class RegisterViewModel
    {

        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        public decimal IdCard { get; set; }


        [Required]
        public string Gender { get; set; }

        [Required]
        public System.DateTime BirthDate { get; set; }

        [Required]
        public int Nationality { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string SecondLastName { get; set; }

        [Required]
        public decimal AccountNumber { get; set; }
    }

    public class ProfileEditViewModel
    {
        public ProfileViewModel profileData { get; set; }
        public ChangePasswordViewModel changePass { get; set; }
        public List<Review> Timeline { get; set; }
        public string Activity { get; set; }
        public string Settings { get; set; }
        public string Change { get; set; }

    }

    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }


    }


    public class ProfileViewModel
    {

        [Required]
        public string Username { get; set; }

        [Required]
        public decimal IdCard { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public System.DateTime BirthDate { get; set; }

        [Required]
        public int Nationality { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public string SecondLastName { get; set; }

        [Required]
        public decimal AccountNumber { get; set; }

        public decimal Followers { get; set; }
        public decimal Following { get; set; }
        public decimal Reviews { get; set; }
    }
}
