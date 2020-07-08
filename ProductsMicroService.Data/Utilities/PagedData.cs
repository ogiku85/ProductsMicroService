using System;
using System.Collections.Generic;
using System.Text;

namespace ProductsMicroService.Data.Utilities
{
     public class PagedData <T> where T : class
    {
        public IQueryParameters queryParameters { get; set; }
        public IEnumerable<T> PagedDataList { get; set; }
        public int DataCount { get; set; }
    }
}
