using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers
{


    public class ReportController : Controller
    {
        IWebHostEnvironment _webHostEnvironment;
        private readonly ProjectService _serviceProjec;
        private readonly ReportService _serviceReport;
        private readonly ContractDetailService _contractDetailservice;
        private readonly ContractDetailBillService _contractDetailBillservice;
        public ReportController(IWebHostEnvironment webHostEnvironment,
            ReportService serviceReport, ProjectService projectService,
            ContractDetailService contractDetailservice)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _contractDetailservice = contractDetailservice;
            _serviceReport = serviceReport;
            _serviceProjec = projectService;
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
    }
}
