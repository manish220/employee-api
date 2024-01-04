using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace YourNamespace.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly List<Employee> _employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Nikhil", JoinDate = new DateTime(2022, 1, 15), IsRetired = false },
            new Employee { Id = 2, Name = "Ravi", JoinDate = new DateTime(2023, 3, 20), IsRetired = true },
            new Employee { Id = 3, Name = "Manish", JoinDate = new DateTime(2023, 5, 10), IsRetired = true },
        };

        private void LogErrorAndStatusCode(Exception ex, int statusCode, string message)
        {
            Log.Error(ex, message);
            Response.StatusCode = statusCode;
        }

        [HttpGet("{id}")]
        public ActionResult<Employee> GetEmployeeById(int id)
        {
            try
            {
                var employee = _employees.FirstOrDefault(e => e.Id == id);
                if (employee == null)
                {
                    return NotFound();
                }
                return Ok(employee);
            }
            catch (Exception ex)
            {
                LogErrorAndStatusCode(ex, 500, $"An error occurred while fetching employee with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("joined")]
        public ActionResult<IEnumerable<Employee>> GetEmployeesByDateRange(
            [FromQuery] DateTime startDate,
            [FromQuery] DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    return BadRequest("Invalid date range.");
                }

                var filteredEmployees = _employees
                    .Where(e => e.JoinDate >= startDate && e.JoinDate <= endDate)
                    .ToList();

                return Ok(filteredEmployees);
            }
            catch (Exception ex)
            {
                LogErrorAndStatusCode(ex, 500, "An error occurred while fetching employees by date range");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpPost]
        public ActionResult<Employee> AddEmployee([FromBody] Employee employee)
        {
            try
            {
                if (employee == null || string.IsNullOrWhiteSpace(employee.Name))
                {
                    return BadRequest("Employee object or name cannot be null or empty.");
                }

                if (_employees.Any(e => e.Name.Equals(employee.Name, StringComparison.OrdinalIgnoreCase)))
                {
                    return BadRequest("Employee with the same name already exists.");
                }

                // Additional input validations can be added here

                employee.Id = _employees.Count + 1;
                _employees.Add(employee);

                Log.Information($"Employee added - ID: {employee.Id}, Name: {employee.Name}");

                return CreatedAtAction(nameof(GetEmployeeById), new { id = employee.Id }, employee);
            }
            catch (Exception ex)
            {
                LogErrorAndStatusCode(ex, 500, "An error occurred while adding an employee");
                return StatusCode(500, "Internal server error");
            }
        }




        [HttpPut("{id}")]
        public ActionResult<Employee> UpdateEmployee(int id, [FromBody] Employee updatedEmployee)
        {
            try
            {
                var existingEmployee = _employees.FirstOrDefault(e => e.Id == id);
                if (existingEmployee == null)
                {
                    return NotFound();
                }

                existingEmployee.Name = updatedEmployee.Name;
                existingEmployee.JoinDate = updatedEmployee.JoinDate;

                Log.Information($"Employee updated - ID: {existingEmployee.Id}");

                return Ok(existingEmployee);
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while updating an employee with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteEmployee(int id)
        {
            try
            {
                var employeeToRemove = _employees.FirstOrDefault(e => e.Id == id);
                if (employeeToRemove == null)
                {
                    return NotFound();
                }

                _employees.Remove(employeeToRemove);

                Log.Information($"Employee deleted - ID: {id}");

                return NoContent();
            }
            catch (Exception ex)
            {
                Log.Error(ex, $"An error occurred while deleting an employee with ID {id}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
  