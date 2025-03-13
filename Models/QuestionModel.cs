using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_Management_System.Models
{
    public class QuestionModel
    {
        [Key]
        public int QuestionID { get; set; }
        [Required(ErrorMessage ="Enter QuestionText")]
        [MaxLength(100,ErrorMessage ="QuestionText should not exceed 100 characters")]
        public string? QuestionText { get; set; }
        [Required(ErrorMessage ="Enter Question Level")]
        public int? QuestionLevelID { get; set; }
        [Required(ErrorMessage ="Enter OptionA")]
        [MaxLength(100,ErrorMessage ="OptionA cannot should not 100 characters")]
        public string? OptionA { get; set; }
        [Required(ErrorMessage = "Enter OptionB")]
        [MaxLength(100, ErrorMessage = "OptionB cannot should not 100 characters")]
        public string? OptionB { get; set; }
        [Required(ErrorMessage = "Enter OptionC")]
        [MaxLength(100, ErrorMessage = "OptionC cannot should not 100 characters")]
        public string? OptionC { get; set; }
        [Required(ErrorMessage = "Enter OptionD")]
        [MaxLength(100, ErrorMessage = "OptionD cannot should not 100 characters")]
        public string? OptionD { get; set; }
        [Required(ErrorMessage="Enter CorrectOption")]
        public string? CorrectOption { get; set; }
        [Required(ErrorMessage = "Enter Question Marks")]
        public int QuestionMarks { get; set; }=0;
        public bool IsActive { get; set; } = true;
        [HiddenInput]
        public int UserID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public class QuestionLevelDropDownModel
        {
            public int QuestionLevelID { get; set; }
            public string QuestionLevel { get; set; }
        }
        public class QuestionDropDownModel
        {
            public int QuestionID { get; set; }
            public string QuestionText { get; set; }

        }
        public class UserDropDownModel
        {
            public int UserID { get; set; }
            public string UserName { get; set; }
        }
    }
}
