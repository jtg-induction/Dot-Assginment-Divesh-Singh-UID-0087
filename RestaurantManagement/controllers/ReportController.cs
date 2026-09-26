using RestaurantManagement.Models.Dto;
using RestaurantManagement.Models.Enum;
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
    [Authorize(Roles ="Owner")]
    [RoutePrefix("api/reports")]
    public class ReportController :ApiController
    {
        [HttpGet]
        [Route("Top-10-Most-Ordered-Items")]
        public HttpResponseMessage Top10MostOrderedItems([FromUri] ExportRequest exportRequest)
        {
            string reportpath = HostingEnvironment.MapPath($"~/Reports/Top10OrderItem.trdx");
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
            var exclude =exportRequest.OrderId;
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
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"application/{exportRequest.Format}");
            response.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = "Top10OrderItem" + "." + exportRequest.Format;


            return response;


        }

        [HttpGet]
        [Route("Frequently-Bought-Together")]
        public HttpResponseMessage FrequentlyBoughtTogether([FromUri]ExportRequest exportRequest)
        {
            string reportpath = HostingEnvironment.MapPath($"~/Reports/MostBroughtItem.trdx");
            if (string.IsNullOrEmpty(reportpath) || !File.Exists(reportpath))
            {
                throw new InvalidOperationException("File Not Exists");
            }
            Telerik.Reporting.Report report;
            using (FileStream stream = File.OpenRead(reportpath))
            {
                var serializer = new ReportXmlSerializer();
                report = (Telerik.Reporting.Report)serializer.Deserialize(stream);
            }
            var exclude =exportRequest.RestaurantId;
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
                    Name = "@RESTAURANTID",
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
            response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue($"application/{exportRequest.Format}");
            response.Content.Headers.ContentDisposition.FileName = "MostBroughtItem" + "." + exportRequest.Format;


            return response;


        }


    }
}