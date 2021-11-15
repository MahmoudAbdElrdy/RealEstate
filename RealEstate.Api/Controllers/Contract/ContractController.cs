using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess;
using RealEstate.DataAccess.Contract;
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
        private readonly ContractDetailService _contractDetailservice;
        public ContractController(ContractService ContractService, ContractDetailService contractDetailservice)
        {
            _service = ContractService;
            _contractDetailservice = contractDetailservice;
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
        public ActionResult<ResponseData> CreateUpdatEContract(ContractDto Contract)
        {
            var result = _service.SaveContract(Contract);
            return Ok(result);
        } 
        [HttpPost]
        [Route("CancellContract")]
        public async Task<ActionResult<ResponseData>> CancellContractAsync(CancelledContractDto cancelledContract)
        {
            var result =await _service.CancellContract(cancelledContract);
            return Ok(result);

        }

        // ContractDetail
        [HttpGet("GetAllContractDetail")]
        public async Task<ActionResult<ResponseData>> GetAllContractDetail(int contractId)
        {
            return await _contractDetailservice.GetAllContractDetail(contractId);
        }

        [HttpGet("GetByIdContractDetail")]
        public async Task<ActionResult<ResponseData>> GetByIdContractDetail(int id)
        {
            return await _contractDetailservice.GetByIdContractDetail(id);
        }
        [HttpGet("DeleteContractDetail")]
        public async Task<ActionResult<ResponseData>> DeleteContractDetail(int id)
        {
            return await _contractDetailservice.DeleteContractDetail(id);
        }
        [HttpPost]
        [Route("SaveContractDetail")]
        public ActionResult<ResponseData> SaveContractDetail(ContractDetailDto Contract)
        {
            var result = _contractDetailservice.SaveContractDetail(Contract);
            return Ok(result);
        }

    }
}
