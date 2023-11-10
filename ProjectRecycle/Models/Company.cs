#pragma warning disable CS8618
using ProjectRecycle.Utility;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ProjectRecycle.Models
{
    public class Company
    {
        [Key]
        public int CompanyId { get; set; }
        public string Name { get; set; }
        public string WebUrl { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        public int PhoneNumber { get; set; }
        /// Email
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
        public string Description { get; set; }

        public StaticData.CompanyActivity ActivityType { get; set; }

        // Created At
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Updated At
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        //Navigation Properties
        public List<Waste> CompanyWastes { get; set; } = new();
        public List<Bid> Bids { get; set; } = new();


    }

}
