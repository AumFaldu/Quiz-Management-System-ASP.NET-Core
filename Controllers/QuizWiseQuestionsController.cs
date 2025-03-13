using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Quiz_Management_System.Models;
using static Quiz_Management_System.Models.QuizWiseQuestionsModel;
using static Quiz_Management_System.Models.QuestionModel;
using OfficeOpenXml;
namespace Quiz_Management_System.Controllers
{
    [CheckAccess]
    public class QuizWiseQuestionsController : Controller
    {
        private IConfiguration configuration;

        public QuizWiseQuestionsController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult QuizWiseQuestions_Add_Edit(int QuizWiseQuestionsID)
        {
            if (QuizWiseQuestionsID==0)
            {
                TempData["PageTitle"] = "QuizWiseQuestions Add";
            }
            else
            {
                TempData["PageTitle"] = "QuizWiseQuestions Edit";
            }
            ViewBag.QuizWiseQuestionsID = QuizWiseQuestionsID;

            // Dropdown Quiz List
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command2 = connection.CreateCommand();

                command2.CommandType = CommandType.StoredProcedure;
                command2.CommandText = "PR_MST_Quiz_SelectAll";
                
                SqlDataReader reader2 = command2.ExecuteReader();
                DataTable dataTable2 = new DataTable();
                
                dataTable2.Load(reader2);
                
                List<QuizWiseDropDownModel> quizList = new List<QuizWiseDropDownModel>();
                foreach (DataRow data in dataTable2.Rows)
                {
                    QuizWiseDropDownModel quizWiseDropDownModel = new QuizWiseDropDownModel();
                    quizWiseDropDownModel.QuizID = Convert.ToInt32(data["QuizID"]);
                    quizWiseDropDownModel.QuizName = data["QuizName"].ToString();
                    quizList.Add(quizWiseDropDownModel);
                }
                ViewBag.QuizList = quizList;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command3 = connection.CreateCommand();

                command3.CommandType = CommandType.StoredProcedure;
                command3.CommandText = "PR_MST_Question_SelectAll";

                SqlDataReader reader3 = command3.ExecuteReader();
                DataTable dataTable3 = new DataTable();

                dataTable3.Load(reader3);

                List<QuestionDropDownModel> questionList = new List<QuestionDropDownModel>();
                foreach (DataRow data in dataTable3.Rows)
                {
                    QuestionDropDownModel questionDropDownModel = new QuestionDropDownModel();
                    questionDropDownModel.QuestionID = Convert.ToInt32(data["QuestionID"]);
                    questionDropDownModel.QuestionText = data["QuestionText"].ToString();
                    questionList.Add(questionDropDownModel);
                }
                ViewBag.QuestionList = questionList;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command5 = connection.CreateCommand();

                command5.CommandType = CommandType.StoredProcedure;
                command5.CommandText = "PR_MST_User_SelectAll";

                SqlDataReader reader5 = command5.ExecuteReader();
                DataTable dataTable5 = new DataTable();

                dataTable5.Load(reader5);

                List<UserDropDownModel> userList = new List<UserDropDownModel>();
                foreach (DataRow data in dataTable5.Rows)
                {
                    UserDropDownModel userDropDownModel = new UserDropDownModel();
                    userDropDownModel.UserID = Convert.ToInt32(data["UserID"]);
                    userDropDownModel.UserName = data["UserName"].ToString();
                    userList.Add(userDropDownModel);
                }
                ViewBag.UserList = userList;
            }
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command4 = connection.CreateCommand();

                command4.CommandType = CommandType.StoredProcedure;

                command4.CommandText = "PR_MST_QuizWiseQuestions_SelectByPK";
                command4.Parameters.AddWithValue("@QuizWiseQuestionsID", QuizWiseQuestionsID);
              
                SqlDataReader reader4 = command4.ExecuteReader();
                DataTable dataTable4 = new DataTable();

                dataTable4.Load(reader4);
                QuizWiseQuestionsModel quizWiseQuestionsModel = new QuizWiseQuestionsModel();
                foreach (DataRow data in dataTable4.Rows)
                {
                    quizWiseQuestionsModel.QuizWiseQuestionsID = Convert.ToInt32(data["QuizWiseQuestionsID"]);
                    quizWiseQuestionsModel.QuizID = Convert.ToInt32(data["QuizID"]);
                    quizWiseQuestionsModel.QuestionID = Convert.ToInt32(data["QuestionID"]);
                    quizWiseQuestionsModel.UserID = Convert.ToInt32(data["UserID"]);
                }
                return View(quizWiseQuestionsModel);
            }
        }
        public IActionResult QuizWiseQuestionsSave(QuizWiseQuestionsModel model)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    if (model.QuizWiseQuestionsID <= 0)
                    {
                        command.CommandText = "PR_MST_QuizWiseQuestions_Insert";
                    }
                    else
                    {
                        command.CommandText = "PR_MST_QuizWiseQuestions_UpdateByPK";
                        command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = model.QuizWiseQuestionsID;
                    }
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = model.QuizID;
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = model.QuestionID;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = model.UserID;
                    command.ExecuteNonQuery();
                }
                return RedirectToAction("QuizWiseQuestions_List");
            }
            return View(model);
        }
        public IActionResult QuizWiseQuestionsExportToExcel()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_QuizWiseQuestions_SelectAll";
            //sqlCommand.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cells[1, 1].Value = "QuizWiseQuestionsID";
                worksheet.Cells[1, 2].Value = "QuizID";
                worksheet.Cells[1, 3].Value = "QuestionID";
                worksheet.Cells[1, 4].Value = "UserID";
                worksheet.Cells[1, 5].Value = "Created";
                worksheet.Cells[1, 6].Value = "Modified";
                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuizWiseQuestionsID"];
                    worksheet.Cells[row, 2].Value = item["QuizID"];
                    worksheet.Cells[row, 3].Value = item["QuestionID"];
                    worksheet.Cells[row, 4].Value = item["UserID"];
                    worksheet.Cells[row, 5].Value = item["Created"];
                    worksheet.Cells[row, 6].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Data-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
        public IActionResult QuizWiseQuestions_List()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_QuizWiseQuestions_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        public IActionResult QuizWiseQuestionsDelete(int QuizWiseQuestionsID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_QuizWiseQuestions_DeleteByPK";
                    command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = QuizWiseQuestionsID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "QuizWiseQuestions deleted successfully.";
                return RedirectToAction("QuizWiseQuestions_List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the QuizWiseQuestions: " + ex.Message;
                return RedirectToAction("QuizWiseQuestions_List");
            }
        }
        public IActionResult QuizWiseQuestions_Details(int QuizWiseQuestionsID,string QuizName,int TotalQuestions)
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            ViewBag.QuizName = QuizName;
            ViewBag.TotalQuestions = TotalQuestions;
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_QuizWiseQuestions_SelectByPK";
            command.Parameters.Add("@QuizWiseQuestionsID", SqlDbType.Int).Value = QuizWiseQuestionsID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
    }
}
