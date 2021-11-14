using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers.Contract
{
    [Route("api/[controller]")]
    [ApiController]
    [RealEstate.Service.Classes.Authorize]
    public class ContractController : ControllerBase
    {
        private readonly ContractService _service;
        public ContractController(ContractService ContractService)
        {
            _service = ContractService;
        }
        [HttpPost("GetAll")]
        public async Task<ActionResult<ResponseData>> GetAll(ContractSearch model)
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
        public ActionResult<ResponseData> CreateUpdatEContract( ContractDto Contract)
        {
            
          
            var result = _service.SaveContract(Contract);
            return Ok(result);

        }



    }
}
