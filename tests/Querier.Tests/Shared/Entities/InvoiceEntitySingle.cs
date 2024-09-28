using Querier.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Tests.Shared.Entities
{
    [Query]
    internal class InvoiceEntitySingle
    {
        [QueryDimension]
        public int InvoiceId { get; set; }
        [QueryDimension]
        public int CustomerId { get; set; }
        [QueryTimeDimension]
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
        [QueryMeasure]
        public decimal Total { get; set; }
    }
}
