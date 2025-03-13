using System.ComponentModel.DataAnnotations;

namespace Quiz_Management_System.Models
{
    public class UserLoginModel
    {
        [Required(ErrorMessage ="UserName or Email is required.")]
        public string UserNameEmail { get; set; }
        [Required(ErrorMessage ="Password is required.")]
        public string Password { get; set; }
    }
}
