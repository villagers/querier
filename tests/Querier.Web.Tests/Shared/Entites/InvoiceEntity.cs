using Microsoft.AspNetCore.Http;
using Querier.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Web.Tests.Shared.Entites
{
    [Query(Key = "invoices")]
    public class InvoiceEntity
    {
        [QueryKey("IID")]
        [QueryDisplay("Invoice ID")]
        [QueryDimension(Key = "ID")]
        public int InvoiceId { get; set; }

        public int CustomerId { get; set; }

        [QueryTimeDimension]
        public DateTime InvoiceDate { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingCountry { get; set; }
        public string BillingPostalCode { get; set; }

        [QueryMeasure]
        public decimal Total { get; set; }
    }
}
