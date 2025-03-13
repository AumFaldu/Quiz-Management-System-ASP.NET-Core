using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_Management_System.Models
{
    public class QuestionLevelModel
    {
        [Key]
        public int QuestionLevelID { get; set; }
        [Required(ErrorMessage ="Enter QuestionLevel")]
        [MaxLength(100,ErrorMessage ="QuestionLevel cannot exceed 100 characters")]
        public string? QuestionLevel { get; set; }
        [HiddenInput]
        public int UserID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public class UserDropDownModel
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
        }
    }
}
