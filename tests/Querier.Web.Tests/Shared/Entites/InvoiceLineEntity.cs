using Querier.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Querier.Web.Tests.Shared.Entites
{
    public class InvoiceLineEntity
    {
        public int InvoiceLineId { get; set; }

        public int InvoiceId { get; set; }
        public int TrackId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
    }
}
