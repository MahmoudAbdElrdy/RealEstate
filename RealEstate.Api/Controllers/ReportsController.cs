using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;
using RealEstate.Data.StoredProc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        IWebHostEnvironment _webHostEnvironment;
        private readonly ProjectService _serviceProjec;
        private readonly ReportService _serviceReport;
        private readonly ContractDetailService _contractDetailservice;
        private readonly ContractService _contractService;
        private readonly CustomerService _customerService;

        public ReportsController(IWebHostEnvironment webHostEnvironment,
            ReportService serviceReport, ProjectService projectService,
            ContractService contractService,
            CustomerService customerService,
            ContractDetailService contractDetailservice)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _contractDetailservice = contractDetailservice;
            _serviceReport = serviceReport;
            _serviceProjec = projectService;
            _contractService = contractService;
            _customerService = customerService;
        }

        [HttpGet]
        public async Task<ResponseData> ReportExtraContrcat(ExtraContrcatDto dto)
        {

            var data = (await _serviceReport.GetExtraContrcat((int)dto.ProjectID, dto.ContractExtraName)).Data;
            var projectName = (await _serviceProjec.GetName((int)dto.ProjectID)).Data;
            return new ResponseData();

        }
        [HttpGet]
        public async Task<ResponseData> CustomerCard(string customerName)
        {
            
            ContractReportDto parmarter = (await _contractService.GetByName(customerName)).Data;
            List<CustomerCard> data = (await _serviceReport.GetCustomerCard((int)parmarter.Id, false)).Data;
           
            return new ResponseData();
        }
        [HttpGet]
        public async Task<ResponseData> ReportCustomerData(int option, int? ProjectId)
        {
           

            var data = (await _serviceReport.GetViewCustomerData(ProjectId)).Data;
            return new ResponseData();
        }
        [HttpGet]
        public async Task<ResponseData> ReportSalesYear(int year)
        {
           

            var data = (await _serviceReport.GetViewCustomerData(year)).Data;
            var data2 = (await _serviceReport.GetViewCancelledContract(year)).Data;
          

            return new ResponseData();
        }
        [HttpGet]
        public async Task<ResponseData> ReportAlert(int id, DateTime from, DateTime to)
        {
           

            var data = (await _serviceReport.GetAlert(id, from, to)).Data;
            var projectName = (await _serviceProjec.GetName((int)id)).Data;
            return new ResponseData();
        }
        [HttpGet]
        public async Task<ResponseData> ReportOverdue(int id)
        {
            
            var data = (await _serviceReport.GetOverdue(id)).Data;
            var projectName = (await _serviceProjec.GetName((int)id)).Data;
            return new ResponseData();
        }
        [HttpGet]
        public async Task<ResponseData> ReportBill(int id)
        {

            var data = (await _serviceReport.GetPrintBill(id)).Data;


            var paid = (await _serviceReport.Getpaid((int)id)).Data;
            return new ResponseData();
        }
        [HttpGet]
        public async Task<ResponseData> ReportCustomerWaiting(string region, DateTime? from, DateTime? to)
        {

            CustomerReport customerReport = new CustomerReport();
            customerReport.Region = region;
            customerReport.FormDate = from;
            customerReport.ToDate = to;
            var data = (await _serviceReport.GetCustomerReport(customerReport)).Data;

            return new ResponseData();
        }
        [HttpGet]
        public async Task<ResponseData> ReportSupervisor(int supervisorId, DateTime? from, DateTime? to)
        {
            
            SupervisorReport customerReport = new SupervisorReport();
            customerReport.SupervisorId = supervisorId;
            customerReport.FromDate = from;
            customerReport.ToDate = to;
            var data = (await _serviceReport.GetSupervisorReport(customerReport)).Data;
            var supervisor = (await _serviceReport.GetSupervisor((int)customerReport.SupervisorId)).Data;

            return new ResponseData();
        }
    }
}
