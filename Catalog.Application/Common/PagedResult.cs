using System;
using System.Collections.Generic;
using System.Text;

namespace e_bookshop.Catalog.Application.Common
{
    public class PagedResult<T>
    {
        public IEnumerable<T> Items { get; init; }
        public int TotalCount { get; init; }
        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }
}
