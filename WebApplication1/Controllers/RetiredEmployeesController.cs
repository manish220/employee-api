using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourNamespace.Controllers;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RetiredEmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private static List<Employee> _employees = new List<Employee>
        {
            new Employee { Id = 1, Name = "Nikhil", JoinDate = new DateTime(2022, 1, 15), IsRetired = false },
            new Employee { Id = 2, Name = "Ravi", JoinDate = new DateTime(2023, 3, 20), IsRetired = true },
            new Employee { Id = 3, Name = "Manish", JoinDate = new DateTime(2023, 5, 10), IsRetired = true },

        };

        public RetiredEmployeesController(ILogger<EmployeesController> logger)
        {
            _logger = logger;
        }


        [HttpGet]

        public ActionResult<IEnumerable<Employee>> GetRetiredEmployees()
        {
            var retiredEmployees = _employees.Where(e => e.IsRetired).ToList();

            return Ok(retiredEmployees);

        }
    }
}

/*
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using YourNamespace.Controllers;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class RetiredEmployeesController : ControllerBase
    {
        [HttpGet] 

        public ActionResult<IEnumerable<Employee>> GetRetiredEmployees()
        {
            var retiredEmployees = _employees.Employees.Where(e => e.IsRetired).ToList();

            return Ok(retiredEmployees);

        }
    }
}
*/