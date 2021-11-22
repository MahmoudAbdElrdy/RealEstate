using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RealEstate.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RealEstate.Api.Controllers.DailyReport
{
    [Route("api/[controller]")]
    [ApiController]
    public class DailyReportController : ControllerBase
    {
        private readonly DailyReportService _service;
        public DailyReportController(DailyReportService DailyReportService)
        {
            _service = DailyReportService;
        }
        [HttpPost("GetAll")]
        public async Task<ActionResult<ResponseData>> GetAll(DailyReportSearch model)
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
        public ActionResult<ResponseData> CreateUpdateDailyReport(DailyReportDto DailyReport)
        {
            var result = _service.SaveDailyReport(DailyReport);
            return Ok(result);
             
        }
       

    }
}
