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
        [HttpGet("GetAllName")]
        public async Task<ActionResult<ResponseData>> GetAllName()
        {
            return await _service.GetAllName();
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
        [HttpPost("GetAllCancelledContracts")]
        public async Task<ActionResult<ResponseData>> GetAllCancelledContracts(CancelledContractDto search)
        {
            return await _service.GetAllCancelledContracts(search);
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
        [HttpGet("DeleteAllContractDetail")]
        public async Task<ActionResult<ResponseData>> DeleteAllContractDetail(int id)
        {
            return await _contractDetailservice.DeleteAllContractDetail(id);
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

        [HttpGet]
        [Route("GetAllInstallmentOverdue")]
        public async Task<ActionResult<ResponseData>> GetAllInstallmentOverdueAsync(int contractId)
        {
            var result =await _contractDetailservice.GetAllInstallmentOverdue(contractId);
            return Ok(result);
        }
        [HttpPost("GetAllInstallmentAlert")]
        public async Task<ActionResult<ResponseData>> GetAllInstallmentAlert(ContractDetailDate date)
        {
            return await _contractDetailservice.GetAllInstallmentAlert(date);
        }
        // ContractDetailBill
        [HttpGet("GetAllContractDetailBill")]
        public async Task<ActionResult<ResponseData>> GetAllContractDetailBill(int ContractDetailId)
        {
            return await _contractDetailBillservice.GetAllContractDetailBill(ContractDetailId);
        }
         [HttpGet("GetAllInstallmentNotPaid")]
        public async Task<ActionResult<ResponseData>> GetAllInstallmentNotPaid(int contractId)
        {
            return await _contractDetailservice.GetAllInstallmentNotPaid(contractId);
        }
        [HttpGet("GetAllViewPayInstallments")]
        public async Task<ActionResult<ResponseData>> GetAllViewPayInstallments(int contractId)
        {
            return await _contractDetailservice.GetAllViewPayInstallments(contractId);
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
