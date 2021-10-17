using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _EmployeeService;
        public EmployeeController(EmployeeService EmployeeService)
        {
            _EmployeeService = EmployeeService;
        }

        [HttpGet("GetUser")]
        public async Task<ActionResult<EmployeeDto>> GetUser(int UserId)
        {
            return  _EmployeeService.GetUser(UserId);
        }
    }
}
