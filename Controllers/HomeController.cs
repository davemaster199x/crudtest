using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using TestCrud.Models;
using TestCrud.DataAccess;

namespace TestCrud.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string connectionString;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=crud_db;Integrated Security=True;";
        }

        public IActionResult Index()
        {
            List<User> userList = new List<User>();

            try
            {
                string query = "SELECT * FROM tbl_users;";
                TestCrud.DataAccess.DataAccess dataAccess = new TestCrud.DataAccess.DataAccess(connectionString);

                // Fetch data from the database
                userList = dataAccess.GetUsers(query);

                // Log success message
                _logger.LogInformation("Query executed successfully");

            }
            catch (Exception ex)
            {
                // Log error
                _logger.LogError(ex, "Error executing query");

                return View("Error");
            }

            // Pass the user list to the view
            return View("~/Views/User/Index.cshtml", userList);
        }
    }
}
