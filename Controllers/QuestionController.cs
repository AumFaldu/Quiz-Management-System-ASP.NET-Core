using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Quiz_Management_System.Models;
using static Quiz_Management_System.Models.QuestionModel;
using NuGet.Protocol.Plugins;
using System.Reflection;
using OfficeOpenXml;

namespace Quiz_Management_System.Controllers
{
    [CheckAccess]
    public class QuestionController : Controller
    {
        private IConfiguration configuration;

        public QuestionController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Question_Add_Edit(int QuestionID)
        {
            if (QuestionID==0)
            {
                TempData["PageTitle"] = "Question Add";
            }
            else
            {
                TempData["PageTitle"] = "Question Edit";
            }
            ViewBag.QuestionID = QuestionID;
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();

            SqlCommand command2 = connection.CreateCommand();
            command2.CommandType = System.Data.CommandType.StoredProcedure;
            command2.CommandText = "PR_MST_QuestionLevel_SelectAll";
            SqlDataReader reader2 = command2.ExecuteReader();
            DataTable dataTable2 = new DataTable();
            dataTable2.Load(reader2);
            reader2.Close();

            List<QuestionLevelDropDownModel> questionLevelList = new List<QuestionLevelDropDownModel>();

            foreach (DataRow data in dataTable2.Rows)
            {
                QuestionLevelDropDownModel questionLevelDropDownModel = new QuestionLevelDropDownModel();
                questionLevelDropDownModel.QuestionLevelID = Convert.ToInt32(data["QuestionLevelID"]);
                questionLevelDropDownModel.QuestionLevel = data["QuestionLevel"].ToString();
                questionLevelList.Add(questionLevelDropDownModel);
            }
            ViewBag.QuestionLevelList = questionLevelList;
            SqlCommand command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "PR_MST_Question_SelectByPK";

            command.Parameters.AddWithValue("@QuestionID", QuestionID);

            SqlDataReader reader = command.ExecuteReader();

            DataTable table = new DataTable();

            table.Load(reader);
            QuestionModel questionmodel = new QuestionModel();
            foreach (DataRow data in table.Rows) 
            { 
                questionmodel.QuestionID = Convert.ToInt32(data["QuestionID"]);
                questionmodel.QuestionText = data["QuestionText"].ToString();
                questionmodel.QuestionLevelID = Convert.ToInt32(data["QuestionLevelID"]);
                questionmodel.OptionA = data["OptionA"].ToString();
                questionmodel.OptionB = data["OptionB"].ToString();
                questionmodel.OptionC = data["OptionC"].ToString();
                questionmodel.OptionD = data["OptionD"].ToString();
                questionmodel.CorrectOption = data["CorrectOption"].ToString();
                questionmodel.QuestionMarks = Convert.ToInt32(data["QuestionMarks"]);
                questionmodel.IsActive = Convert.ToBoolean(data["IsActive"]);
                questionmodel.UserID = Convert.ToInt32(data["UserID"]);
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
            return View(questionmodel);
        }
        public IActionResult QuestionSave(QuestionModel questionmodel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (questionmodel.QuestionID <= 0)
                {
                    command.CommandText = "PR_MST_Question_Insert";
                }
                else
                {
                    command.CommandText = "PR_MST_Question_UpdateByPK";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = questionmodel.QuestionID;
                }
                    command.Parameters.Add("@QuestionText", SqlDbType.VarChar).Value = questionmodel.QuestionText;
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = questionmodel.QuestionLevelID;
                    command.Parameters.Add("@OptionA", SqlDbType.VarChar).Value = questionmodel.OptionA;
                    command.Parameters.Add("@OptionB", SqlDbType.VarChar).Value = questionmodel.OptionB;
                    command.Parameters.Add("@OptionC", SqlDbType.VarChar).Value = questionmodel.OptionC;
                    command.Parameters.Add("@OptionD", SqlDbType.VarChar).Value = questionmodel.OptionD;
                    command.Parameters.Add("@CorrectOption", SqlDbType.VarChar).Value = questionmodel.CorrectOption;
                    command.Parameters.Add("@QuestionMarks", SqlDbType.Int).Value = questionmodel.QuestionMarks;
                    command.Parameters.Add("@IsActive", SqlDbType.Bit).Value = questionmodel.IsActive;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = questionmodel.UserID;
                    command.ExecuteNonQuery();
                return RedirectToAction("Question_List");
            }
            return View("Question_Add_Edit",questionmodel);
        }
        
        public IActionResult QuestionExportToExcel()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_Question_SelectAll";
            //sqlCommand.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cells[1, 1].Value = "QuestionID";
                worksheet.Cells[1, 2].Value = "QuestionText";
                worksheet.Cells[1, 3].Value = "OptionA";
                worksheet.Cells[1, 4].Value = "OptionB";
                worksheet.Cells[1, 5].Value = "OptionC";
                worksheet.Cells[1, 6].Value = "OptionD";
                worksheet.Cells[1, 7].Value = "CorrectOption";
                worksheet.Cells[1, 8].Value = "QuestionMarks";
                worksheet.Cells[1, 9].Value = "IsActive";
                worksheet.Cells[1, 10].Value = "UserID";
                worksheet.Cells[1, 11].Value = "Created";
                worksheet.Cells[1, 12].Value = "Modified";

                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuestionID"];
                    worksheet.Cells[row, 2].Value = item["QuestionText"];
                    worksheet.Cells[row, 3].Value = item["OptionA"];
                    worksheet.Cells[row, 4].Value = item["OptionB"];
                    worksheet.Cells[row, 5].Value = item["OptionC"];
                    worksheet.Cells[row, 6].Value = item["OptionD"];
                    worksheet.Cells[row, 7].Value = item["CorrectOption"];
                    worksheet.Cells[row, 8].Value = item["QuestionMarks"];
                    worksheet.Cells[row, 9].Value = item["IsActive"];
                    worksheet.Cells[row, 10].Value = item["UserID"];
                    worksheet.Cells[row, 11].Value = item["Created"];
                    worksheet.Cells[row, 12].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Data-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
        public IActionResult Question_List()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_Question_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        public IActionResult QuestionDelete(int QuestionID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_Question_DeleteByPK";
                    command.Parameters.Add("@QuestionID", SqlDbType.Int).Value = QuestionID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "Question deleted successfully.";
                return RedirectToAction("Question_List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the question: " + ex.Message;
                return RedirectToAction("Question_List");
            }
        }
    }
}
