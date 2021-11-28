using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RealEstate.Data.StoredProc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
        public ReportController(IWebHostEnvironment webHostEnvironment,
            ReportService serviceReport, ProjectService projectService,
            ContractService contractService,
            ContractDetailService contractDetailservice)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _contractDetailservice = contractDetailservice;
            _serviceReport = serviceReport;
            _serviceProjec = projectService;
            _contractService = contractService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> ReportExtraContrcat(ExtraContrcatDto dto)
        {
            string mym = "";
            int ext = 1;
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\ExtraContrcat.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();

            LocalReport localReport = new LocalReport(path);
            var data = (await _serviceReport.GetExtraContrcat((int)dto.ProjectID, dto.ContractExtraName)).Data;
            var projectName = (await _serviceProjec.GetName((int)dto.ProjectID)).Data;
            parmarters.Add("ProjectName", projectName ?? "");
            parmarters.Add("ExtraName", dto.ContractExtraName ?? "");
            localReport.AddDataSource("ContractExtraDataSet", data);
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

            return File(res.MainStream, "application/pdf");
        }
        [HttpGet]
        public async Task<IActionResult> CustomerCard(string customerName)
        {
            string mym = "";
            int ext = 1;
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerCardStock.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();

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

            localReport.AddDataSource("CustomerCardStock", data.Where(x=>x.IsExtra==false));
            localReport.AddDataSource("CustomerCardStock2", data.Where(x => x.IsExtra == true));
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

            return File(res.MainStream, "application/pdf");
        }
        [HttpGet]
        public async Task<IActionResult> ReportCustomerData() 
        {
            string mym = "";
            int ext = 1;
            var path = $"{_webHostEnvironment.WebRootPath}\\Reports\\CustomerData.rdlc";
            Dictionary<string, string> parmarters = new Dictionary<string, string>();

            LocalReport localReport = new LocalReport(path);
            var data = (await _serviceReport.GetViewCustomerData()).Data;
           
            parmarters.Add("Notes", "true" ?? "");
          
            localReport.AddDataSource("ViewCustomerData", data);
            var res = localReport.Execute(RenderType.Pdf, ext, parmarters, mym);

            return File(res.MainStream, "application/pdf");
        }
    }
}
