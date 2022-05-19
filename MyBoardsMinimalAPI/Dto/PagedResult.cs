using MyBoardsMinimalAPI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBoardsMinimalAPI.Dto
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemsFrom { get; set; }
        public int ItemsTo { get; set; }
        public int TotalItemsCount { get; set; }

        public PagedResult(List<T> items, int totalItemsCount, int pageSize, int pageNumber)
        {
            Items = items;
            TotalItemsCount = totalItemsCount;
            ItemsFrom = (pageSize * (pageNumber - 1)) + 1;
            ItemsTo = (ItemsFrom + pageSize) - 1;
            TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            //for int 12/5 = 2 || for double =12/5 = 2.4 so we need 3 pages
        }
    }
}
