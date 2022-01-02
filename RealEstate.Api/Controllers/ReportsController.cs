using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Reporting.WebForms;
using RealEstate.DataAccess;
using System;
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
        public async Task<IActionResult> ReportExtraContrcat(ExtraContrcatDto dto)
        {
            var data = (await _serviceReport.GetExtraContrcat((int)dto.ProjectID, dto.ContractExtraName)).Data;
            var projectName = (await _serviceProjec.GetName((int)dto.ProjectID)).Data;
            var reportViewer = new ReportViewer { ProcessingMode = ProcessingMode.Local };
            reportViewer.LocalReport.ReportPath = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\ExtraContrcat.rdlc");
            var rds = new ReportDataSource();
            rds.Name = "ContractExtraDataSet";
            reportViewer.LocalReport.DataSources.Clear();
            reportViewer.LocalReport.DataSources.Add(data);

            string mym = "";
            int ext = (int)(DateTime.Now.Ticks >> 10);
            var path = Path.Combine($"{_webHostEnvironment.WebRootPath}\\Reports\\ExtraContrcat.rdlc");
            //Dictionary<string, string> parmarters = new Dictionary<string, string>();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding.GetEncoding("windows-1252");
          //  LocalReport localReport = new LocalReport(path);

          
            //parmarters.Add("ProjectName", projectName ?? "");
            //parmarters.Add("ExtraName", dto.ContractExtraName ?? "");
            //localReport.AddDataSource("ContractExtraDataSet", data.ToArray());
            Warning[] warnings;
            string[] streamids;
            string mimeType;
            string encoding;
            string extension;
            reportViewer.LocalReport.SetParameters(new ReportParameter("ProjectName", projectName ?? ""));
            reportViewer.LocalReport.SetParameters(new ReportParameter("ExtraName", dto.ContractExtraName ?? ""));
            var bytes = reportViewer.LocalReport.Render("application/pdf", null, out mimeType, out encoding, out extension, out streamids, out warnings);

            try
            {
             //   var res = localReport.Execute(RenderType.Pdf, ext, parmarters);

                return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet);
            }
            catch (Exception ex)
            {

                return File(ex.InnerException.Message, System.Net.Mime.MediaTypeNames.Application.Pdf);
            }
        }
    }
}