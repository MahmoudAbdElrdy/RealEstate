using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers.ProjectVisit
{
    [Route("api/[controller]")]
    [ApiController]
    [RealEstate.Service.Classes.Authorize]
    public class ProjectVisitController : ControllerBase
    {
        private readonly ProjectVisitService _service;
        public ProjectVisitController(ProjectVisitService ProjectVisitService)
        {
            _service = ProjectVisitService;
        }
        [HttpPost("GetAll")]
        public async Task<ActionResult<ResponseData>> GetAll(ProjectVisitSearch model)
        {
            return await _service.GetAll(model);
        }  
        [HttpGet("GetDropDownList")]
        public async Task<ActionResult<ResponseData>> GetDropDownList()
        {
            return await _service.GetDropDownList();
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
        public ActionResult<ResponseData> CreateUpdate(ProjectVisitDto ProjectVisit) 
        {
          

            var result = _service.SaveProjectVisit(ProjectVisit);
            return Ok(result);

        }

    }
}
