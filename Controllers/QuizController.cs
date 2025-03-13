using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Quiz_Management_System.Models;
using System.Reflection;
using OfficeOpenXml;
using static Quiz_Management_System.Models.QuizModel;

namespace Quiz_Management_System.Controllers
{
    [CheckAccess]
    public class QuizController : Controller
    {
        private IConfiguration configuration;

        public QuizController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Quiz_Add_Edit(int quizID)
        {
            if (quizID<=0)
            {
                TempData["PageTitle"] = "Quiz Add";
            }
            else
            {
                TempData["PageTitle"] = "Quiz Edit";
            }
            ViewBag.QuizID = quizID;
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_Quiz_SelectByPK";
            command.Parameters.AddWithValue("@QuizID", SqlDbType.Int).Value = quizID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            QuizModel quizmodel = new QuizModel();
            foreach (DataRow row in table.Rows)
            {
                quizmodel.QuizID = Convert.ToInt32(row["QuizID"]);
                quizmodel.QuizName = row["QuizName"].ToString();
                quizmodel.TotalQuestions= Convert.ToInt32(row["TotalQuestions"]);
                quizmodel.QuizDate = Convert.ToDateTime(row["QuizDate"]);
                quizmodel.UserID = Convert.ToInt32(row["UserID"]);
            }
            List<UserDropDownModel> userList = new List<UserDropDownModel>();
            SqlCommand command1 = connection.CreateCommand();
            command1.CommandType = CommandType.StoredProcedure;
            command1.CommandText = "PR_MST_User_SelectAll";
            SqlDataReader reader1 = command1.ExecuteReader();
            DataTable table1 = new DataTable();
            table1.Load(reader1);
            foreach (DataRow row in table1.Rows)
            {
                UserDropDownModel user = new UserDropDownModel();
                user.UserID = Convert.ToInt32(row["UserID"]);
                user.UserName = row["UserName"].ToString();
                userList.Add(user);
            }
            ViewBag.UserList = userList;
            return View(quizmodel);
        }
        public IActionResult QuizSave(QuizModel quizmodel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (quizmodel.QuizID <= 0)
                {
                    command.CommandText = "PR_MST_Quiz_Insert";
                }
                else
                {
                    command.CommandText = "PR_MST_Quiz_UpdateByPK";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = quizmodel.QuizID;
                }
                    command.Parameters.Add("@QuizName", SqlDbType.VarChar).Value = quizmodel.QuizName;
                    command.Parameters.Add("@TotalQuestions", SqlDbType.Int).Value = quizmodel.TotalQuestions;
                    command.Parameters.Add("@QuizDate", SqlDbType.DateTime).Value = quizmodel.QuizDate;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = quizmodel.UserID;
                    command.ExecuteNonQuery();
                return RedirectToAction("Quiz_List");
            }
            return View("Quiz_Add_Edit", quizmodel);
        }
        public IActionResult QuizExportToExcel()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_Quiz_SelectAll";
            //sqlCommand.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cells[1, 1].Value = "QuizID";
                worksheet.Cells[1, 2].Value = "QuizName";
                worksheet.Cells[1, 3].Value = "TotalQuestions";
                worksheet.Cells[1, 4].Value = "QuizDate";
                worksheet.Cells[1, 5].Value = "UserID";
                worksheet.Cells[1, 6].Value = "Created";
                worksheet.Cells[1, 7].Value = "Modified";
                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuizID"];
                    worksheet.Cells[row, 2].Value = item["QuizName"];
                    worksheet.Cells[row, 3].Value = item["TotalQuestions"];
                    worksheet.Cells[row, 4].Value = item["QuizDate"];
                    worksheet.Cells[row, 5].Value = item["UserID"];
                    worksheet.Cells[row, 6].Value = item["Created"];
                    worksheet.Cells[row, 7].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Data-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
        public IActionResult Quiz_List()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_Quiz_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        public IActionResult QuizDelete(int QuizID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_Quiz_DeleteByPK";
                    command.Parameters.Add("@QuizID", SqlDbType.Int).Value = QuizID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Quiz deleted successfully.";
                return RedirectToAction("Quiz_List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the quiz: " + ex.Message;
                return RedirectToAction("Quiz_List");
            }
        }
    }
}
