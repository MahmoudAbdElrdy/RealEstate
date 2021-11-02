﻿using Microsoft.AspNetCore.Authorization;
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
    public class ProjectController : ControllerBase
    {
        private readonly ProjectService _service;
        public ProjectController(ProjectService ProjectService)
        {
            _service = ProjectService;
        }
        [HttpPost("GetAll")]
        public async Task<ActionResult<ResponseData>> GetAll(ProjectSearch model)
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
        public ActionResult<ResponseData> CreateUpdatEProject(ProjectDto Project) 
        {

              var result = _service.SaveProject(Project);
                return Ok(result);
           
        }


      
    }
}