using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.AspNetCore.Mvc;

namespace Quiz_Management_System.Models
{
    public class QuizWiseQuestionsModel
    {
        [Key]
        public int QuizWiseQuestionsID { get; set; }
        [Required(ErrorMessage = "Enter QuizID")]
        public int QuizID { get; set; }
        [Required(ErrorMessage = "Enter QuestionID")]
        public int QuestionID { get; set; }
        [HiddenInput]
        public int UserID { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public class QuizWiseDropDownModel
        {
            public int QuizID { get; set; }
            public string QuizName { get; set; }
        }
        //public class UserDropDownModel
        //{
        //    public int UserID { get; set; }
        //    public string UserName { get; set; }
        //}
    }
}
