using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers.Supervisor
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorController : ControllerBase
    {
        private readonly SupervisorService _service;
        public SupervisorController(SupervisorService SupervisorService)
        {
            _service = SupervisorService;
        }
        [HttpPost("GetAll")]
        public async Task<ActionResult<ResponseData>> GetAll(SupervisorSearch model)
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
        public ActionResult<ResponseData> CreateUpdateSupervisor(SupervisorDto Supervisor)
        {
            var result = _service.SaveSupervisor(Supervisor);
            return Ok(result);
             
        }
        [HttpPost]
        [Route("SaveSupervisorDetail")]
        public ActionResult<ResponseData> SaveSupervisorDetail(SupervisorDetailDto supervisorDetail) 
        {
            var result = _service.SaveSupervisorDetail(supervisorDetail);
            return Ok(result);

        }

    }
}
