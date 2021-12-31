using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Data.StoredProc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers
{
    // [RealEstate.Service.Classes.Authorize]
    public class ReportController : Controller
    {
        IWebHostEnvironment _webHostEnvironment;
        private readonly ProjectService _serviceProjec;
        private readonly ReportService _serviceReport;
        private readonly ContractDetailService _contractDetailservice;
        private readonly ContractService _contractService;
        private readonly CustomerService _customerService;
        public ReportController(IWebHostEnvironment webHostEnvironment,
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
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ReportExtraContrcat(ExtraContrcatDto dto)
        {
            string mym = "";
             int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\ExtraContrcat.rdlc");
            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            var data = (await _serviceReport.GetExtraContrcat((int)dto.ProjectID, dto.ContractExtraName)).Data;
            var projectName = (await _serviceProjec.GetName((int)dto.ProjectID)).Data;
            parmarters.Add("ProjectName", projectName ?? "");
            parmarters.Add("ExtraName", dto.ContractExtraName ?? "");
            localReport.AddDataSource("ContractExtraDataSet", data);
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

           return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
        [HttpGet]
        public async Task<IActionResult> CustomerCard(string customerName)
        {
            string mym = "";
             int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerCardStock.rdlc");
            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            ContractReportDto parmarter = (await _contractService.GetByName(customerName)).Data;
            List<CustomerCard> data = (await _serviceReport.GetCustomerCard((int)parmarter.Id, false)).Data;
            //  var date = DateTime.ParseExact((DateTime)parmarter.Date, "yyyyMMdd", CultureInfo.InvariantCulture);
            parmarters.Add("ProjectName", parmarter.ProjectName ?? "");
            parmarters.Add("Name", parmarter.Name ?? "");
            parmarters.Add("Number", parmarter?.Number.ToString() ?? "");
            parmarters.Add("Area", parmarter?.Area.ToString() ?? "");
            parmarters.Add("FloorNumber", parmarter?.FloorNumber.ToString() ?? "");
            parmarters.Add("NationalNumber", parmarter.NationalNumber ?? "");
            parmarters.Add("Address", parmarter.Address ?? "");
            parmarters.Add("Phone", parmarter?.Phone ?? "");
            parmarters.Add("ProjectAddress", parmarter.ProjectAddress ?? "");
            parmarters.Add("Notes", parmarter.Notes ?? "");

            parmarters.Add("Date", parmarter?.Date.Value.ToString("dd-MM-yyyy") ?? "");

            localReport.AddDataSource("CustomerCardStock", data.Where(x => x.IsExtra == false));
            localReport.AddDataSource("CustomerCardStock2", data.Where(x => x.IsExtra == true));
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

           return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
        [HttpGet]
        public async Task<IActionResult> ReportCustomerData(int option,int? ProjectId)
        {
            // { option: 'الكل', value: 1 },
            //{ option: 'رقم التليفون فقط', value: 2 },
            //{ option: 'تليفون والعنوان ورقم القومى', value: 3 },
            //{ option: ' العنوان فقط', value: 4 },
            //{ option: ' تاريخ التعاقد وملاحظات ونظام الدفع ', value: 5}
            string mym = "";
             int ext = (int)(DateTime.Now.Ticks >> 10);
            string path = "";
            if (option == 1)
            {
                path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerData.rdlc");
            }
            else if (option == 2)
            {
                path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerData2.rdlc");
            }
            else if (option == 3)
            {
                path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerData3.rdlc");
            }
            else if (option == 4)
            {
                path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerData4.rdlc");
            }
            else if (option == 5)
            {
                path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerData5.rdlc");
            }
            else
            {
                path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerData.rdlc");
            }

            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            var data = (await _serviceReport.GetViewCustomerData(ProjectId)).Data;

            parmarters.Add("Notes", "true" ?? "");

            localReport.AddDataSource("ViewCustomerData", data);
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

           return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
        [HttpGet]
        public async Task<IActionResult> ReportSalesYear(int year)
        {
            string mym = "";
             int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\SalesYear.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            var data = (await _serviceReport.GetViewCustomerData(year)).Data;
            var data2 = (await _serviceReport.GetViewCancelledContract(year)).Data;
            var res1 = year;
            parmarters.Add("year", Convert.ToString(res1));
            //parmarters.Add("ReportParameter1", res1 ?? "");

            localReport.AddDataSource("ViewCustomerData", data);
            localReport.AddDataSource("CancelledContract", data2);
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

           return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
        [HttpGet]
        public async Task<IActionResult> ReportAlert(int id, DateTime from, DateTime to)
        {
            string mym = "";
            int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Alert.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            var data = (await _serviceReport.GetAlert(id, from, to)).Data;
            var projectName = (await _serviceProjec.GetName((int)id)).Data;
            if (id == null||id==0)
                projectName = "لايوجد";
            parmarters.Add("ProjectName", projectName ?? "");
            parmarters.Add("FromDate", from.ToString("dd-MM-yyyy") ?? "");
            parmarters.Add("ToDate", to.ToString("dd-MM-yyyy") ?? "");
            localReport.AddDataSource("AlertDataSet", data);
            byte[] fileBytes = System.IO.File.ReadAllBytes(path);
            //localReport.AddDataSource("CancelledContract", data2);
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters);
            //return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);

            return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet, "Alert");
        }
        [HttpGet]
        public async Task<IActionResult> ReportOverdue(int id)
        {
            string mym = "";
             int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Overdue.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            var data = (await _serviceReport.GetOverdue(id)).Data;
            var projectName = (await _serviceProjec.GetName((int)id)).Data;
            parmarters.Add("ProjectName", projectName ?? "");
            DateTime now = DateTime.Now;
            var startDate = new DateTime(now.Year, now.Month, 1);
           
            localReport.AddDataSource("AlertDataSet", data);
            //localReport.AddDataSource("CancelledContract", data2);
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

           return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
        [HttpGet]
        public async Task<IActionResult> ReportBill(int id)
        {
            string mym = "";
             int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\PrintBill.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            var data = (await _serviceReport.GetPrintBill(id)).Data;
            parmarters.Add("ProjectName", data[0].ProjectName ?? "");
            parmarters.Add("CustomerPhone", data[0].CustomerPhone ?? "");
            parmarters.Add("NationalNumber", data[0].NationalNumber ?? "");
            parmarters.Add("Stock", data[0].Stock ?? "");
            parmarters.Add("CustomerName", data[0].CustomerName ?? "");
           
            var paid = (await _serviceReport.Getpaid((int)id)).Data;
            parmarters.Add("Paid", paid.ToString() ?? "");
          

            localReport.AddDataSource("PrintBillDataSet", data);
           
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

            return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet);
        }
        [HttpGet]
        public async Task<IActionResult> ReportCustomerWaiting(string region, DateTime? from, DateTime? to)
        {
            string mym = "";
            int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerWaiting.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            CustomerReport customerReport = new CustomerReport();
            customerReport.Region = region;
            customerReport.FormDate = from;
            customerReport.ToDate = to;
            var data = (await _serviceReport.GetCustomerReport(customerReport)).Data;
          
            parmarters.Add("Region", region ?? "");
            parmarters.Add("FromDate", from?.ToString("dd-MM-yyyy") ?? "");
            parmarters.Add("ToDate", to?.ToString("dd-MM-yyyy") ?? "");
            localReport.AddDataSource("DataSetCustomer", data);
          
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters);
          
            return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet, "Alert");
        }
        [HttpGet]
        public async Task<IActionResult> ReportSupervisor(int supervisorId, DateTime? from, DateTime? to)
        { 
            string mym = "";
            int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\Supervisor.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
            LocalReport localReport = new LocalReport(path);
            SupervisorReport customerReport = new SupervisorReport();
            customerReport.SupervisorId = supervisorId;
            customerReport.FromDate = from;
            customerReport.ToDate = to;
            var data = (await _serviceReport.GetSupervisorReport(customerReport)).Data;
            var supervisor = (await _serviceReport.GetSupervisor((int)customerReport.SupervisorId)).Data;

            parmarters.Add("Supervisor", supervisor ?? "");
            parmarters.Add("FromDate", from?.ToString("dd-MM-yyyy") ?? "");
            parmarters.Add("ToDate", to?.ToString("dd-MM-yyyy") ?? "");
            localReport.AddDataSource("DataSetSupervisor", data);

            var res = localReport.Execute(RenderType.Pdf, ext, parmarters);

            return File(res.MainStream, System.Net.Mime.MediaTypeNames.Application.Octet, supervisor);
        }
    }
}
