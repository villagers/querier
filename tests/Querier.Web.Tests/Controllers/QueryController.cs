using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Querier.Web.Tests.Shared.Entites;

namespace Querier.Web.Tests.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueryController : ControllerBase
    {
        private readonly IQuery _query;

        public QueryController(IQuery query)
        {
            _query = query;
        }


        [HttpGet]
        [Route("measures")]
        public IActionResult GetMeasures()
        {
            var measures = _query.GetMeasures("invoices");
            return Ok(measures);
        }

        [HttpGet]
        [Route("dimensions")]
        public IActionResult GetDimensions()
        {
            var measures = _query.GetDimensions("InvoiceEntity");
            return Ok(measures);
        }

        [HttpGet]
        [Route("time-dimensions")]
        public IActionResult GetTimeDimensions()
        {
            var measures = _query.GetTimeDimensions("InvoiceEntity");
            return Ok(measures);
        }


        [HttpGet]
        [Route("query")]
        public IActionResult GetQuery()
        {
            try
            {
                var invoiceIds = new int[] { 1, 3, 5, 6 };
                var query = _query.From("Invoice");
                query.Filter(e => e.In("InvoiceId", invoiceIds));
                query.Filter(e => e.GreaterOrEqual(e => e.Date("InvoiceDate"), "2021-01-11"));


                query.MeasureSum("Total").Dimension("BillingCountry").TimeDimension("InvoiceDate");

                var result = query.Execute();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
    }
}
