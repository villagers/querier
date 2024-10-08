﻿using Dapper;
using DuckDB.NET.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Querier.Helpers;
using Querier.Interfaces;
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
        [Route("test")]
        public IActionResult Test()
        {
            var result = _query.From("invoices");
            return Ok();
        }

        [HttpGet]
        [Route("measures")]
        public IActionResult GetMeasures()
        {
            var measures = _query.GetMeasures<InvoiceEntity>();
            return Ok(measures);
        }

        [HttpGet]
        [Route("dimensions")]
        public IActionResult GetDimensions()
        {
            var measures = _query.GetDimensions("invoices");
            return Ok(measures);
        }

        [HttpGet]
        [Route("time-dimensions")]
        public IActionResult GetTimeDimensions()
        {
            var measures = _query.GetTimeDimensions("invoices");
            return Ok(measures);
        }


        [HttpGet]
        [Route("query")]
        public async Task<IActionResult> GetQuery()
        {
            try
            {
                var invoiceIds = new int[] { 1, 3, 5, 6 };
                var query = _query.From("InvoiceEntity");

                query.Measure("Total").Dimension("CustomerId").TimeDimension("InvoiceDate").OrderBy("Total", "asc").OrderBy("CustomerId", "asc");

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
