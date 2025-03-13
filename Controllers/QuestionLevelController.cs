using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Quiz_Management_System.Models;
using System.Reflection;
using OfficeOpenXml;
using static Quiz_Management_System.Models.QuestionLevelModel;

namespace Quiz_Management_System.Controllers
{
    [CheckAccess]
    public class QuestionLevelController : Controller
    {
        private IConfiguration configuration;

        public QuestionLevelController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult QuestionLevel_Add_Edit(int QuestionLevelID)
        {
            if (QuestionLevelID==0)
            {
                TempData["PageTitle"] = "QuestionLevel Add";
            }
            else
            {
                TempData["PageTitle"] = "QuestionLevel Edit";
            }
            QuestionLevelModel questionLevelmodel = new QuestionLevelModel();
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_QuestionLevel_SelectByPK";
            command.Parameters.AddWithValue("@QuestionLevelID", SqlDbType.Int).Value = QuestionLevelID;
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            foreach (DataRow row in table.Rows)
            {
                questionLevelmodel.QuestionLevelID = Convert.ToInt32(row["QuestionLevelID"]);
                questionLevelmodel.QuestionLevel = row["QuestionLevel"].ToString();
                questionLevelmodel.UserID = Convert.ToInt32(row["UserID"]);
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
            return View(questionLevelmodel);
        }
        public IActionResult QuestionLevelSave(QuestionLevelModel questionLevelmodel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (questionLevelmodel.QuestionLevelID <= 0)
                {
                    command.CommandText = "PR_MST_QuestionLevel_Insert";
                }
                else
                {
                    command.CommandText = "PR_MST_QuestionLevel_UpdateByPK";
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = questionLevelmodel.QuestionLevelID;
                }
                    command.Parameters.Add("@QuestionLevel", SqlDbType.VarChar).Value = questionLevelmodel.QuestionLevel;
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = questionLevelmodel.UserID;
                    command.ExecuteNonQuery();
                return RedirectToAction("QuestionLevel_List");
            }
            return View("QuestionLevel_Add_Edit",questionLevelmodel);
        }
        public IActionResult QuestionLevelExportToExcel()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_QuestionLevel_SelectAll";
            //sqlCommand.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cells[1, 1].Value = "QuestionLevelID";
                worksheet.Cells[1, 2].Value = "QuestionLevel";
                worksheet.Cells[1, 3].Value = "UserID";
                worksheet.Cells[1, 4].Value = "Created";
                worksheet.Cells[1, 5].Value = "Modified";
                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["QuestionLevelID"];
                    worksheet.Cells[row, 2].Value = item["QuestionLevel"];
                    worksheet.Cells[row, 3].Value = item["UserID"];
                    worksheet.Cells[row, 4].Value = item["Created"];
                    worksheet.Cells[row, 5].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Data-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
        public IActionResult QuestionLevel_List()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_QuestionLevel_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        public IActionResult QuestionLevelDelete(int QuestionLevelID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_QuestionLevel_DeleteByPK";
                    command.Parameters.Add("@QuestionLevelID", SqlDbType.Int).Value = QuestionLevelID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "QuestionLevel deleted successfully.";
                return RedirectToAction("QuestionLevel_List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the QuestionLevel: " + ex.Message;
                return RedirectToAction("QuestionLevel_List");
            }
        }
    }
}
