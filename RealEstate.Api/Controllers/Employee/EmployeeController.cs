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
    [RealEstate.Service.Classes.Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _service;
        public EmployeeController(EmployeeService EmployeeService)
        {
            _service = EmployeeService;
        }
        [HttpPost("GetAll")]
        public async Task<ActionResult<ResponseData>> GetAll(EmployeeSearch model)
        {
            return await _service.GetAll(model);
        }
        [HttpGet("GetAllDepartments")]
        public async Task<ActionResult<ResponseData>> GetAllDepartments()
        {
            return await _service.GetAllDepartments();
        }
        [HttpGet("GetById")]
        public async Task<ActionResult<ResponseData>> GetById(int id)
        {
            return await _service.GetById(id);
        }
        [HttpGet("Delete")]
        public async Task<ActionResult<ResponseData>> Delete(int id)
        {
            return await _service.Delete(id);
        }
        [HttpPost]
        
        [Route("CreateUpdatEemployee")]
        public ActionResult<ResponseData> CreateUpdatEemployee(EmployeeDto employee) 
        {

              var result = _service.SaveEmployee(employee);
                return Ok(result);
           
        }


        [Route("login")]
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
