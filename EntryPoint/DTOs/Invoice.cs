using System;
using System.Collections.Generic;

namespace Test.DTOs
{
    public class Invoice
    {
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public string Supplier { get; set; }
        public List<InvoiceLine> Lines { get; set; }
    }
}