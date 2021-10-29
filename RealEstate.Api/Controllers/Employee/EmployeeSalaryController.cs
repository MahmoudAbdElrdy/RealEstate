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
    public class EmployeeSalaryController : ControllerBase
    {
        private readonly EmployeeSalaryService _service;
        public EmployeeSalaryController(EmployeeSalaryService EmployeeSalaryService)
        {
            _service = EmployeeSalaryService;
        }
        [HttpPost("GetAll")]
        public async Task<ActionResult<ResponseData>> GetAll(EmployeeSalarySearch model)
        {
            return await _service.GetAll(model);
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
        
        [Route("CreateUpdate")]
        public ActionResult<ResponseData> CreateUpdatEEmployeeSalary(EmployeeSalaryDto EmployeeSalary) 
        {

              var result = _service.SaveEmployeeSalary(EmployeeSalary);
                return Ok(result);
           
        }


      
    }
}
