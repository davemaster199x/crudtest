using Microsoft.AspNetCore.Mvc;
using System.Data;
using TestCrud.Models;
using TestCrud.DataAccess;

namespace TestCrud.Controllers
{
    public class UserController : Controller
    {
        private readonly ILogger<UserController> _logger;
        private readonly string connectionString;

        public UserController(ILogger<UserController> logger)
        {
            _logger = logger;
            connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crud_db;Integrated Security=True;";
        }
        public IActionResult Edit(int id)
        {
            try
            {
                string query = $"SELECT user_id, firstname, lastname FROM tbl_users WHERE user_id = {id};";
                TestCrud.DataAccess.DataAccess dataAccess = new TestCrud.DataAccess.DataAccess(connectionString);

                // Fetch user details from the database
                DataTable dataTable = dataAccess.ExecuteQueryWithResult(query);

                // Check if user exists
                if (dataTable.Rows.Count == 0)
                {
                    // If user does not exist, return not found
                    return View("Error");
                }

                // Get user details from the DataTable
                DataRow row = dataTable.Rows[0];
                var user = new User
                {
                    user_id = Convert.ToInt32(row["user_id"]),
                    firstname = row["firstname"].ToString(),
                    lastname = row["lastname"].ToString()
                };

                // Return the view with user details
                return View("~/Views/User/Edit.cshtml", user);
            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError(ex, "Error fetching user details from the database");

                ViewBag.Message = "Error fetching user details from the database";

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditUser(int id, User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string user_id = user.user_id.ToString();

                    string query = $"UPDATE tbl_users SET firstname = '{user.firstname}', lastname = '{user.lastname}' WHERE user_id = {user_id};";
                    TestCrud.DataAccess.DataAccess dataAccess = new TestCrud.DataAccess.DataAccess(connectionString);

                    // Execute the update query
                    DataTable dataTable = dataAccess.ExecuteQueryWithResult(query);

                    // Redirect to the index action of HomeController after successful update
                    return RedirectToAction("Index", "Home");
                }
                catch (Exception ex)
                {
                    // Log error
                    _logger.LogError(ex, "Error updating user details in the database");
                    // Optionally, set an error message to display in the view
                    ViewBag.Message = "Error updating user details in the database";
                    // Return an error view
                    return View("Error");
                }
            }
            return Content("Error: ModelState is not valid.");
        }

        public IActionResult delete(string id)
        {
            try
            {
                string query = $"DELETE FROM tbl_users WHERE user_id = {id};";
                TestCrud.DataAccess.DataAccess dataAccess = new TestCrud.DataAccess.DataAccess(connectionString);

                // Execute the update query
                DataTable dataTable = dataAccess.ExecuteQueryWithResult(query);

                // Redirect to the index action of HomeController after successful update
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError(ex, "Error deleting user details in the database");
                // Optionally, set an error message to display in the view
                ViewBag.Message = "Error deleting user details in the database";
                // Return an error view
                return View("Error");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View("~/Views/User/Create.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateUser(User user)
        {
            try
            {
                string query = $"INSERT INTO tbl_users (firstname, lastname) VALUES ('{user.firstname}', '{user.lastname}');";
                TestCrud.DataAccess.DataAccess dataAccess = new TestCrud.DataAccess.DataAccess(connectionString);

                // Execute the update query
                DataTable dataTable = dataAccess.ExecuteQueryWithResult(query);
            }
            catch (Exception)
            {
                return View("Error");
            }

            return RedirectToAction("Index", "Home");
        }
    }
}
