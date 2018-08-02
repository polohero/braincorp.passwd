using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BrainCorp.Passwd.WebService.Controllers
{
    /// <summary>
    /// I wouldn't normally do this as there are really good tools to measure
    /// performance of services. But, I want to monitor and measure
    /// and see if you guys are actually calling my service :).
    /// </summary>
    public class DiagnosticsController : ApiController
    {
        [HttpGet]
        [Route("api/diagnostics/metrics")]
        public string GetMetrics()
        {
            return Diagnostics.DiagnosticsLogger.NumberOfCalls + "|" + Diagnostics.DiagnosticsLogger.Avg;
        }

        [HttpGet]
        [Route("api/diagnostics/logs")]
        public string[] GetLogs()
        {
            return Diagnostics.DiagnosticsLogger.Logs;
        }
    }
}
