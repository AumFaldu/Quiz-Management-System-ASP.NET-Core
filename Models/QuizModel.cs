using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_Management_System.Models
{
    public class QuizModel
    {
        [Key]
        public int QuizID { get; set; }
        [Required(ErrorMessage ="Enter QuizName")]
        [MaxLength(100, ErrorMessage = "QuizName cannot exceed 100 characters")]
        public string? QuizName { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Total Questions must be greater than 0.")]
        [Required(ErrorMessage = "Enter Total Questions")]
        public int TotalQuestions { get; set; }=0;
        public Boolean IsActive { get; set; } = true;
        [Required(ErrorMessage ="Enter Quiz Date")]
        public DateTime QuizDate { get; set; }
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
