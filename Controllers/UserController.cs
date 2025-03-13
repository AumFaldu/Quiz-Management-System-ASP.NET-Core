using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Quiz_Management_System.Models;
using static Quiz_Management_System.Models.UserModel;
using OfficeOpenXml;

namespace Quiz_Management_System.Controllers
{
    public class UserController : Controller
    {
        private IConfiguration configuration;

        public UserController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }
        public IActionResult Registration(int UserID)
        {
            UserModel usermodel = new UserModel();
            if (UserID == 0)
            {
                TempData["PageTitle"] = "User Registration";
            }
            else
            {
                TempData["PageTitle"] = "User Edit";
            }
            ViewBag.UserID = UserID;
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();

            command.CommandType = CommandType.StoredProcedure;

            command.CommandText = "PR_MST_User_SelectByPK";

            command.Parameters.AddWithValue("@UserID", UserID);

            SqlDataReader reader = command.ExecuteReader();

            DataTable table = new DataTable();

            table.Load(reader);
            foreach (DataRow row in table.Rows)
            {
                usermodel.UserID = Convert.ToInt32(row["UserID"]);
                usermodel.UserName = row["UserName"].ToString();
                usermodel.Mobile = row["Mobile"].ToString();
                usermodel.Email = row["Email"].ToString();
                usermodel.Password = row["Password"].ToString();
            }

            return View(usermodel);
        }
        public IActionResult UserSave(UserModel usermodel)
        {
            if (ModelState.IsValid)
            {
                string connectionString = this.configuration.GetConnectionString("ConnectionString");
                SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
                SqlCommand command = connection.CreateCommand();
                command.CommandType = CommandType.StoredProcedure;

                if (usermodel.UserID <= 0)
                {
                    command.CommandText = "PR_MST_User_Insert";
                }
                else
                {
                    command.CommandText = "PR_MST_User_UpdateByPK";
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = usermodel.UserID;
                }
                command.Parameters.Add("@UserName", SqlDbType.VarChar).Value = usermodel.UserName;
                command.Parameters.Add("@Password", SqlDbType.VarChar).Value = usermodel.Password;
                command.Parameters.Add("@Mobile", SqlDbType.VarChar).Value = usermodel.Mobile;
                command.Parameters.Add("@Email", SqlDbType.VarChar).Value = usermodel.Email;
                command.ExecuteNonQuery();
                return RedirectToAction("User_List");
            }
            return View("Registration", usermodel);
        }
        public IActionResult UserExportToExcel()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            SqlCommand sqlCommand = sqlConnection.CreateCommand();
            sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
            sqlCommand.CommandText = "PR_MST_User_SelectAll";
            //sqlCommand.Parameters.Add("@CityID", SqlDbType.Int).Value = CityID;

            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            DataTable data = new DataTable();
            data.Load(sqlDataReader);

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("DataSheet");

                // Add headers
                worksheet.Cells[1, 1].Value = "UserID";
                worksheet.Cells[1, 2].Value = "UserName";
                worksheet.Cells[1, 3].Value = "Password";
                worksheet.Cells[1, 4].Value = "Email";
                worksheet.Cells[1, 5].Value = "Mobile";
                worksheet.Cells[1, 6].Value = "IsActive";
                worksheet.Cells[1, 7].Value = "IsAdmin";
                worksheet.Cells[1, 8].Value = "Created";
                worksheet.Cells[1, 9].Value = "Modified";
                // Add data
                int row = 2;
                foreach (DataRow item in data.Rows)
                {
                    worksheet.Cells[row, 1].Value = item["UserID"];
                    worksheet.Cells[row, 2].Value = item["UserName"];
                    worksheet.Cells[row, 3].Value = item["Password"];
                    worksheet.Cells[row, 4].Value = item["Email"];
                    worksheet.Cells[row, 5].Value = item["Mobile"];
                    worksheet.Cells[row, 6].Value = item["IsActive"];
                    worksheet.Cells[row, 7].Value = item["IsAdmin"];
                    worksheet.Cells[row, 8].Value = item["Created"];
                    worksheet.Cells[row, 9].Value = item["Modified"];
                    row++;
                }

                var stream = new MemoryStream();
                package.SaveAs(stream);
                stream.Position = 0;

                string excelName = $"Data-{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }
        public IActionResult UserLogin(UserLoginModel userLoginModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    string connectionString = this.configuration.GetConnectionString("ConnectionString");
                    SqlConnection sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    SqlCommand sqlCommand = sqlConnection.CreateCommand();
                    sqlCommand.CommandType = System.Data.CommandType.StoredProcedure;
                    sqlCommand.CommandText = "PR_MST_User_ValidateLogin";
                    sqlCommand.Parameters.Add("@UserNameEmail", SqlDbType.VarChar).Value = userLoginModel.UserNameEmail;
                    sqlCommand.Parameters.Add("@Password", SqlDbType.VarChar).Value = userLoginModel.Password;  
                    SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                    DataTable dataTable = new DataTable();
                    dataTable.Load(sqlDataReader);
                    if (dataTable.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dataTable.Rows)
                        {
                            HttpContext.Session.SetString("UserID", dr["UserID"].ToString());
                            HttpContext.Session.SetString("UserName", dr["UserName"].ToString());
                            HttpContext.Session.SetString("EmailAddress", dr["Email"].ToString());
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "User is not found";
                        return RedirectToAction("Login", "User");
                    }

                }
            }
            catch (Exception e)
            {
                TempData["ErrorMessage"] = e.Message;
            }

            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "User");
        }
        [CheckAccess]
        public IActionResult User_List()
        {
            string connectionString = this.configuration.GetConnectionString("ConnectionString");
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.StoredProcedure;
            command.CommandText = "PR_MST_User_SelectAll";
            SqlDataReader reader = command.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            return View(table);
        }
        public IActionResult UserDelete(int UserID)
        {
            try
            {
                string connectionString = configuration.GetConnectionString("ConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = connection.CreateCommand();
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_MST_User_DeleteByPK";
                    command.Parameters.Add("@UserID", SqlDbType.Int).Value = UserID;


                    command.ExecuteNonQuery();
                }

                TempData["SuccessMessage"] = "User deleted successfully.";
                return RedirectToAction("User_List");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while deleting the user: " + ex.Message;
                return RedirectToAction("User_List");
            }
        }
    }
}
