#pragma warning disable CS8618
using ProjectRecycle.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjectRecycle.Models
{
    public class AppUser
    {
        [Key]
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        // Email
        [Required(ErrorMessage = "Please enter your email.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email.")]
        public string Email { get; set; }

        // Password
        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)] // useful
        [MinLength(6, ErrorMessage = "Please enter a valid Password .")]
        public string Password { get; set; }

        // Confirm Password
        [NotMapped]
        [Required(ErrorMessage = "Please enter your password.")]
        [Compare("Password", ErrorMessage = "Passwords must match.")]
        [DataType(DataType.Password)] // useful
        [Display(Name = "Confirm Password ")]
        public string ConfirmPassword { get; set; }



        public StaticData.UserRole Role { get; set; }
        public string Diploma { get; set; }
        public string Expertise { get; set; }
        public string Description { get; set; }
        // Created At
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Updated At
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public List<Mission> Missions { get; set; } = new();
    }
}
