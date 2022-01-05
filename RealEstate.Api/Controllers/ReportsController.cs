using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;
using RealEstate.Data.StoredProc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
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

        [HttpPost("ReportExtraContrcat")]
        public async Task<ResponseData> ReportExtraContrcat(ExtraContrcatDto dto)
        {

            var data = (await _serviceReport.GetExtraContrcat((int)dto.ProjectID, dto.ContractExtraName)).Data;
            var projectName = (await _serviceProjec.GetName((int)dto.ProjectID)).Data;
      
           
            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data,
                Message=projectName
            };

        }
        [HttpPost("CustomerCard")]
        public async Task<ResponseData> CustomerCard(ContrcatCard customerName)
        {
            
            ContractReportDto parmarter = (await _contractService.GetByName(customerName.customerName)).Data;
            List<CustomerCard> data = (await _serviceReport.GetCustomerCard((int)parmarter.Id, false)).Data;

            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data.Where(x => x.IsExtra == false),
                Data2 = data.Where(x => x.IsExtra == true),
                Data3 = parmarter
            };
        }
        [HttpPost("ReportCustomerData")]
        public async Task<ResponseData> ReportCustomerData(ReportCustomerData report)
        {
           

            var data = (await _serviceReport.GetViewCustomerData(report.ProjectId)).Data;
          
            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data
            };
        }
        [HttpPost("ReportSalesYear")]
        public async Task<ResponseData> ReportSalesYear(Year year)
        {


            var data = (await _serviceReport.GetViewCustomerDatayear((int)year.year)).Data;
            var data2 = (await _serviceReport.GetViewCancelledContract((int)year.year)).Data;



            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data,
                Data2=data2
            };
        }
        [HttpPost("ReportAlert")]
        public async Task<ResponseData> ReportAlert(AlertReport model)
        {
            DateTime fromDate = ((DateTime)model.from);
            DateTime toDate = ((DateTime)model.to);
            int id = (int)model.id;
            var data = (await _serviceReport.GetAlert(id,fromDate , toDate)).Data;
            string projectName = "لايوجد مختار";
            if (id != 0)
            {
                 projectName = (await _serviceProjec.GetName(id)).Data;
            }
          
           
            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data,
                Message = projectName
            };
        }
        [HttpPost("ReportOverdue")]
        public async Task<ResponseData> ReportOverdue(ReportPrintBill report)
        {

            var data = (await _serviceReport.GetOverdue((int)report.id)).Data;
           
            string projectName = "لايوجد مختار";
            if ((int)report.id != 0)
            {
                projectName = (await _serviceProjec.GetName((int)report.id)).Data;
            }


            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data,
                Message = projectName
            };
        }
        [HttpPost("ReportBill")]
        public async Task<ResponseData> ReportBill(ReportPrintBill report)
        {

            var data = (await _serviceReport.GetPrintBill((int)report.id)).Data;


            var paid = (await _serviceReport.Getpaid((int)report.id)).Data;
            var paidReport =Convert.ToString(paid);
            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data,
                Message = paidReport
            };
        }
        [HttpPost("ReportCustomerWaiting")]
        public async Task<ResponseData> ReportCustomerWaiting(CustomerReport customerReport)
        {

           
            var data = (await _serviceReport.GetCustomerReport(customerReport)).Data;
         

            
            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data
            };
         
        }
        [HttpPost("ReportSupervisor")]
        public async Task<ResponseData> ReportSupervisor(SupervisorReport model) 
        {

            var res = await _serviceReport.GetSupervisorReport(model);
            var data = (res).Data;
            var data2 = (res).Data2;
            var supervisor = (await _serviceReport.GetSupervisor((int)model.SupervisorId)).Data;
            if (supervisor == null)
            {
                supervisor = "لم يتم اختار";
            }
        

            return new ResponseData
            {
                IsSuccess = true,
                Code = EResponse.OK,
                Data = data,
                Data2 = data2,
                Message= supervisor
            };
        }
    }
    public class AlertReport
    {
        public DateTime? from { get; set; }
        public DateTime? to { get; set; }
        public int? id { get; set; }
        public string region { get; set; }

    }
    public class ContrcatCard
    {
        public string customerName { get; set; }
    }
    public class Year
    {
        public int? year { get; set; }
    }
    public class ReportCustomerData
    {
      public  int? ProjectId { get; set; }
    }
    public class ReportPrintBill
    {
        public int? id { get; set; } 
    }
}
