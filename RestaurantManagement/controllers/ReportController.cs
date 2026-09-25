using RestaurantManagement.Models.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.UI.WebControls;
using Telerik.Reporting;
using Telerik.Reporting.Processing;
using Telerik.Reporting.XmlSerialization;

namespace RestaurantManagement.Controllers
{
    [Authorize]
    [Route("api/reports")]
    public class ReportController :ApiController
    {
        [HttpGet]
        public HttpResponseMessage Report(ExportRequest exportRequest)
        {
            string reportpath = HostingEnvironment.MapPath($"~/Reports/{exportRequest.ReportId}.trdx");
            if(string.IsNullOrEmpty(reportpath) || !File.Exists(reportpath))
            {
                throw new InvalidOperationException("File Not Exists");
            }
            Telerik.Reporting.Report report;
            using (FileStream stream = File.OpenRead(reportpath))
            {
                var serializer = new ReportXmlSerializer();
                report = (Telerik.Reporting.Report)serializer.Deserialize(stream);
            }
            var exclude = String.Join(",", exportRequest.Parameters);
            var foundItems = report.Items.Find("sqlDataSource1", true);

            var sqlDataSource = report.GetDataSources()
      .OfType<Telerik.Reporting.SqlDataSource>()
      .FirstOrDefault(ds => ds.Name == "sqlDataSource1");

            if (sqlDataSource != null)
            {
                // 2. Clear any design-time variables configuration safely
                sqlDataSource.Parameters.Clear();

                // 3. Inject the clean runtime parameter token matching your SQL variable
                sqlDataSource.Parameters.Add(new Telerik.Reporting.SqlDataSourceParameter
                {
                    Name = "@EXCULDEITEM",
                    DbType = System.Data.DbType.String,
                    Value = exclude
                });
            }

            var reportsource = new InstanceReportSource();
            reportsource.ReportDocument = report;

            var processor = new ReportProcessor();
            RenderingResult result = processor.RenderReport(exportRequest.Format, reportsource, null);
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK);
            response.Content = new ByteArrayContent(result.DocumentBytes);
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = exportRequest.ReportId + "." + exportRequest.Format;


            return response;


        }

       
    }
}