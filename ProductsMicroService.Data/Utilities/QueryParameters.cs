using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsMicroService.Data.Utilities
{
    public class QueryParameters : IQueryParameters
    {
        public int PageSize { get; set; }
        public int Page { get; set; }
    }
}
