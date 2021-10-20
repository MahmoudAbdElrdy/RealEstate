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
        private readonly EmployeeService _service;
        public EmployeeController(EmployeeService EmployeeService)
        {
            _service = EmployeeService;
        }
        [HttpPost]
        [RealEstate.Service.Classes.Authorize]
        [Route("CreateUpdateemployee")]
        public ActionResult<ResponseData> CreateUpdateemployee(EmployeeDto employee)
        {

            try
            {
                var result = _service.Saveemployee(employee);
                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
        }


        [Route("/login")]
        [AllowAnonymous]
        [HttpPost]
        public ActionResult employeeInfo(EmployeeInfo employee)
        {

            IActionResult response = Unauthorized();
            try
            {
                if (!employee.Name.Equals("") && !employee.PassWord.Equals(""))
                {
                    string strtoken = _service.Getemployee(employee);
                    if (strtoken != null)
                    {
                        response = Ok(new { token = strtoken });
                        return (ActionResult)response;
                    }
                    else
                    {
                        return BadRequest("wrong employeename or password !");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new NotSupportedException(ex.Message);
            }
            return BadRequest(response);
        }
    }
}
