using System.ComponentModel.DataAnnotations;

namespace Quiz_Management_System.Models
{
    public class UserModel
    {
        public int UserID { get; set; }

        [Required(ErrorMessage = "Enter UserName")]
        [MaxLength(100, ErrorMessage = "UserName cannot exceed 100 characters")]
        public string UserName { get; set; }

        [Required]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long")]
        [MaxLength(17, ErrorMessage = "Password cannot exceed 17 characters")]
        [RegularExpression(@"^(?=.*[a-zA-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
        ErrorMessage = "Password must contain at least one letter, one number, and one special character")]

        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Enter Email")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage ="Enter Mobile")]
        [RegularExpression(@"^\+?[0-9]{10,15}$", ErrorMessage = "Invalid mobile number format")]
        [MaxLength(15, ErrorMessage = "Mobile cannot exceed 15 characters")]
        public string Mobile { get; set; }

        //[Required]
        //public string? UserNameEmail { get; set; }

        [Required]
        public bool IsActive { get; set; }
        
        [Required]
        public bool IsAdmin { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public DateTime Modified { get; set; }
    }
}
