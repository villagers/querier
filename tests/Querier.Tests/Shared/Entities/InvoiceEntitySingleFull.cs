using Querier.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Mysqlx.Expect.Open.Types.Condition.Types;

namespace Querier.Tests.Shared.Entities
{
    [Query(
        Alias = "Invoices",
        Description = "Description",
        Key = "invoice",
        Table = "Invoice",
        RefreshInterval = "* * * * *",
        RefreshSql = "select MAX(`InvoiceDate`) from `Invoice`")]
    internal class InvoiceEntitySingleFull
    {
        [QueryDimension(Key = "id", Column = "InvoiceIdColumn", Description = "Description", Alias = "InvoiceIdAlias", Order = "asc")]
        public int InvoiceId { get; set; }
        [QueryDimension]
        public int CustomerId { get; set; }
        [QueryTimeDimension(Key = "invoice_date", Column = "InvoiceDateColumn", Description = "Description", Alias = "InvoiceDateAlias", Order = "asc", Granularity = "month")]
        public DateTime InvoiceDate { get; set; }
        [QueryDimension]
        public string BillingAddress { get; set; }
        [QueryDimension]
        public string BillingCity { get; set; }
        [QueryDimension]
        public string BillingState { get; set; }
        [QueryDimension]
        public string BillingCountry { get; set; }
        [QueryDimension]
        public string BillingPostalCode { get; set; }
        [QueryMeasure(Key = "total_key", Alias = "TotalAlias", Column = "TotalColumn", Description = "Description", Order = "desc", Aggregation = "sum")]
        public decimal Total { get; set; }
    }
}
