using AdoDotNetAssignment.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace AdoDotNetAssignment.Controllers
{
    public class EmployeeController : Controller
    {
        SqlConnection connection = null;
        static IConfiguration _configuration;

        public EmployeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        SqlCommand command = null;
        public IActionResult Index()
        {
            List<Employee> employees = new List<Employee>();
            using (connection = GetConnection())
            {
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = "Select * from Employee";
                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee()
                                {
                                    Id = (int)reader["id"],
                                    Name = reader["name"].ToString(),
                                    Doj = DateOnly.FromDateTime((DateTime)reader["doj"]),
                                    Salary = (int)reader["salary"]

                                };
                                employees.Add(employee);

                            }
                        }
                        else
                        {
                            ViewBag.msg = "There are no records";
                            return View();
                        }
                    }
                }
            }
            return View(employees);
        }

        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public IActionResult Create(Employee employee)
        {
            using (connection = GetConnection())
            {
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = $"insert into Employee(id,name, doj,salary) values('{employee.Id}','{employee.Name}','{employee.Doj.ToString("yyyy-MM-dd")}', {employee.Salary})";
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        public IActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.msg = "Please provide a ID"; return View();
            }
            else
            {
                Employee emp = null;
                //List<Employee> employees = new List<Employee>();
                using (connection = GetConnection())
                {
                    {
                        using (command = new SqlCommand())
                        {
                            command.CommandText = $"Select * from Employee where Id='{id}'";
                            command.Connection = connection;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Employee employee = new Employee()
                                    {
                                        Id = (int)reader["id"],
                                        Name = reader["name"].ToString(),
                                        Doj = DateOnly.FromDateTime((DateTime)reader["doj"]),
                                        Salary = (int)reader["salary"]
                                    };
                                    emp = employee;
                                }
                            }
                            else
                            {
                                ViewBag.msg = "There is no such record";
                                return View();
                            }
                        }
                    }
                }
                //var emp = employees.Where(x => x.Id == id).FirstOrDefault();
                if (emp == null)
                {
                    ViewBag.msg = "There is no record woth this ID";
                    return View();
                }
                else
                    return View(emp);
            }
        }

        [HttpPost]
        public IActionResult Delete(Employee employee, int id)
        {
            using (connection = GetConnection())
            {
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = $"DELETE FROM Employee WHERE Id='{id}';";
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        return RedirectToAction("Index");
                    }
                }
            }
        }

        public IActionResult Display(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.msg = "Please provide a ID"; return View();
            }
            else
            {
                Employee emp = null;
                using (connection = GetConnection())
                {
                    {
                        using (command = new SqlCommand())
                        {
                            command.CommandText = $"Select * from Employee where Id='{id}'";
                            command.Connection = connection;
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();

                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Employee employee = new Employee()
                                    {
                                        Id = (int)reader["id"],
                                        Name = reader["name"].ToString(),
                                        Doj = DateOnly.FromDateTime((DateTime)reader["doj"]),
                                        Salary = (int)reader["salary"]
                                    };
                                    emp = employee;
                                }
                                return View(emp);
                            }
                            else
                            {
                                ViewBag.msg = "There is no such record";
                                return View();
                            }
                        }
                    }
                }
            }
        }

        public IActionResult Edit(int? id)
        {
            if (!id.HasValue)
            {
                ViewBag.msg = "Please provide an ID";
                return View();
            }
            else
            {
                Employee emp = null;
                using (connection = GetConnection())
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = $"Select * from Employee where Id='{id}'";
                        command.Connection = connection;
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                Employee employee = new Employee()
                                {
                                    Id = (int)reader["id"],
                                    Name = reader["name"].ToString(),
                                    Doj = DateOnly.FromDateTime((DateTime)reader["doj"]),
                                    Salary = (int)reader["salary"]
                                };
                                emp = employee;
                            }
                            return View(emp);
                        }
                        else
                        {
                            ViewBag.msg = "There is no such record";
                            return View();
                        }
                    }
                }
            }
        }

        [HttpPost]
        public IActionResult Edit(Employee employee)
        {
            if (ModelState.IsValid)
            {
                using (connection = GetConnection())
                {
                    using (command = new SqlCommand())
                    {
                        command.CommandText = $"UPDATE Employee SET Name='{employee.Name}', Doj='{employee.Doj.ToString("yyyy-MM-dd")}', Salary={employee.Salary} WHERE Id={employee.Id}";
                        command.Connection = connection;
                        connection.Open();
                        command.ExecuteNonQuery();
                        connection.Close();
                        return RedirectToAction("Index");
                    }
                }
            }
            // If the model state is not valid, return to the edit view with the existing data
            return View(employee);
        }


        static string GetConnectionString()
        {
            return _configuration.GetConnectionString("EmployeeDbCS").ToString();
            
        }

        static SqlConnection GetConnection()
        {
            return new SqlConnection(GetConnectionString());
        }
    }
}
