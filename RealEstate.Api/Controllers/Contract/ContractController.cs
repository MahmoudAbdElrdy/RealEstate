using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess;
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
        private readonly ContractDetailBillService _contractDetailBillservice; 
        public ContractController(ContractService ContractService,
            ContractDetailBillService  contractDetailBillservice,
            ContractDetailService contractDetailservice)
        {
            _service = ContractService;
            _contractDetailservice = contractDetailservice;
            _contractDetailBillservice = contractDetailBillservice;
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
        [HttpPost]
        [Route("SaveListContractDetail")]
        public ActionResult<ResponseData> SaveListContractDetail(ContractDetailDtoList Contract)
        {
            var result = _contractDetailservice.SaveListContractDetail(Contract?.ContractDetailDtos);
            return Ok(result);
        }

        [HttpPost]
        [Route("GetAllInstallmentOverdue")]
        public ActionResult<ResponseData> GetAllInstallmentOverdue(ContractDetailDate Contract)
        {
            var result = _contractDetailservice.GetAllInstallmentOverdue(Contract);
            return Ok(result);
        }
        [HttpGet("GetAllInstallmentAlert")]
        public async Task<ActionResult<ResponseData>> GetAllInstallmentAlert(int id)
        {
            return await _contractDetailservice.GetAllInstallmentAlert(id);
        }
        // ContractDetailBill
        [HttpGet("GetAllContractDetailBill")]
        public async Task<ActionResult<ResponseData>> GetAllContractDetailBill(int contractId)
        {
            return await _contractDetailBillservice.GetAllContractDetailBill(contractId);
        }

        [HttpGet("GetByIdContractDetailBill")]
        public async Task<ActionResult<ResponseData>> GetByIdContractDetailBill(int id)
        {
            return await _contractDetailBillservice.GetByIdContractDetailBill(id);
        }
        [HttpGet("DeleteContractDetailBill")]
        public async Task<ActionResult<ResponseData>> DeleteContractDetailBill(int id)
        {
            return await _contractDetailBillservice.DeleteContractDetailBill(id);
        }
        [HttpPost]
        [Route("SaveContractDetailBill")]
        public ActionResult<ResponseData> SaveContractDetailBill(ContractDetailBillDto Contract)
        {
            var result = _contractDetailBillservice.SaveContractDetailBill(Contract);
            return Ok(result);
        }
    }
}
