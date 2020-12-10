using System.ComponentModel.DataAnnotations;

namespace UnikBolig.Web.Models
{
    public class RegisterModel
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Efternavn")]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "Telefonnummer")]
        public string Phone { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Adgangskoden skal mindst være {2} tegn", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Adgangskode")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Gentag adgangskoden")]
        [Compare("Password", ErrorMessage = "Begge adgangskoder skal være ens")]
        public string ConfirmPassword { get; set; }
    }
}