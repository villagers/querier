using Querier.Attributes;

namespace Querier.Tests.Shared.Entities
{

    [QueryKey("invoice")]
    [QueryTable("Invoice")]
    [QueryAlias("Invoices")]
    [QueryDescription("Description")]
    [Query(RefreshInterval = "* * * * *", RefreshSql = "select MAX(`InvoiceDate`) from `Invoice`")]
    internal class InvoiceEntityMultipleFull
    {
        [QueryKey("id")]
        [QueryColumn("InvoiceIdColumn")]
        [QueryDescription("Description")]
        [QueryAlias("InvoiceIdAlias")]
        [QueryOrder("asc")]
        [QueryDimension()]
        public int InvoiceId { get; set; }
        [QueryDimension]
        public int CustomerId { get; set; }

        [QueryKey("invoice_date")]
        [QueryColumn("InvoiceDateColumn")]
        [QueryDescription("Description")]
        [QueryAlias("InvoiceDateAlias")]
        [QueryOrder("asc")]
        [QueryGranularity("month")]
        [QueryTimeDimension(Key = "", Column = "", Description = "", Alias = "", Order = "", Granularity = "")]
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

        [QueryKey("total_key")]
        [QueryColumn("TotalColumn")]
        [QueryDescription("Description")]
        [QueryAlias("TotalAlias")]
        [QueryOrder("desc")]
        [QueryAggregation("sum")]
        [QueryMeasure(Key = "", Alias = "", Column = "", Description = "", Order = "", Aggregation = "")]
        public decimal Total { get; set; }
    }
}
